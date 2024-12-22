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
		public static List<int[]> Directions = [[1, 0], [-1, 0], [0, 1], [0, -1]]; //e, w, n, s
		public static int[] PerimiterTypes = [0, 1, 2, 3]; //e, w, n, s

		static void Main(string[] args)
		{
			for (var y = 0; y < farm.Length; y++)
			{
				var row = farm[y];
				for (var x = 0; x < row.Length; x++)
				{
					var cropType = farm[y][x];
					var crop = new Plant
					{
						Coordinates = new Tuple<int, int>(x, y),
						Type = cropType
					};

					var isAlreadyAddedToCropArea = Regions.Any(cropArea => cropArea.Plants.Any(crop => crop.Coordinates.Item1 == x && crop.Coordinates.Item2 == y));

					if (!isAlreadyAddedToCropArea)
					{
						var cropArea = CreateNewCropArea(farm[y][x]);
						Regions.Add(cropArea);
						MapAllCropsWithinArea(x, y, crop, cropArea);
					}
				}
			}

			foreach (var region in Regions)
			{

				foreach (var perimiterType in PerimiterTypes)
				{
					var plantsInRegionWithSameDirection = region.Plants.Where(plants => plants.Perimiters.Any(plantPerimiter => plantPerimiter == perimiterType));

					var splitBySides = new List<IEnumerable<IEnumerable<int>>>();
					int solitarySidePerimiter = 0;

					if (perimiterType == 0 || perimiterType == 1)
					{
						var perimitersOnSameRow = plantsInRegionWithSameDirection.OrderBy(plants => plants.Coordinates.Item2)
							.GroupBy(plants => plants.Coordinates.Item1).Where(plants => plants.Count() > 1)
							.Select(x => x.Select(x => x.Coordinates)).ToList();

						splitBySides = perimitersOnSameRow.Select(x => x.Select(x => x.Item2)
								.Select((value, index) => new { value, groupKey = value - index })
								.GroupBy(x => x.groupKey)
								.Select(group => group.Select(x => x.value))).ToList();

						solitarySidePerimiter = plantsInRegionWithSameDirection.GroupBy(plants => plants.Coordinates.Item1)
							.Where(plants => plants.Count() == 1).Count();

					}

					if (perimiterType == 2 || perimiterType == 3)
					{
						var perimitersOnSameRow = plantsInRegionWithSameDirection.OrderBy(plants => plants.Coordinates.Item1)
							.GroupBy(plants => plants.Coordinates.Item2).Where(plants => plants.Count() > 1).Select(x => x.Select(x => x.Coordinates)).ToList();

						splitBySides = perimitersOnSameRow.Select(x => x.Select(x => x.Item1)
						.Select((value, index) => new { value, groupKey = value - index })
						.GroupBy(x => x.groupKey)
						.Select(group => group.Select(x => x.value))).ToList();

						solitarySidePerimiter = plantsInRegionWithSameDirection.GroupBy(plants => plants.Coordinates.Item2)
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

			var p1Result = 0;
			var p2Result = 0;
			foreach (var region in Regions)
			{
				p1Result += region.Plants.Count * region.Perimiter;
				p2Result += region.Sides * region.Plants.Count;
			}

			Console.WriteLine($"part 1: {p1Result}");
			Console.WriteLine($"part 2: {p2Result}");

		}

		public static void MapAllCropsWithinArea(int startingX, int startingY, Plant plant, Region region)
		{
			region.Plants.Add(plant);


			for (int direction = 0; direction < 4; direction++)
			{
				var dx = Directions[direction][0];
				var dy = Directions[direction][1];

				var adjacentX = dx + startingX;
				var adjacentY = dy + startingY;

				var isOutsideMapEdges = adjacentY < 0 || adjacentY >= farm.Length || adjacentX < 0 || adjacentX >= farm[startingY].Length;
				var isNeighbourSameAsCurrentPosition = false;
				var isAlreadyAddedToCropArea = Regions.Any(x => x.Plants.Any(x => x.Coordinates.Item1 == adjacentX && x.Coordinates.Item2 == adjacentY));

				if (!isOutsideMapEdges)
				{
					isNeighbourSameAsCurrentPosition = farm[startingY][startingX] == farm[adjacentY][adjacentX];
				}

				if (isOutsideMapEdges || !isNeighbourSameAsCurrentPosition)
				{
					region.Perimiter++;
					plant.Perimiters.Add(direction);
				}

				if (!isOutsideMapEdges && isNeighbourSameAsCurrentPosition && !isAlreadyAddedToCropArea)
				{
					var newCrop = new Plant
					{
						Coordinates = new Tuple<int, int>(adjacentX, adjacentY),
						Type = farm[startingY][startingX]
					};

					MapAllCropsWithinArea(adjacentX, adjacentY, newCrop, region);
				}

			}

		}

		public static Region CreateNewCropArea(char type)
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

	public class Plant
	{
		public Tuple<int, int> Coordinates { get; set; }
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
