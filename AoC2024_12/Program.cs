using System.Diagnostics;
using System.Net.WebSockets;

namespace AoC2024_12
{
	internal class Program
	{
		public static List<Region> Regions = [];
		public static bool useTestData = true;
		public static string path = useTestData ? "Test" : "Real";
		public static string[] farm = File.ReadAllLines($"{path}/input.txt").Reverse().ToArray();
		public static readonly int[,] Directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } }; // e, w, n, s
		public static int[] PerimiterTypes = [0, 1, 2, 3]; //e, w, n, s
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
					var plant = new Plant
					{
						Coordinate = new Coordinate(x, y),
						Type = farm[y][x]
					};

					if (!Visited[x, y])
					{
						var region = CreateNewPlantRegion(farm[y][x]);
						Regions.Add(region);
						Visited[x, y] = true;
						MapAllCropsWithinArea(x, y, plant, region);
					}
				}
			}

			var p1Result = 0;
			foreach (var region in Regions)
			{
				p1Result += region.Plants.Count * region.Perimiter;
			}
			Console.WriteLine($"Time (ms): {timer.ElapsedMilliseconds}");
			Console.WriteLine($"part 1: {p1Result}");

			foreach (var region in Regions)
			{

				foreach (var perimiterType in PerimiterTypes)
				{
					var plantsInRegionWithSameDirection = region.Plants.Where(plants => plants.Perimiters.Any(plantPerimiter => plantPerimiter == perimiterType));

					var splitBySides = new List<IEnumerable<IEnumerable<int>>>();
					int solitarySidePerimiter = 0;

					if (perimiterType == 0 || perimiterType == 1)
					{
						var perimitersOnSameRow = plantsInRegionWithSameDirection.OrderBy(plants => plants.Coordinate.Y)
							.GroupBy(plants => plants.Coordinate.X).Where(plants => plants.Count() > 1)
							.Select(x => x.Select(x => x.Coordinate)).ToList();

						splitBySides = perimitersOnSameRow.Select(x => x.Select(x => x.Y)
								.Select((value, index) => new { value, groupKey = value - index })
								.GroupBy(x => x.groupKey)
								.Select(group => group.Select(x => x.value))).ToList();

						solitarySidePerimiter = plantsInRegionWithSameDirection.GroupBy(plants => plants.Coordinate.X)
							.Where(plants => plants.Count() == 1).Count();

					}

					if (perimiterType == 2 || perimiterType == 3)
					{
						var perimitersOnSameRow = plantsInRegionWithSameDirection.OrderBy(plants => plants.Coordinate.X)
							.GroupBy(plants => plants.Coordinate.Y).Where(plants => plants.Count() > 1).Select(x => x.Select(x => x.Coordinate)).ToList();

						splitBySides = perimitersOnSameRow.Select(x => x.Select(x => x.X)
						.Select((value, index) => new { value, groupKey = value - index })
						.GroupBy(x => x.groupKey)
						.Select(group => group.Select(x => x.value))).ToList();

						solitarySidePerimiter = plantsInRegionWithSameDirection.GroupBy(plants => plants.Coordinate.Y)
							.Where(plants => plants.Count() == 1).Count();

					}

					var sideCounter = 0;

					for (int i = 0; i < splitBySides.Count; i++)
					{
						sideCounter += splitBySides[i].Count();
					}

					region.Sides += sideCounter;
					region.Sides += solitarySidePerimiter;
				}
			}

			var p2Result = 0;
			foreach (var region in Regions)
			{
				p2Result += region.Sides * region.Plants.Count;
			}

			timer.Stop();

			Console.WriteLine($"Time (ms): {timer.ElapsedMilliseconds}");
			Console.WriteLine($"part 2: {p2Result}");
		}

		public static void MapAllCropsWithinArea(int startingX, int startingY, Plant plant, Region region)
		{
			region.Plants.Add(plant);

			for (int direction = 0; direction < 4; direction++)
			{
				var dx = Directions[direction, 0];
				var dy = Directions[direction, 1];

				var adjacentX = dx + startingX;
				var adjacentY = dy + startingY;

				var isOutsideMapEdges = adjacentY < 0 || adjacentY >= farm.Length || adjacentX < 0 || adjacentX >= farm[startingY].Length;
				var isNeighbourSameAsCurrentPosition = false;

				if (!isOutsideMapEdges)
				{
					isNeighbourSameAsCurrentPosition = farm[startingY][startingX] == farm[adjacentY][adjacentX];
				}

				if (isOutsideMapEdges || !isNeighbourSameAsCurrentPosition)
				{
					region.Perimiter++;
					plant.Perimiters.Add(direction);
				}

				if (!isOutsideMapEdges && isNeighbourSameAsCurrentPosition && !Visited[adjacentX, adjacentY])
				{
					var newCrop = new Plant
					{
						Coordinate = new Coordinate(adjacentX, adjacentY),
						Type = farm[startingY][startingX]
					};
					Visited[adjacentX, adjacentY] = true;

					MapAllCropsWithinArea(adjacentX, adjacentY, newCrop, region);
				}
			}
		}

		public static Region CreateNewPlantRegion(char type)
		{
			var identifier = 0;

			if (Regions.Any(x => x.Type == type))
			{
				identifier = Regions.Where(x => x.Type == type).Max(x => x.Identifier) + 1;
			}

			return new Region
			{
				Type = type,
				Plants = [],
				Identifier = identifier
			};
		}
	}


	public struct Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Coordinate(int x, int y) { X = x; Y = y; }
	}

	public class Plant
	{
		public Coordinate Coordinate { get; set; }
		public char Type { get; set; }
		public List<int> Perimiters = new List<int>();
	}

	public class Region
	{
		public int Identifier { get; set; }
		public char Type { get; set; }
		public int Perimiter { get; set; }
		public List<Plant> Plants { get; set; }
		public int Sides { get; set; }	
	}
}
