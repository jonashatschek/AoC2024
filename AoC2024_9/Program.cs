using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace AoC2024_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = true;
            var path = test ? "Test" : "Real";
            var diskMap = File.ReadAllText($"{path}/input.txt").ToArray();
            var id = 0;
            string[] fileBlocks = [];

            for (int i = 0; i < diskMap.Length; i++)
            {
                fileBlocks = GetNewBlocks(fileBlocks, diskMap[i].ToString(), i, id);

				if (i % 2 == 0)
				{
					id++;
				}
            }

            bool allSorted = false;
			var indexOfLastInteger = 0;

            //p1
			while (!allSorted)
            {
				var indexOfFirstDot = Array.FindIndex(fileBlocks, x => x == ".");
				indexOfLastInteger = Array.FindLastIndex(fileBlocks, x => x != ".");

                if(indexOfFirstDot > indexOfLastInteger)
                {
                    allSorted = true;
                }
                else
                {
                    fileBlocks[indexOfFirstDot] = fileBlocks[indexOfLastInteger];
                    fileBlocks[indexOfLastInteger] = ".";
                }
			}

            long checksum = 0;

            for (int i = 0; i < indexOfLastInteger + 1; i++)
            {
                long res = long.Parse(fileBlocks[i]) * i;
                checksum += res;
            }

            Console.WriteLine(checksum);
        }

        public static string[] GetNewBlocks(string[] oldStuff, string toAdd, int currentIndex, int id)
        {
            var toAddLength = int.Parse(toAdd);
            var newStuff = new string[oldStuff.Length + toAddLength];

            if (oldStuff.Length > 0)
            {
                oldStuff.CopyTo(newStuff, 0);
            }

            int whereToStart = oldStuff.Length > 0 ? oldStuff.Length : 0;

            for (; whereToStart < newStuff.Length; whereToStart++)
            {
                newStuff[whereToStart] += currentIndex % 2 == 0 ? id : ".";
            }

            return newStuff;
        }

	}

}
