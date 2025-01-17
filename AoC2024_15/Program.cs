using System;
using System.Diagnostics;
using System.Text;

namespace AoC2024_15
{
	internal class Program
	{
		private const bool useTestInput = true;
		private static string[]? map;
		public static readonly List<int[]> Directions = [[0, 1], [1, 0], [0, -1], [-1, 0] ]; // n, e, s, w

		static void Main(string[] args)
		{

			var path = useTestInput ? "Test" : "Real";
			map = File.ReadAllLines($"{path}/map.txt");
			var replacedDirections = File.ReadAllText($"{path}/directions.txt").Replace('v', '0').Replace('>', '1').Replace('^', '2').Replace('<', '3');
            int[] robot = [];
			var intDirections = new List<int>();
            var timer = new Stopwatch();
            timer.Start();

			//p2
			//If the tile is #, the new map contains ## instead.
			// If the tile is O, the new map contains [] instead.
			// If the tile is ., the new map contains .. instead.
			// If the tile is @, the new map contains @. instead.

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
					robot = [map[y].IndexOf('@'), y];
				}
			}

			foreach (var direction in intDirections)
			{

				var closestDot = GetPositionOfClosestCharacter('.', direction, robot);

				if (closestDot != null)
				{
					var distanceToDot = GetDistanceBetween(robot, closestDot);

					if (distanceToDot == 1)
					{
						Move('@', robot, closestDot);
						robot = closestDot;
					}
					else
					{
						var closestWall = GetPositionOfClosestCharacter('#', direction, robot);

						if (closestWall != null)
						{
							var distanceToWall = GetDistanceBetween(robot, closestWall);

							if (distanceToDot < distanceToWall)
							{


								var numberOfBoxes = distanceToDot - 1;

								for (var boxesLeft = numberOfBoxes; boxesLeft >= 1; boxesLeft--)
								{
									int[] moveBoxFrom;

									switch (direction)
									{
										case 0:
                                            moveBoxFrom = [robot[0], robot[1] + boxesLeft];
                                            break;
										case 1:
                                            moveBoxFrom = [robot[0] + boxesLeft, robot[1]];
                                            break;
										case 2:
                                            moveBoxFrom = [robot[0], robot[1] - boxesLeft];
                                            break;
										default:
											moveBoxFrom = [robot[0] - boxesLeft, robot[1]];
											break;
									}

                                    int[] moveBoxTo = [moveBoxFrom[0] + Directions[direction][0], moveBoxFrom[1] + Directions[direction][1]];
                                    
									Move('O', moveBoxFrom, moveBoxTo);
								}

								closestDot = GetPositionOfClosestCharacter('.', direction, robot);
								Move('@', robot, closestDot);
								robot = closestDot;

							}
						}
					}
				}

				//var stringDirection = direction == 0 ? "east" : direction == 1 ? "west" : direction == 2 ? "north" : "south";

            }

            timer.Stop();
            
            double p1Score = 0;
			for (int y = 0; y < map.Length; y++)
			{
                for (int x = 0; x < map[y].Length; x++)
				{
					if (map[y][x] == 'O')
					{
                        p1Score += 100 * y + x;
                    }
				}
				Console.WriteLine(map[y]);
			}



			Console.WriteLine($"Timer: {timer.ElapsedMilliseconds} ms, part 1:{p1Score}");
			
			Console.ReadKey();

		}

		public static int GetDistanceBetween(int[] a, int[] b)
		{
			return Math.Abs(a[0] - b[0]) + Math.Abs(a[1] - b[1]);
		}

		public static void Move(char character, int[] from, int[] to)
		{
			var toRow = map[to[1]].ToCharArray();
			toRow[to[0]] = character;
			map[to[1]] = new string(toRow);

			var fromRow = map[from[1]].ToCharArray();
			fromRow[from[0]] = '.';
			map[from[1]] = new string(fromRow);
		}


		public static int[]? GetPositionOfClosestCharacter(char character, int direction, int[] position)
		{

			if (direction == 0)
			{
                //n
                for (int y = position[1]; y <= map.Length - 1; y++)
                {
                    char? pivot = map[y][position[0]];

                    if (pivot == character)
                    {
                        return [position[0], y];
                    }
                }

            }
			else if (direction == 1)
			{

				//e
                var currentRow = map[position[1]];
                var indexOfClosest = currentRow.Substring(position[0]).IndexOf(character) + position[0];

                if (indexOfClosest != -1 && indexOfClosest > position[0])
                {
                    return [indexOfClosest, position[1]];
                }
                
			}
			else if (direction == 2)
			{
				//s
                for (int y = position[1]; y >= 0; y--)
                {
                    char? pivot = map[y][position[0]];
                    if (pivot == character)
                    {
                        return [position[0], y];
                    }
                }
                
            }
			else
			{
				//w
                var currentRow = map[position[1]];
                var indexOfClosest = currentRow.Substring(0, position[0]).LastIndexOf(character);

                if (indexOfClosest != -1)
                {
                    return [indexOfClosest, position[1]];
                }
			}

			return null;
		}

	}

	public enum Type
	{
		wall,
		box,
		empty
	}
}
