using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC2024_14
{
    internal class Program
    {
        private const bool useTestInput = false;
        private static Dictionary<long[], long[]> robots = new();
        private const int width = useTestInput ? 11 : 101;
        private const int height = useTestInput ? 7 : 103;

        static void Main(string[] args)
        {
            var path = useTestInput ? "Test" : "Real";
            var input = File.ReadAllLines($"{path}/input.txt");
            var findDigitsRegex = @"-?\d+";

            var p1Timer = new Stopwatch();
            p1Timer.Start();

            var p2Timer = new Stopwatch();
            p2Timer.Start();
            
            foreach (var line in input)
            {
                var digits = Regex.Matches(line, findDigitsRegex);
                for (int i = 0; i < 1; i++)
                {
                    var posX = long.Parse(digits[i].Value);
                    var posY = long.Parse(digits[i + 1].Value);
                    long[] position = [posX, posY];

                    var velX = long.Parse(digits[i + 2].Value);
                    var velY = long.Parse(digits[i + 3].Value);
                    long[] velocities = [velX, velY];

                    robots.Add(velocities, position);
                }
            }

            var foundChristmasTree = false;
            double p1Answer = 0;
            for (int seconds = 1; !foundChristmasTree; seconds++)
            {

                foreach (var robot in robots)
                {
                    robots[robot.Key] = GetNewPosition(robot.Value, robot.Key);
                }

                //p1
                if (seconds == 100)
                {
                    var middleWidth = width / 2;
                    var middleHeigth = height / 2;

                    var topLeft = robots.Count(p => p.Value[0] < middleWidth && p.Value[1] < middleHeigth);
                    var topRight = robots.Count(p => p.Value[0] > middleWidth && p.Value[1] < middleHeigth);
                    var bottomLeft = robots.Count(p => p.Value[0] < middleWidth && p.Value[1] > middleHeigth);
                    var bottomRight = robots.Count(p => p.Value[0] > middleWidth && p.Value[1] > middleHeigth);

                    p1Answer = topLeft * topRight * bottomLeft * bottomRight;

                    p1Timer.Stop();
                }

                //p2
                var robotPositions = robots.Select(x => x.Value).ToArray();
                foreach (var robotPositionRows in robotPositions.OrderBy(y => y[0]).GroupBy(y => y[1]).Where(y => y.Count() > 6).Select(x => x).ToList())
                {
                    var possibles = robotPositionRows.Where(y => y[1] == robotPositionRows.Key).ToList();

                    foreach (var possible in possibles)
                    {
                        foundChristmasTree = IsPartOfXmasTree(possible, possibles);

                        if (foundChristmasTree)
                        {
                            break;
                        }
                    }
                }

                if (foundChristmasTree)
                {
                    DrawCurrentPositions();
                    p2Timer.Stop();
                    Console.WriteLine($"\nPart 1: {p1Answer}\nTime p1: {p1Timer.ElapsedMilliseconds} ms\n");
                    Console.WriteLine($"Part 2: {seconds} seconds\nTime p2: {p2Timer.ElapsedMilliseconds} ms");
                    break;
                }
            }
        }

        public static void DrawCurrentPositions()
        {
            var robotPositions = robots.Select(x => x.Value).ToList();

            for (int y = 0; y <= height; y++)
            {
                string toAdd = "";
                for (int x = 0; x <= width; x++)
                {

                    if (robotPositions.Any(p => p[0] == x && p[1] == y))
                    {
                        toAdd += "X";
                    }
                    else
                    {
                        toAdd += ".";
                    }
                }

                Console.WriteLine(toAdd);
            }
        }

        public static bool IsPartOfXmasTree(long[] coordinates, List<long[]> possibles)
        {
            for (long dx = 0; dx < 8; dx++)
            {
                if (!possibles.Any(robot => robot[0] == coordinates[0] + dx && robot[1] == coordinates[1]))
                {
                    return false;
                }
            }
            return true;
        }

        public static long[] GetNewPosition(long[] currentPosition, long[] velocities)
        {
            var newX = (currentPosition[0] + velocities[0]) % width;
            var newY = (currentPosition[1] + velocities[1]) % height;

            if (newX < 0)
            {
                newX += width;
            }

            if (newY < 0)
            {
                newY += height;
            }

            return [newX, newY];
        }
    }
}