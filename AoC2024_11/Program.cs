using System.Collections.Concurrent;
using System.Diagnostics;

namespace AoC2024_11
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var useTestInput = false;
			var path = useTestInput ? "Test" : "Real";
			var input = File.ReadAllText($"{path}/input.txt").Split(' ').Select(long.Parse).ToList();

			var timer = new Stopwatch();
			timer.Start();

			var numberOfBlinks = 75;

			var stoneCounts = new ConcurrentDictionary<long, long>();

			foreach (var stone in input)
			{
				stoneCounts[stone] = stoneCounts.GetOrAdd(stone, 0) + 1;
			}

			for (var y = 0; y < numberOfBlinks; y++)
			{
				if(y == 25)
				{
					Console.WriteLine($"Time p1: {timer.ElapsedMilliseconds}");
					Console.WriteLine($"Part 1: {stoneCounts.Values.Sum()}");
				}

				var newStoneCounts = new ConcurrentDictionary<long, long>();

				foreach (var (stone, count) in stoneCounts)
				{
					if (stone == 0)
					{
						newStoneCounts[1] = newStoneCounts.GetOrAdd(1, 0) + count;
					}
					else
					{
						var stoneString = stone.ToString();

						if (stoneString.Length % 2 == 0)
						{
							var mid = stoneString.Length / 2;
							var leftHalf = long.Parse(stoneString.Substring(0, mid));
							var rightHalf = long.Parse(stoneString.Substring(mid));

							newStoneCounts[leftHalf] = newStoneCounts.GetOrAdd(leftHalf, 0) + count;
							newStoneCounts[rightHalf] = newStoneCounts.GetOrAdd(rightHalf, 0) + count;
						}
						else
						{
							var multiplied = stone * 2024;
							newStoneCounts[multiplied] = newStoneCounts.GetOrAdd(multiplied, 0) + count;
						}
					}
				}

				stoneCounts = newStoneCounts;
			}

			timer.Stop();
			Console.WriteLine($"Time p2: {timer.ElapsedMilliseconds}");
			Console.WriteLine($"Part 2: {stoneCounts.Values.Sum()}");
		}
	}

}