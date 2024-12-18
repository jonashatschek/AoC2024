using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace AoC2024_11
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var useTestInput = false;
			var path = useTestInput ? "Test" : "Real";
			var stones = File.ReadAllText($"{path}/input.txt").Split(' ').ToList();
			var timer = new Stopwatch();
			timer.Start();
			var numberOfBlinks = 25;

			for (var y = 0; y < numberOfBlinks; y++)
			{
				var stoneCounterBeforeStart = stones.Count;
				if(y == 4)
				{
					Console.WriteLine();
				}
				for (int i = 0; i < stones.Count; i++)
				{
					var stone = stones[i];


					//-If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
					if (stone == "0")
					{
						//stone = "1";
						stones.RemoveAt(i);
						stones.Insert(i, "1");
						continue;
					}

					//-If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The
					//left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on
					//the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)

					if (stone.Length > 0 && stone.Length % 2 == 0)
					{
						var leftHalf = stone.Substring(0, stone.Length / 2);
						var rightHalfStart = stone.Length / 2;
						var rightHalf = stone.Substring(rightHalfStart, stone.Length - rightHalfStart);

						stones.RemoveAt(i);
						stones.Insert(i, $"{long.Parse(leftHalf)}");
						stones.Insert(i + 1, $"{long.Parse(rightHalf)}");
						i++;
						stoneCounterBeforeStart++;
						continue;
					}


					//-If none of the other rules apply, the stone is replaced by a new stone; the old stone's number
					//multiplied by 2024 is engraved on the new stone.

					var test = long.Parse(stone) * 2024;
					stones.RemoveAt(i);
					stones.Insert(i, test.ToString());

				}


			}

			timer.Stop();
			Console.WriteLine($"time ms: {timer.ElapsedMilliseconds}");
			Console.Write(stones.Count());

		}
	}

}