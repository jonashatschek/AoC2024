using System.Diagnostics;

namespace AoC2024_12
{
	internal class Program
	{
		static void Main(string[] args)
		{
            var timer = new Stopwatch();
            timer.Start();
            var useTestData = false;
            var path = useTestData ? "Test" : "Real";
            var farm = File.ReadAllLines($"{path}/input.txt");


            var crops = new List<Crop>();
            //count how many of each crop is in the farm
            var farmCount = new Dictionary<char, int>();

            var cols = farm.Length;
            for (int y = 0; y < cols; y++)
            {
                var row = farm[y].Length;

                for (int x = 0; x < row; x++)
                {
                    var crop = new Crop();

                }
            }


            //File.ReadAllText($"{path}/input.txt").Split(' ').Select(long.Parse).ToList()


        }


	}

    public class Crop
    {
        public char Type { get; set; }
        public int Perimiter { get; set; }
        public int Area { get; set; }
    }
}
