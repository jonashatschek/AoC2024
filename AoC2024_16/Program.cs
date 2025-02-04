namespace AoC2024_16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class Reindeer
    {
        public int[] Position { get; set; }
        public Direction Direction { get; set; }

    }

    public enum Direction
    {
        East,
        West,
        South,
        North
    }
}
