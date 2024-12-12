using System.ComponentModel;

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
                    var headAntCoords = headAntenna.Coordinates;

                    foreach (var otherAntenna in antennasWithRightFrequency.Where(x => x.Coordinates.Item1 != headAntCoords.Item1 && x.Coordinates.Item2 != headAntCoords.Item2))
                    {
                        var othAntCoords = otherAntenna.Coordinates;

                        var dx = othAntCoords.Item1 - headAntCoords.Item1;
                        var dy = othAntCoords.Item2 - headAntCoords.Item2;

                        //if (headAntCoords.Item1 == 8 && headAntCoords.Item2 == 3 && othAntCoords.Item1 == 9 &&
                        //    othAntCoords.Item2 == 2)
                        //{
                        //    Console.WriteLine("");
                        //}

                        var antiNodePosX = dx + othAntCoords.Item1;
                        var antiNodePosY = dy + othAntCoords.Item2;

                        if (0 <= antiNodePosX && antiNodePosX <= mapXMax && 0 <= antiNodePosY && antiNodePosY <= mapYMax)
                        {
                            otherAntenna.Antinodes.Add(new AntiNode()
                            {
                                AntiNodeForFrequency = frequency.Key,
                                Coordinates = new Tuple<int, int>(antiNodePosX, antiNodePosY)
                            });
                        }

                    }
                }

            }

            //p1
            Console.WriteLine(antennas.SelectMany(a => a.Antinodes).DistinctBy(x => new { x.Coordinates.Item1, x.Coordinates.Item2 }).ToList().Count);


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