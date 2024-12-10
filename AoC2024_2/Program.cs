namespace aoc2024_2
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var p = new Program();

			var input = File.ReadLines("input.txt");

			var safeReports = 0;

			foreach (var report in input)
			{
				var levels = Array.ConvertAll(report.Split(' '), int.Parse);
				if (p.IsSafe(levels))
				{
					safeReports++;
				}
				else
				{
					for (var i = 0; i < levels.Length; i++)
					{
						var updatedLevels = levels.Where((c, index) => index != i).ToArray();

						if (p.IsSafe(updatedLevels))
						{
							safeReports++;
							i = levels.Length;
						}
					}
				}

			}

			Console.WriteLine(safeReports);
		}

		public bool IsSafe(int[] row)
		{
			var isDecreasing = false;
			var isIncreasing = false;

			for (var i = 0; i < row.Length - 1; i++)
			{
				var left = row[i];
				var right = row[i + 1];

				var diff = left - right;

				switch (diff)
				{
					case > 0:
						isIncreasing = true;
						break;
					case < 0:
						isDecreasing = true;
						break;
				}

				if (Math.Abs(diff) > 3 || Math.Abs(diff) < 1)
				{
					return false;
				}

				if (isIncreasing && isDecreasing)
				{
					return false;
				}

			}

			return true;
		}
	}
}