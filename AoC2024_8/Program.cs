using System.ComponentModel;
using System.Linq.Expressions;

namespace AoC2024_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = false;
            var path = test ? "Test" : "Real";
            var input = File.ReadAllLines($"{path}/input.txt").Reverse().ToArray();
            var antennas = new List<Antenna>();
            var mapYMax = input.Length - 1;
            var mapXMax = input[0].Length - 1;

            bool part1 = true;
            var p2ResultList = new List<Tuple<int, int>>();

            for (var y = 0; y <= mapYMax; y++)
            {
                var line = input[y];

                for (var x = 0; x <= mapXMax; x++)
                {
                    var position = line[x];

                    if (position != '.')
                    {
                        antennas.Add(new Antenna
                        {
                            Frequency = position.ToString(),
                            Coordinates = new Tuple<int, int>(x, y),
                            Antinodes = new List<AntiNode>()
                        });
                    }
                }
            }

            var frequencies = antennas.GroupBy(x => x.Frequency).Where(y => y.Count() > 1).ToList();

            foreach (var frequency in frequencies)
            {
                var antennasWithRightFrequency = antennas.Where(x => x.Frequency == frequency.Key).ToList();

                foreach (var headAntenna in antennasWithRightFrequency)
                {
                    p2ResultList.Add(headAntenna.Coordinates);

                    var headAntCoords = headAntenna.Coordinates;

                    foreach (var otherAntenna in antennasWithRightFrequency.Where(x => x.Coordinates.Item1 != headAntCoords.Item1 && x.Coordinates.Item2 != headAntCoords.Item2))
                    {
                        var othAntCoords = otherAntenna.Coordinates;

                        var delta = ReturnNextPosition(headAntCoords.Item1, headAntCoords.Item2, othAntCoords.Item1, othAntCoords.Item2);

                        var antiNodePosX = delta.Item1 + othAntCoords.Item1;
                        var antiNodePosY = delta.Item2 + othAntCoords.Item2;

                        if (part1 && 0 <= antiNodePosX && antiNodePosX <= mapXMax && 0 <= antiNodePosY && antiNodePosY <= mapYMax)
                        {
                            otherAntenna.Antinodes.Add(new AntiNode
                            {
                                AntiNodeForFrequency = frequency.Key,
                                Coordinates = new Tuple<int, int>(antiNodePosX, antiNodePosY)
                            });
                        }
                        else
                        {
                            var nextHeadX = headAntCoords.Item1;
                            var nextHeadY = headAntCoords.Item2;
                            var nextOtherX = othAntCoords.Item1;
                            var nextOtherY = othAntCoords.Item2;
                            var hasLeftMap = false;

                            while (!hasLeftMap)
                            {

                                delta = ReturnNextPosition(nextHeadX, nextHeadY, nextOtherX, nextOtherY);

                                nextHeadX = nextOtherX;
                                nextHeadY = nextOtherY;

                                antiNodePosX = delta.Item1 + nextOtherX;
                                antiNodePosY = delta.Item2 + nextOtherY;

                                nextOtherX = antiNodePosX;
                                nextOtherY = antiNodePosY;

                                if (0 <= antiNodePosX && antiNodePosX <= mapXMax && 0 <= antiNodePosY && antiNodePosY <= mapYMax)
                                {
                                    p2ResultList.Add(new Tuple<int, int>(antiNodePosX, antiNodePosY));
                                }
                                else
                                {
                                    hasLeftMap = true;
                                }
                                
                            }
                        }
                    }
                }
            }

            Console.WriteLine(part1
                ? antennas.SelectMany(a => a.Antinodes)
                    .DistinctBy(x => new { x.Coordinates.Item1, x.Coordinates.Item2 }).ToList().Count
              : p2ResultList.DistinctBy(x => new { x.Item1, x.Item2 }).ToList().Count);
        }

        public static Tuple<int, int> ReturnNextPosition(int headCurrentX, int headCurrentY, int otherCurrentX, int otherCurrentY)
        {
            var dx = otherCurrentX - headCurrentX;
            var dy = otherCurrentY - headCurrentY;

            return new Tuple<int, int>(dx, dy);
        }
    }

    public class Antenna
    {
        public string Frequency { get; set; }
        public Tuple<int, int> Coordinates { get; set; }
        public List<AntiNode> Antinodes { get; set; }
    }

    public class AntiNode
    {
        public string AntiNodeForFrequency { get; set; }
        public Tuple<int, int> Coordinates { get; set; }
    }
}