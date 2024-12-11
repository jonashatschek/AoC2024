using System.ComponentModel;

namespace AoC2024_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = true;

            var path = test ? "Test" : "Real";

            var input = File.ReadAllLines($"{path}/input.txt");

            var antennas = new List<Antenna>();


            //todo add all antennas to a list
            for (var y = 0; y < input.Length; y++)
            {
                var line = input[y];

                for (var x = 0; x < line.Length; x++)
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

            //todo select all antennas that count > 1
            var frequencies = antennas.GroupBy(x => x.Frequency).Where(y => y.Count() > 1).ToList();

            foreach (var frequency in frequencies)
            {

                //for every antenna in antenna group, calculate distance to all other antennas with same Id

                var antennasWithRightFrequency = antennas.Where(x => x.Frequency == frequency.Key).ToList();


                foreach (var headAntenna in antennasWithRightFrequency)
                {
                    var headAntCoords = headAntenna.Coordinates;

                    foreach (var otherAntenna in antennasWithRightFrequency.Where(x => x.Coordinates.Item1 != headAntCoords.Item1 && x.Coordinates.Item2 != headAntCoords.Item2))
                    {
                        var dx = headAntCoords.Item1 - otherAntenna.Coordinates.Item1;
                        var dy = headAntCoords.Item2 - otherAntenna.Coordinates.Item2;

                        headAntenna.Antinodes.Add(new AntiNode()
                        {
                            AntiNodeForFrequency = frequency.Key,
                            Coordinates = new Tuple<int, int>(dy * 2, dx * 2)
                        });
                    }
                }

                //for (int i = 0; i < antennasWithRightFrequency.Count - 1; i++)
                //{
                //    //var dx = Math.Abs(antennasWithRigthFrequency[i].Coordinates.Item1 - antennasWithRigthFrequency[i + 1].Coordinates.Item1);
                //    //var dy = Math.Abs(antennasWithRigthFrequency[i].Coordinates.Item2 - antennasWithRigthFrequency[i + 1].Coordinates.Item2);
                //    var dx = antennasWithRightFrequency[i].Coordinates.Item1 - antennasWithRightFrequency[i + 1].Coordinates.Item1;
                //    var dy = antennasWithRightFrequency[i].Coordinates.Item2 - antennasWithRightFrequency[i + 1].Coordinates.Item2;

                //    antennasWithRightFrequency[i].Antinodes.Add(new AntiNode()
                //    {
                //        AntiNodeForFrequency = frequency.Key,
                //        Coordinates = new Tuple<int, int>(dy * 2, dx * 2)
                //    });

                //}

                //foreach (var antenna in antennas.Where(x => x.Frequency == frequency.Key))
                //{



                //}

                //when distance to other antennas with same Ids is known, an antinode should be added.

                //only locate antennas within map size



            }

            var count = antennas.Select(x => x.Antinodes).Count().ToString();

            Console.WriteLine(count);
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