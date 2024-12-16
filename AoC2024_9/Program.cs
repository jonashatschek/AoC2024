using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AoC2024_9
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var timer = new Stopwatch();
			timer.Start();

			var test = true;
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

			bool p1AllSorted = false;
			var indexOfLastInteger = fileBlocks.Length - 1;
			var indexOfFirstDot = 0;
			long checksum = 0;

			bool part1 = false;
			if (part1)
			{
				while (!p1AllSorted)
				{

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
						p1AllSorted = true;
					}
					else
					{
						fileBlocks[indexOfFirstDot] = fileBlocks[indexOfLastInteger];
						fileBlocks[indexOfLastInteger] = -1;
					}
				}
			}

			var part2 = true;

			if (part2)
			{
				var occupiedBlocks = fileBlocks.Select((x, i) => new { FileId = x, Block = i }).Where(x => x.FileId > 0).GroupBy(x => x.FileId).Select(x => x).ToList();
				var occupiedBlocks2 = fileBlocks.Select((x, i) => new { FileId = x, Block = i }).Where(x => x.FileId > 0).Select(x => new { x.FileId, x.Block }).ToList();

				for (int i = occupiedBlocks.Count() - 1; 0 <= i; i--)
				{
					var blockSize = occupiedBlocks[i].Count();

					var freeBlocks = fileBlocks.Select((x, i) => new { FileId = x, Block = i }).Where(x => x.FileId == -1).Select(x => x.Block).ToList();

					for (int j = 0; j < freeBlocks.Count() - 1; j++)
					{

						if (freeBlocks.Count() > j + blockSize)
						{

							if (freeBlocks[j + blockSize - 1] == freeBlocks[j] + blockSize - 1)
							{
								var freeBlockStartingElement = freeBlocks[j];
								var occupiedBlockStartingElement = occupiedBlocks2.Where(x => x.FileId == i + 1).OrderBy(x => x.Block).Select(x => x.Block).FirstOrDefault();

								if (freeBlockStartingElement < occupiedBlockStartingElement)
								{
									for (int k = freeBlockStartingElement; k < freeBlockStartingElement + blockSize; k++)
									{
										fileBlocks[k] = occupiedBlocks[i].Key;
									}

									for (int o = occupiedBlockStartingElement; o < occupiedBlockStartingElement + blockSize; o++)
									{
										fileBlocks[o] = -1;
									}

								}

								break;
							}

						}

					}

				}

			}


			if (part1)
			{
				for (int i = 0; i < indexOfLastInteger + 1; i++)
				{
					checksum += fileBlocks[i] * i;
				}


				Console.WriteLine($"Part 1 result: {checksum}");
			}

			if (part2)
			{
				long p2checksum = 0;
				foreach (var res in fileBlocks.Select((x, i) => new { FileId = x, Block = i }).Where(x => x.FileId > -1))
				{
					p2checksum += res.FileId * res.Block;
				}

				Console.WriteLine($"Part 2 result: {p2checksum}");
			}

			timer.Stop();

			Console.WriteLine($"timer: {timer.ElapsedMilliseconds}");

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
