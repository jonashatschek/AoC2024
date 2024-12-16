using System.Diagnostics;

namespace AoC2024_9
{
    internal class Program
    { 
        static void Main(string[] args)
        {

            var timer = new Stopwatch();
            timer.Start();
            var useTestData = false;
            var path = useTestData ? "Test" : "Real";
            var diskMap = File.ReadAllText($"{path}/input.txt").ToArray();

            var id = 0;
            int currentIndex = 0;
            var freeBlocks = new List<MemoryBlock>();
            var occupiedBlocks = new List<MemoryBlock>();

            for (int i = 0; i < diskMap.Length; i++)
            {
                var length = diskMap[i] - '0';
                var list = i % 2 == 0 ? occupiedBlocks : freeBlocks;

                if (length > 0)
                {
                    list.Add(new MemoryBlock { Id = id, Length = length, StartIndex = currentIndex });
                }

                currentIndex += length;
                if (i % 2 == 0)
                {
                    id++;
                }
            }

            Part1(freeBlocks, occupiedBlocks);

            Part2(freeBlocks, occupiedBlocks);
            
            timer.Stop();
            Console.WriteLine($"timer: {timer.ElapsedMilliseconds}");
        }

        public static int[] indexOfNextFreeMemoryOfSize = new int[10];

        public static MemoryBlock FindNext(List<MemoryBlock> list, int length, int maxIndex)
        {
            for (int i = indexOfNextFreeMemoryOfSize[length]; i < list.Count; i++)
            {
                if (list[i].StartIndex > maxIndex)
                    return null;
                if (list[i].Length >= length)
                {
                    indexOfNextFreeMemoryOfSize[length] = i;
                    return list[i];
                }
            }
            return null;
        }

        public static void Part1(List<MemoryBlock> freeBlocks, List<MemoryBlock> occupiedBlocks)
        {
            decimal checksum = 0;

            while (true)
            {
                var lastOccupied = occupiedBlocks.Last();
                var firstFreeBlock = freeBlocks.First();

                if (firstFreeBlock.StartIndex > lastOccupied.StartIndex)
                {
                    break;
                }

                var lengthToMove = lastOccupied.Length;

                if (firstFreeBlock.Length < lengthToMove)
                {
                    lengthToMove = firstFreeBlock.Length;
                }

                checksum += (decimal)lastOccupied.Id * (firstFreeBlock.StartIndex * lengthToMove + (lengthToMove * (lengthToMove - 1)) / 2);

                lastOccupied.Length -= lengthToMove;
                firstFreeBlock.Length -= lengthToMove;
                firstFreeBlock.StartIndex += lengthToMove;

                if (lastOccupied.Length == 0)
                {
                    occupiedBlocks.RemoveAt(occupiedBlocks.Count - 1);
                }

                if (firstFreeBlock.Length == 0)
                {
                    freeBlocks.RemoveAt(0);
                }
            }

            checksum += occupiedBlocks.Sum(x => (decimal)x.Id * (x.StartIndex * x.Length + (x.Length * (x.Length - 1)) / 2));

            Console.WriteLine($"Part 1 result: {checksum}");
        }

        public static void Part2(List<MemoryBlock> freeBlocks, List<MemoryBlock> occupiedBlocks)
        {
            decimal checksum = 0;
            for (int i = occupiedBlocks.Count - 1; 0 <= i; i--)
            {
                var blockSize = occupiedBlocks[i].Length;
                var availableFreeBlock = FindNext(freeBlocks, blockSize, occupiedBlocks[i].StartIndex);

                if (availableFreeBlock == null)
                {
                    checksum += (decimal)(occupiedBlocks[i].Id) * (occupiedBlocks[i].StartIndex * occupiedBlocks[i].Length + (occupiedBlocks[i].Length * (occupiedBlocks[i].Length - 1)) / 2);
                    continue;
                }

                checksum += (decimal)(occupiedBlocks[i].Id) * (availableFreeBlock.StartIndex * occupiedBlocks[i].Length + (occupiedBlocks[i].Length * (occupiedBlocks[i].Length - 1)) / 2);

                for (int k = 0; k < blockSize; k++)
                {
                    availableFreeBlock.StartIndex++;
                    availableFreeBlock.Length--;
                }
            }

            Console.WriteLine($"Part 2 result: {checksum}");
        }

    }


    public class MemoryBlock
    {
        public int Id;
        public int StartIndex;
        public int Length;
    }
}