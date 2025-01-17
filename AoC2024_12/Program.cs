using System.Diagnostics;

namespace AoC2024_12
{
    internal class Program
    {
        public static List<Region> Regions = [];
        public static bool useTestData = false;
        public static string path = useTestData ? "Test" : "Real";
        public static string[] farm = File.ReadAllLines($"{path}/input.txt").Reverse().ToArray();
        public static readonly List<int[]> Directions = [[0, 1], [1, 1], [1, 0], [1, -1], [0, -1], [-1, -1], [-1, 0], [-1, 1]]; //n, ne, e, se, s, sw, w, nw
        public static bool[,] Visited;

        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();
            Visited = new bool[farm.Length, farm[0].Length];

            for (var y = 0; y < farm.Length; y++)
            {
                var row = farm[y];
                for (var x = 0; x < row.Length; x++)
                {
                    if (!Visited[x, y])
                    {
                        var region = new Region
                        {
                            Type = farm[y][x]
                        };
                        Regions.Add(region);
                        Visited[x, y] = true;
                        MapAllCropsWithinArea(x, y, region);
                    }
                }
            }

            var p1Result = Regions.Sum(region => region.Plants * region.Perimiter);
            Console.WriteLine($"Time (ms): {timer.ElapsedMilliseconds} \nPart 1: {p1Result}");

            CountSides();

            var p2Result = Regions.Sum(region => region.Sides * region.Plants);
            timer.Stop();
            Console.WriteLine($"Time (ms): {timer.ElapsedMilliseconds} \nPart 2: {p2Result}");
        }

        public static void MapAllCropsWithinArea(int x, int y, Region region)
        {
            region.Plants++;

            for (var index = 0; index < Directions.Count; index++)
            {
                var direction = Directions[index];
                var dx = direction[0];
                var dy = direction[1];

                var adjacentX = dx + x;
                var adjacentY = dy + y;
                
                if (!IsPartOfRegion(adjacentX, adjacentY, region.Type))
                {
                    if (index % 2 == 0)
                    {
                        region.Perimiter++;
                    }

                    if (!region.Coordinates.Any(coord => coord[0] == x && coord[1] == y))
                    {
                        region.Coordinates.Add([x, y]);
                    }
                }
                else if (!Visited[adjacentX, adjacentY])
                {
                    Visited[adjacentX, adjacentY] = true;
                    MapAllCropsWithinArea(adjacentX, adjacentY, region);
                }
            }
        }

        public static bool IsPartOfRegion(int x, int y, char region)
        {
            return !(y < 0 || y >= farm.Length || x < 0 || x >= farm[y].Length) && farm[y][x] == region;
        }

        public static void CountSides()
        {
            foreach (var region in Regions)
            {
                foreach (var coordinate in region.Coordinates)
                {
                    for (var index = 0; index < Directions.Count; index += 2)
                    {
                        var x = coordinate[0];
                        var y = coordinate[1];

                        var firstDirection = index < 6 ? Directions[index + 2] : Directions[0];
                        var firstX = x + firstDirection[0];
                        var firstY = y + firstDirection[1];

                        var intermediateDirection = Directions[index + 1];
                        var interX = x + intermediateDirection[0];
                        var interY = y + intermediateDirection[1];

                        var secondDirection = Directions[index];
                        var secondX = x + secondDirection[0];
                        var secondY = y + secondDirection[1];

                        var isFirstCoordinatePartOfRegion = IsPartOfRegion(firstX, firstY, region.Type);
                        var isSecondCoordinatePartOfRegion = IsPartOfRegion(secondX, secondY, region.Type);

                        if (!isFirstCoordinatePartOfRegion && !isSecondCoordinatePartOfRegion)
                        {
                            region.Sides++;
                        }
                        else if (isFirstCoordinatePartOfRegion && isSecondCoordinatePartOfRegion && farm[interY][interX] != region.Type)
                        {
                            region.Sides++;
                        }
                    }
                }
            }
        }
    }

    public class Region
    {
        public char Type { get; set; }
        public int Perimiter { get; set; }
        public int Plants { get; set; }
        public int Sides { get; set; }
        public List<int[]> Coordinates = [];

    }
}