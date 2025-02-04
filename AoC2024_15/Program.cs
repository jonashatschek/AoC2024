using System.Diagnostics;

namespace AoC2024_15
{
    internal class Program
    {
        private const bool useTestInput = false;
        private static string[]? map;
        private static List<Entity>? part2Map = [];
        private static List<Entity>? part1Map = [];
        private static List<Direction> moves = [];
        private static int Y = 1;
        private static int X = 0;
        private static Stopwatch timer = new();
        public static List<int[]> Directions = [[1, 0], [-1, 0], [0, 1], [0, -1]]; //e, w, s, n

        static void Main(string[] args)
        {
            var path = useTestInput ? "Test" : "Real";
            map = File.ReadAllLines($"{path}/map.txt");
            part1Map = GetPart1Map(map);
            part2Map = GetPart2Map(map);
            
            var replacedMovements = File.ReadAllText($"{path}/directions.txt").Replace('>', '0').Replace('<', '1').Replace('v', '2').Replace('^', '3');
            moves = replacedMovements.Where(c => !char.IsWhiteSpace(c)).Select(c => (Direction)(c - '0')).ToList();            
            
            timer.Start();
            for (var part = 1; part <= 2; part++)
            {
                Run(part == 2);
            }

            timer.Stop();
        }


        public static int GetDistanceBetween(int[] a, int[] b)
        {
            return Math.Abs(a[X] - b[X]) + Math.Abs(a[Y] - b[Y]);
        }

        public static void MoveHorizontally(Direction direction, bool part2)
        {

            var map = part2 ? part2Map : part1Map;

            List<Entity> wallsOnRow;
            List<Entity> boxesOnRow;
            var toBeMoved = new List<Entity>();

            var robot = map.First(x => x.Type == Type.Robot);

            Entity? closestWallPosition;

            if (direction == Direction.East)
            {
                wallsOnRow = map.Where(wall => wall.Coordinates[Y] == robot.Coordinates[Y]
                                               && wall.Type == Type.Wall && wall.Coordinates[X] > robot.Coordinates[X]).ToList();

                closestWallPosition = wallsOnRow.Aggregate((minPos, nextPos) =>
                    GetDistanceBetween(robot.Coordinates, nextPos.Coordinates) < GetDistanceBetween(robot.Coordinates, minPos.Coordinates) ? nextPos : minPos);

                boxesOnRow = map.Where(box => box.Coordinates[Y] == robot.Coordinates[Y]
                                              && box.IsBox
                                              && box.Coordinates[X] > robot.Coordinates[X] && box.Coordinates[X] < closestWallPosition.Coordinates[X]).OrderBy(x => x.Coordinates[X]).ToList();

            }
            else
            {
                wallsOnRow = map.Where(wall => wall.Coordinates[Y] == robot.Coordinates[Y]
                                               && wall.Type == Type.Wall && wall.Coordinates[X] < robot.Coordinates[X]).ToList();

                closestWallPosition = wallsOnRow.Aggregate((minPos, nextPos) =>
                    GetDistanceBetween(robot.Coordinates, nextPos.Coordinates) < GetDistanceBetween(robot.Coordinates, minPos.Coordinates) ? nextPos : minPos);

                boxesOnRow = map.Where(box => box.Coordinates[Y] == robot.Coordinates[Y]
                                              && box.IsBox
                                              && box.Coordinates[X] < robot.Coordinates[X] && box.Coordinates[X] > closestWallPosition.Coordinates[X])
                    .OrderByDescending(x => x.Coordinates[X]).ToList();

            }

            var distanceToClosestWall = GetDistanceBetween(robot.Coordinates, closestWallPosition.Coordinates);

            if (distanceToClosestWall == 1)
            {
                return;
            }

            if (boxesOnRow.Count > 0)
            {

                var distanceToClosestBox = boxesOnRow.Min(pos => GetDistanceBetween(robot.Coordinates, pos.Coordinates));

                if (distanceToClosestBox > distanceToClosestWall && distanceToClosestWall > 1)
                {
                    robot.Coordinates[X] += Directions[(int)direction][X];
                    return;
                }

                if (distanceToClosestWall <= boxesOnRow.Count + 1)
                {
                    return;
                }

                if (distanceToClosestBox == 1)
                {
                    toBeMoved.Add(boxesOnRow[X]);
                    for (var i = 1; i < boxesOnRow.Count; i++)
                    {
                        if (GetDistanceBetween(boxesOnRow[i - 1].Coordinates, boxesOnRow[i].Coordinates) != 1)
                        {
                            break;
                        }

                        toBeMoved.Add(boxesOnRow[i]);

                    }

                    foreach (var box in toBeMoved)
                    {
                        box.Coordinates[X] += Directions[(int)direction][X];
                    }
                }
            }

            robot.Coordinates[X] += Directions[(int)direction][X];

        }

        public static void MoveVertically(Direction direction, bool part2)
        {

            var map = part2 ? part2Map : part1Map;

            var robot = map.First(e => e.Type == Type.Robot);
            var moveEntities = new List<Entity> { robot };

            var searchPositionsOnRow = new List<Entity>();
            var searchPositionsOnNextRow = new List<Entity>();

            searchPositionsOnRow.Add(robot);

            while (true)
            {
                if (searchPositionsOnRow.Count == 0)
                {
                    foreach (var entity in moveEntities)
                    {
                        entity.Coordinates[Y] += Directions[(int)direction][Y];
                    }

                    return;
                }

                foreach (var entity in searchPositionsOnRow)
                {

                    var rowOfNextMove = entity.Coordinates[Y] + Directions[(int)direction][Y];
                    var columnOfNextMove = entity.Coordinates[X] + Directions[(int)direction][X];

                    var firstSide = map.FirstOrDefault(x => x.Coordinates[X] == columnOfNextMove && x.Coordinates[Y] == rowOfNextMove);

                    if (firstSide == null)
                    {
                        if (GetDistanceBetween(robot.Coordinates, [columnOfNextMove, rowOfNextMove]) == 1)
                        {
                            var rowOfNextMoveRobot = robot.Coordinates[Y] + Directions[(int)direction][Y];
                            var columnOfNextMoveRobot = robot.Coordinates[X] + Directions[(int)direction][X];

                            robot.Coordinates = [columnOfNextMoveRobot, rowOfNextMoveRobot];
                            return;

                        }
                    }
                    else
                    {

                        if (firstSide.Type == Type.Wall)
                        {
                            return;
                        }

                        if (firstSide.IsBox)
                        {

                            var positionIsAlreadyAdded = moveEntities.Any(addedEntity => addedEntity.Coordinates[X] == firstSide.Coordinates[X]
                                && addedEntity.Coordinates[Y] == firstSide.Coordinates[Y]);

                            if (!positionIsAlreadyAdded)
                            {
                                searchPositionsOnNextRow.Add(firstSide);
                                moveEntities.Add(firstSide);

                                if (part2)
                                {
                                    var secondSideColumn = columnOfNextMove + (firstSide.Type == Type.RightBox ? -1 : 1);
                                    var secondSide = part2Map.First(x => x.Coordinates[X] == secondSideColumn && x.Coordinates[Y] == rowOfNextMove);

                                    searchPositionsOnNextRow.Add(secondSide);
                                    moveEntities.Add(secondSide);
                                }

                            }
                        }
                    }
                }
                searchPositionsOnRow.Clear();
                searchPositionsOnRow.AddRange(searchPositionsOnNextRow);

                searchPositionsOnNextRow.Clear();
            }
        }

        public static List<Entity> GetPart1Map(string[] map)
        {
            var toReturn = new List<Entity>();

            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] != '.')
                    {
                        toReturn.Add(new Entity(map[y][x], [x, y]));
                    }
                }
            }

            return toReturn;

        }

        public static List<Entity> GetPart2Map(string[] part1Map)
        {

            var toReturn = new List<Entity>();

            for (int y = 0; y < part1Map.Length; y++)
            {
                for (int x = 0; x < part1Map[y].Length; x++)
                {
                    var toAdd = "";
                    if (part1Map[y][x] == '#')
                    {
                        toAdd += "##";
                    }

                    if (part1Map[y][x] == 'O')
                    {
                        toAdd += "[]";
                    }

                    if (part1Map[y][x] == '@')
                    {
                        toAdd += "@.";
                    }

                    for (var i = 0; i < toAdd.Length; i++)
                    {
                        if (toAdd[i] != '.')
                        {
                            toReturn.Add(new Entity(toAdd[i], [x * 2 + i, y]));
                        }
                    }
                }
            }

            return toReturn;

        }

        public static void Run(bool part2)
        {
            foreach (var movement in moves)
            {
                if (movement == Direction.South || movement == Direction.North)
                {
                    MoveVertically(movement, part2);
                }
                else
                {
                    MoveHorizontally(movement, part2);
                }
            }

            var score = part2
                ? part2Map.Where(x => x.Type == Type.LeftBox).Sum(x => 100 * x.Coordinates[Y] + x.Coordinates[X]) :
                part1Map.Where(x => x.Type == Type.Part1Box).Sum(x => 100 * x.Coordinates[Y] + x.Coordinates[X]);
            
            Console.WriteLine($"{score}\nTime (ms): {timer.ElapsedMilliseconds}");
        }

        public static void Print(Direction direction, bool part2)
        {

            var map = part2 ? part2Map : part1Map;
            var stringDirection = direction == Direction.East ? "east" : direction == Direction.West ? "west" : direction == Direction.South ? "south" : "north";

            Console.WriteLine(stringDirection);

            for (var y = 0; y <= map.Max(y => y.Coordinates[Y]); y++)
            {
                var toAdd = "";
                for (var x = 0; x <= map.Max(x => x.Coordinates[X]); x++)
                {
                    if (map.Any(coords => coords.Coordinates[X] == x && coords.Coordinates[Y] == y))
                    {
                        toAdd += EnumToCharMap(map.First(coords =>
                            coords.Coordinates[X] == x && coords.Coordinates[Y] == y).Type);
                    }
                    else
                    {
                        toAdd += '.';
                    }

                }

                if (toAdd.Contains('@'))
                {
                    string[] toAddSplit = toAdd.Split(['\n', '@'], StringSplitOptions.RemoveEmptyEntries);
                    Console.Write(toAddSplit[0]);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("@");
                    Console.ResetColor();
                    Console.Write($"{toAddSplit[1]}\n");

                }
                else
                {
                    Console.WriteLine(toAdd);
                }
            }
        }

        public static char EnumToCharMap(Type type)
        {
            switch (type)
            {
                case Type.LeftBox:
                    return '[';
                case Type.RightBox:
                    return ']';
                case Type.Part1Box:
                    return 'O';
                case Type.Robot:
                    return '@';
                case Type.Wall:
                    return '#';
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public class Entity
    {
        public int[] Coordinates { get; set; }
        public Type Type { get; set; }
        public bool IsBox => Type == Type.LeftBox || Type == Type.RightBox || Type == Type.Part1Box;

        public Entity(char type, int[] coordinates)
        {
            Coordinates = coordinates;

            switch (type)
            {
                case '[':
                    Type = Type.LeftBox;
                    break;
                case ']':
                    Type = Type.RightBox;
                    break;
                case 'O':
                    Type = Type.Part1Box;
                    break;
                case '@':
                    Type = Type.Robot;
                    break;
                case '#':
                    Type = Type.Wall;
                    break;
            }
        }
    }

    public enum Type
    {
        Wall,
        Part1Box,
        LeftBox,
        RightBox,
        Robot
    }

    public enum Direction
    {
        East = 0,
        West = 1,
        South = 2,
        North = 3
    }
}