using System.Diagnostics;

namespace AoC2024_10
{
	internal class Program
	{

		public static List<int[]> Directions = [[1, 0], [-1, 0], [0, 1], [0, -1]];
		public static HashSet<int> foundNines = [];
		public static Dictionary<int, HashSet<int>> numbers = [];
		public static int sumOfTrailheadRatings = 0;

		static void Main(string[] args)
		{
			var timer = new Stopwatch();
			timer.Start();

			for (int i = 0; i < 10; i++)
			{
				numbers[i] = [];
			}

			var useTestInput = false;
			var path = useTestInput ? "Test" : "Real";
			var input = File.ReadAllLines($"{path}/input.txt");

			for (int y = 0; y < input.Length; y++)
			{
				var row = input[y];
				for (int x = 0; x < row.Length; x++)
				{
					var num = row[x] - '0';
					numbers[num].Add(x * 100 + y);

				}
			}

			var score = 0;

			foreach (var startingPoint in numbers[0])
			{
				ExploreNeighbours(startingPoint, 0);

				score += foundNines.Count;
				foundNines.Clear();
			}

			timer.Stop();


			Console.WriteLine($"p1: {score}");

			Console.WriteLine($"p1 ms: {timer.ElapsedMilliseconds}");

			Console.WriteLine(sumOfTrailheadRatings);

		}

		public static void ExploreNeighbours(int currentPoint, int current)
		{
			if (current == 9)
			{
				foundNines.Add(currentPoint);
				return;
			}

			var find = current + 1;

			foreach (var direction in Directions)
			{
				var newPoint = currentPoint + (direction[0] * 100) + direction[1];

				if (numbers[find].Contains(newPoint))
				{
					ExploreNeighbours(newPoint, find);

					if (find == 9)
					{
						sumOfTrailheadRatings++;
					}
				}

			}
		}
	}
}