using System;
using System.Text;

namespace AoC2024_15
{
	internal class Program
	{
		private const bool useTestInput = false;
		private static string[]? map;
		public static readonly int[,] Directions = { { 1, 0 }, { -1, 0 }, { 0, -1 }, { 0, 1 } }; // e, w, s, n

		static void Main(string[] args)
		{
			//var mapped = new List<MapEntity>();

			//move in direction

			//find direction
			//find nearest '.' in direction
			//find nearest wall (#) in that direction. If there's a '.' between here and the wall, we can move.
			//also need to move boxes (O)

			var path = useTestInput ? "Test" : "Real";
			map = File.ReadAllLines($"{path}/map.txt");
			var replacedDirections = File.ReadAllText($"{path}/directions.txt").Replace('>', '0').Replace('<', '1').Replace('^', '2').Replace('v', '3');
			MapEntity? robot = null;
			var intDirections = new List<int>();
			foreach (char i in replacedDirections)
			{
				if (i != '\n' && i != '\r')
				{
					intDirections.Add(i - '0');
				}
			}

			for (int y = 0; y < map.Length; y++)
			{
				if (map[y].IndexOf('@') > 0)
				{
					robot = new MapEntity(new Coordinate(map[y].IndexOf('@'), y), Type.robot);
				}
			}

			foreach (var direction in intDirections)
			{

				// 0 = e, 1 = w, 2 = n, 3 = s

				var closestDot = GetPositionOfClosestCharacter('.', direction, robot.Position);

				if (closestDot.HasValue)
				{
					var distanceToDot = GetDistanceBetween(robot.Position, closestDot.Value);

					if (distanceToDot == 1)
					{
						Move('@', robot.Position, closestDot.Value);
						robot.Position = closestDot.Value;
					}
					else
					{
						var closestWall = GetPositionOfClosestCharacter('#', direction, robot.Position);

						if (closestWall.HasValue)
						{
							var distanceToWall = GetDistanceBetween(robot.Position, closestWall.Value);

							if (distanceToDot < distanceToWall)
							{

								//If there are O's in direction between here and the '.' they move
								//in this case there are definitely O's
								//Move('O', new Coordinate(robot.))

								var numberOfBoxes = distanceToDot - 1;

								for (var boxesLeft = numberOfBoxes; boxesLeft >= 1; boxesLeft--)
								{
									Coordinate moveBoxFrom;

									switch (direction)
									{
										case 0:
											moveBoxFrom = new Coordinate(robot.Position.X + boxesLeft, robot.Position.Y);
											break;
										case 1:
											moveBoxFrom = new Coordinate(robot.Position.X - boxesLeft, robot.Position.Y);
											break;
										case 2:
											moveBoxFrom = new Coordinate(robot.Position.X, robot.Position.Y - boxesLeft);
											break;
										default:
											moveBoxFrom = new Coordinate(robot.Position.X, robot.Position.Y + boxesLeft);
											break;
									}

									var moveBoxTo = new Coordinate(moveBoxFrom.X + Directions[direction, 0], moveBoxFrom.Y + Directions[direction, 1]);

									Move('O', moveBoxFrom, moveBoxTo);
								}

								closestDot = GetPositionOfClosestCharacter('.', direction, robot.Position);
								Move('@', robot.Position, closestDot.Value);
								robot.Position = closestDot.Value;

							}
						}
					}
				}

				var stringDirection = direction == 0 ? "east" : direction == 1 ? "west" : direction == 2 ? "north" : "south";

				// 0 = e, 1 = w, 2 = n, 3 = s



			}

			double score = 0;
			for (int y = 0; y < map.Length; y++)
			{
				string toAdd = "";

				for (int x = 0; x < map[y].Length; x++)
				{
					if (map[y][x] == 'O')
					{
						score += CountScore(new Coordinate(x, y), false);
					}
					toAdd += map[y][x];
				}

				Console.WriteLine(toAdd);

			}

			Console.WriteLine($"Score: {score}");

			Console.WriteLine();

			Console.ReadKey();

		}

		public static int CountScore(Coordinate boxCoordinate, bool part2)
		{
			var distanceToTopWall = GetPositionOfClosestCharacter('#', 2, boxCoordinate).Value.X;
			var distanceToLeftWall = GetPositionOfClosestCharacter('#', 1, boxCoordinate).Value.Y;

			if (part2)
			{
				distanceToLeftWall++;
			}

			return 100 * distanceToLeftWall + distanceToTopWall;
		}

		public static int GetDistanceBetween(Coordinate a, Coordinate b)
		{
			return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
		}

		public static void Move(char character, Coordinate from, Coordinate to)
		{
			var toRow = map[to.Y].ToCharArray();
			toRow[to.X] = character;
			map[to.Y] = new string(toRow);

			var fromRow = map[from.Y].ToCharArray();
			fromRow[from.X] = '.';
			map[from.Y] = new string(fromRow);
		}

		public static Coordinate? GetPositionOfClosestCharacter(char character, int direction, Coordinate position)
		{
			if (direction == 0)
			{
				var currentRow = map[position.Y];
				var indexOfClosest = currentRow.Substring(position.X).IndexOf(character) + position.X;

				if (indexOfClosest != -1 && indexOfClosest > position.X)
				{
					return new Coordinate(indexOfClosest, position.Y);
				}

			}
			else if (direction == 1)
			{
				var currentRow = map[position.Y];
				var indexOfClosest = currentRow.Substring(0, position.X).LastIndexOf(character);

				if (indexOfClosest != -1)
				{
					return new Coordinate(indexOfClosest, position.Y);
				}
			}
			else if (direction == 2)
			{

				for (int y = position.Y; y >= 0; y--)
				{

					char? pivot = map[y][position.X];
					if (pivot == character)
					{
						var found = new Coordinate(position.X, y);
						y = -1;
						return found;
					}
				}
			}
			else
			{
				for (int y = position.Y; y <= map.Length - 1; y++)
				{
					char? pivot = map[y][position.X];

					if (pivot == character)
					{
						var found = new Coordinate(position.X, y);
						y = map.Length;
						return found;
					}
				}
			}

			return null;
		}

	}

	public class MapEntity
	{
		public Coordinate Position { get; set; }
		public Type Type { get; set; }

		public MapEntity(Coordinate startPosition, Type type)
		{
			Position = startPosition;
			Type = type;
		}

	}

	public struct Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }
		public Coordinate(int x, int y) { X = x; Y = y; }
	}

	public enum Type
	{
		wall,
		box,
		robot
	}
}
