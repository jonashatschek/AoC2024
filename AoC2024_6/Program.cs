using System.ComponentModel;
using System.Diagnostics;

namespace AoC2024_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = true;

            var path = test ? "Test" : "Real";
            var map = File.ReadAllLines($"{path}/input.txt").Reverse().ToArray();

            var guardGetsStuckCounter = 0;

            var startPosition = FindStartingPosition(map);

            var timer = new Stopwatch();
            timer.Start();

            for (int cols = 0; cols < map.Length; cols++)
            {
                var row = map[cols];

                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i] == '.')
                    {
                        if (GetsGuardStuck(map, startPosition, new Tuple<int, int>(i, cols)))
                        {
                            guardGetsStuckCounter++;
                        }
                    }
                }
            }

            //p1
            //Console.WriteLine(guard.Visited.Count);
            Console.WriteLine($"Part1 time: {timer.ElapsedMilliseconds}");

            //p2
            Console.WriteLine(guardGetsStuckCounter);
            Console.WriteLine($"Part2 time: {timer.ElapsedMilliseconds}");
        }

        public static bool GetsGuardStuck(string[] map, Tuple<int, int> startPosition, Tuple<int, int> editedPosition)
        {
            var isOutsideMap = false;

            var guard = new Guard
            {
                CurrentDirection = Direction.north,
                CurrentPosition = startPosition,
                Visited = new List<Tuple<int, int>>(),

            };

            guard.Visited.Add(startPosition);

            while (!isOutsideMap)
            {

                var currentX = guard.CurrentPosition.Item1;
                var currentY = guard.CurrentPosition.Item2;
                var destination = new Tuple<int, int>(0, 0);

                if (guard.CurrentDirection == Direction.north)
                {
                    for (var i = currentY + 1; i <= map.Length - 1; i++)
                    {
                        if (map[i][currentX] == '#' || (currentX == editedPosition.Item1 && i == editedPosition.Item2))
                        {
                            break;
                        }

                        currentY = i;
                        guard.Visited.Add(new Tuple<int, int>(currentX, currentY));

                    }

                    destination = new Tuple<int, int>(currentX, currentY + 1);

                    guard.CurrentPosition = new Tuple<int, int>(currentX, currentY);
                }
                else if (guard.CurrentDirection == Direction.east)
                {

                    for (var i = currentX + 1; i <= map[currentY].Length - 1; i++)
                    {
                        if (map[currentY][i] == '#' || (i == editedPosition.Item1 && currentY == editedPosition.Item2))
                        {
                            break;
                        }

                        currentX = i;
                        guard.Visited.Add(new Tuple<int, int>(currentX, currentY));
                    }

                    destination = new Tuple<int, int>(currentX + 1, currentY);
                    guard.CurrentPosition = new Tuple<int, int>(currentX, currentY);
                }
                else if (guard.CurrentDirection == Direction.south)
                {

                    for (var i = currentY - 1; i >= 0; i--)
                    {
                        if (map[i][currentX] == '#' || (currentX == editedPosition.Item1 && i == editedPosition.Item2))
                        {
                            break;
                        }

                        currentY = i;
                        guard.Visited.Add(new Tuple<int, int>(currentX, currentY));
                    }

                    destination = new Tuple<int, int>(currentX, currentY - 1);

                    guard.CurrentPosition = new Tuple<int, int>(currentX, currentY);
                }
                else if (guard.CurrentDirection == Direction.west)
                {

                    for (var i = currentX - 1; i >= 0; i--)
                    {
                        if (map[currentY][i] == '#' || (i == editedPosition.Item1 && currentY == editedPosition.Item2))
                        {
                            break;
                        }

                        currentX = i;
                        guard.Visited.Add(new Tuple<int, int>(currentX, currentY));
                    }

                    destination = new Tuple<int, int>(currentX - 1, currentY);
                    guard.CurrentPosition = new Tuple<int, int>(currentX, currentY);

                }

                isOutsideMap = destination.Item1 + 1 > map[0].Length || destination.Item1 < 0 || destination.Item2 + 1 > map.Length || destination.Item2 < 0;

                if (isOutsideMap)
                {
                    break;
                }

                var nextStepHasObstacle = map[destination.Item2][destination.Item1] == '#' || (destination.Item1 == editedPosition.Item1 && destination.Item2 == editedPosition.Item2);

                if (nextStepHasObstacle)
                {
                    guard.CurrentDirection = ChangeDirection(guard.CurrentDirection);
                }

                if (guard.Visited.GroupBy(tuple => tuple).Any(group => group.Count() > 3))
                {
                    return true;
                }

            }

            return false;
        }


        public static Tuple<int, int> FindStartingPosition(string[] map)
        {
            int x = 0;
            int y;

            for (y = 0; y < map.Length; y++)
            {
                if (map[y].Contains('^'))
                {
                    x = map[y].IndexOf('^');
                    break;
                }
            }

            return new Tuple<int, int>(x, y);
        }

        public static Direction ChangeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.north:
                    return Direction.east;
                case Direction.east:
                    return Direction.south;
                case Direction.south:
                    return Direction.west;
                default:
                    return Direction.north;
            }

        }
    }
 
	public class Guard
	{
		public Direction CurrentDirection { get; set; }
		public Tuple<int, int>? CurrentPosition { get; set; }
		public List<Tuple<int, int>>? Visited { get; set; }
	}

	public enum Direction
	{
		north,
		south,
		west,
		east
	}
}