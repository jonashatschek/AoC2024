namespace AoC2024_10
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var useTestInput = true;
			var path = useTestInput ? "Real" : "Test";
			var input = File.ReadAllLines($"{useTestInput}/input.txt");

			for (int y = 0; y < input.Length; y++)
			{
				var row = input[y];
				for (int x = 0; x < row.Length; x++)
				{

				}
			}

		}
	}
}
