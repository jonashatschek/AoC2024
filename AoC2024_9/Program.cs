using System.Diagnostics;

namespace AoC2024_9
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var timer = new Stopwatch();
			timer.Start();

			var test = false;
			var path = test ? "Test" : "Real";
			var diskMap = File.ReadAllText($"{path}/input.txt").ToArray();
			var id = 0;
			int[] fileBlocks = [];

			for (int i = 0; i < diskMap.Length; i++)
			{
				fileBlocks = GetNewBlocks(fileBlocks, diskMap[i].ToString(), i, id);

				if (i % 2 == 0)
				{
					id++;
				}
			}

			bool allSorted = false;
			var indexOfLastInteger = fileBlocks.Length - 1;
			var indexOfFirstDot = 0;
			long checksum = 0;

			//p1
			while (!allSorted)
			{
				//slower
				//var indexOfFirstDot = Array.FindIndex(fileBlocks, x => x == ".");
				//indexOfLastInteger = Array.FindLastIndex(fileBlocks, x => x != ".");

				//faster

				for (; indexOfFirstDot < fileBlocks.Length; indexOfFirstDot++)
				{
					if (fileBlocks[indexOfFirstDot] < 0)
					{
						break;
					}
				}

				for (; indexOfLastInteger > 0; indexOfLastInteger--)
				{
					if (fileBlocks[indexOfLastInteger] > 0)
					{
						break;
					}
				}

				if (indexOfFirstDot > indexOfLastInteger)
				{
					allSorted = true;
				}
				else
				{
					fileBlocks[indexOfFirstDot] = fileBlocks[indexOfLastInteger];
					fileBlocks[indexOfLastInteger] = -1;
				}
			}

			for (int i = 0; i < indexOfLastInteger + 1; i++)
			{
				checksum += fileBlocks[i] * i;
			}

			timer.Stop();
			Console.WriteLine($"Part1 time: {timer.ElapsedMilliseconds}");
			Console.WriteLine(checksum);
		}

		public static int[] GetNewBlocks(int[] oldArray, string toAdd, int currentIndex, int id)
		{
			var toAddLength = int.Parse(toAdd);
			var newArray = new int[oldArray.Length + toAddLength];

			if (oldArray.Length > 0)
			{
				oldArray.CopyTo(newArray, 0);
			}

			int whereToStart = oldArray.Length > 0 ? oldArray.Length : 0;

			for (; whereToStart < newArray.Length; whereToStart++)
			{
				newArray[whereToStart] += currentIndex % 2 == 0 ? id : -1;
			}

			return newArray;
		}

	}

}
