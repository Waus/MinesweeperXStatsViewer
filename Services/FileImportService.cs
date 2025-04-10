using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using MinesweeperXStatsViewer.Models;

namespace MinesweeperXStatsViewer.Services
{
	public static class FileImportService
	{
		public static List<StatsItem> LoadStatsFromFile(string filePath)
		{
			// Try to detect BOM, fallback to Windows-1250 if none
			Encoding encoding = DetectEncodingOrDefault(filePath, Encoding.GetEncoding("Windows-1250"));

			var lines = File.ReadAllLines(filePath, encoding).Skip(1);

			var allowedLevels = new HashSet<string> { "Beg", "Int", "Exp" };

			var statsItemsList = new List<StatsItem>();

			try
			{
				statsItemsList = lines
				.Where(line => !string.IsNullOrWhiteSpace(line))
				.Select(ParseLineToDTO)
				.Where(dto => dto != null
							  && allowedLevels.Contains(dto.Level)
							  && dto.Time == dto.Est)
				.Select(MapDtoToModel)
				.ToList();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Please configure month names in the settings correctly before loading the file.",
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}

			var sortedBegStatsItems = statsItemsList
				.Where(a => a.Level == LevelEnum.Beg)
				.OrderBy(item => item.Time)
				.ToList();

			var sortedIntStatsItems = statsItemsList
				.Where(a => a.Level == LevelEnum.Int)
				.OrderBy(item => item.Time)
				.ToList();

			var sortedExpStatsItems = statsItemsList
				.Where(a => a.Level == LevelEnum.Exp)
				.OrderBy(item => item.Time)
				.ToList();

			var BegStatsItemsWithTimeRank = SetTimeRanks(sortedBegStatsItems);
			var IntStatsItemsWithTimeRank = SetTimeRanks(sortedIntStatsItems);
			var ExpStatsItemsWithTimeRank = SetTimeRanks(sortedExpStatsItems);

			sortedBegStatsItems = BegStatsItemsWithTimeRank
				.OrderByDescending(item => item.BBBVPerSec)
				.ToList();

			sortedIntStatsItems = ExpStatsItemsWithTimeRank
				.OrderByDescending(item => item.BBBVPerSec)
				.ToList();

			sortedExpStatsItems = IntStatsItemsWithTimeRank
				.OrderByDescending(item => item.BBBVPerSec)
				.ToList();

			var BegStatsItemsWithAllRanks = SetBBBVPerSecRanks(sortedBegStatsItems);
			var IntStatsItemsWithAllRanks = SetBBBVPerSecRanks(sortedIntStatsItems);
			var ExpStatsItemsWithAllRanks = SetBBBVPerSecRanks(sortedExpStatsItems);

			var allStatsItemsWithRank = BegStatsItemsWithAllRanks
			.Concat(IntStatsItemsWithAllRanks)
			.Concat(ExpStatsItemsWithAllRanks)
			.ToList();

			return allStatsItemsWithRank;
		}

		private static Encoding DetectEncodingOrDefault(string filePath, Encoding defaultEncoding)
		{
			// Read BOM manually
			var bom = new byte[4];
			using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			{
				file.Read(bom, 0, 4);
			}

			// Detect encoding by BOM
			if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
			if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
			if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode;        // UTF-16 LE
			if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; // UTF-16 BE
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

			// No BOM found – fallback
			return defaultEncoding;
		}



		private static StatsItemDTO ParseLineToDTO(string line)
		{
			var columns = line.Split('\t');
			if (columns.Length < 14)
				return null;

			return new StatsItemDTO
			{
				Level = columns[0],
				Height = int.Parse(columns[1]),
				Width = int.Parse(columns[2]),
				Mines = int.Parse(columns[3]),
				Time = double.Parse(columns[4].Replace(',', '.'), CultureInfo.InvariantCulture),
				BBBV = int.Parse(columns[5]),
				Solved = int.Parse(columns[6]),
				BBBVPerSec = double.Parse(columns[7].Replace(',', '.'), CultureInfo.InvariantCulture),
				Est = double.Parse(columns[8].Replace(',', '.'), CultureInfo.InvariantCulture),
				Left = int.Parse(columns[9]),
				Middle = int.Parse(columns[10]),
				Right = int.Parse(columns[11]),
				Date = columns[12],
				Started = columns[13]
			};
		}

		private static StatsItem MapDtoToModel(StatsItemDTO dto)
		{
			return new StatsItem
			{
				Level = Enum.Parse<LevelEnum>(dto.Level),
				Time = dto.Time,
				BBBV = dto.BBBV,
				BBBVPerSec = dto.BBBVPerSec,
				Left = dto.Left,
				Middle = dto.Middle,
				Right = dto.Right,
				DateTime = ParseCustomDateTime(dto.Date, dto.Started)
			};
		}

		private static DateTime ParseCustomDateTime(string rawDate, string rawTime)
		{
			var months = new Dictionary<string, int>
				{
					{ Properties.Settings.Default.January, 1 },
					{ Properties.Settings.Default.February, 2 },
					{ Properties.Settings.Default.March, 3 },
					{ Properties.Settings.Default.April, 4 },
					{ Properties.Settings.Default.May, 5 },
					{ Properties.Settings.Default.June, 6 },
					{ Properties.Settings.Default.July, 7 },
					{ Properties.Settings.Default.August, 8 },
					{ Properties.Settings.Default.September, 9 },
					{ Properties.Settings.Default.October, 10 },
					{ Properties.Settings.Default.November, 11 },
					{ Properties.Settings.Default.December, 12 }
				};

			var dateParts = rawDate.Split(' ');
			if (dateParts.Length != 3)
				throw new FormatException($"Invalid date format: {rawDate}");

			if (!months.TryGetValue(dateParts[0].Trim(), out int month))
				throw new KeyNotFoundException($"Unrecognized month name '{dateParts[0]}' in date: {rawDate}");

			int day = int.Parse(dateParts[1]);
			int year = int.Parse(dateParts[2]);

			if (!DateTime.TryParseExact(
					rawTime,
					"hh:mm:ss tt",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None,
					out DateTime timePart))
			{
				throw new FormatException($"Invalid time format: '{rawTime}' (expected format like 12:34:56 AM)");
			}

			return new DateTime(year, month, day, timePart.Hour, timePart.Minute, timePart.Second);
		}

		private static List<StatsItem> SetTimeRanks(List<StatsItem> sortedStatsItems)
		{
			double previousTime = -1;
			int rank = 0;
			int rankModifier = 1;
			foreach (var statsItem in sortedStatsItems)
			{
				if (statsItem.Time == previousTime)
				{
					statsItem.TimeRank = rank;
					rankModifier++;
				}
				else
				{
					rank += rankModifier;
					statsItem.TimeRank = rank;
					previousTime = statsItem.Time;
					rankModifier = 1;
				}
			}
			return sortedStatsItems;
		}

		private static List<StatsItem> SetBBBVPerSecRanks(List<StatsItem> sortedStatsItems)
		{
			double previousBBBVPerSec = -1;
			int rank = 0;
			int rankModifier = 1;
			foreach (var statsItem in sortedStatsItems)
			{
				if (statsItem.BBBVPerSec == previousBBBVPerSec)
				{
					statsItem.BBBVPerSecRank = rank;
					rankModifier++;
				}
				else
				{
					rank += rankModifier;
					statsItem.BBBVPerSecRank = rank;
					previousBBBVPerSec = statsItem.BBBVPerSec;
					rankModifier = 1;
				}
			}
			return sortedStatsItems;
		}


	}
}
