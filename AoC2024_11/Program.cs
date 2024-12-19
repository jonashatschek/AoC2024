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

			var stoneCounts = new Dictionary<long, long>();

			foreach (var stone in input.Where(stone => !stoneCounts.TryAdd(stone, 1)))
            {
                stoneCounts[stone]++;
            }

			for (var y = 0; y < numberOfBlinks; y++)
			{
				if(y == 25)
				{
					Console.WriteLine($"Time p1: {timer.ElapsedMilliseconds}");
					Console.WriteLine($"Part 1: {stoneCounts.Values.Sum()}");
				}

				var newStoneCounts = new Dictionary<long, long>();

				foreach (var (stone, count) in stoneCounts)
				{
					if (stone == 0)
					{
                        if (!newStoneCounts.TryAdd(1, count))
                        {
                            newStoneCounts[1] += count;
                        }
                    }
					else
					{
						var stoneString = stone.ToString();

						if (stoneString.Length % 2 == 0)
						{
							var mid = stoneString.Length / 2;
							var leftHalf = long.Parse(stoneString.Substring(0, mid));
							var rightHalf = long.Parse(stoneString.Substring(mid));

                            if (!newStoneCounts.TryAdd(leftHalf, count))
                            {
								newStoneCounts[leftHalf] += count;
                            }

                            if (!newStoneCounts.TryAdd(rightHalf, count))
                            {
								newStoneCounts[rightHalf] += count;
                            }
						
						}
						else
						{
							var multiplied = stone * 2024;

                            if (!newStoneCounts.TryAdd(multiplied, count))
                            {
								newStoneCounts[multiplied] += count;
                            }
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