namespace AoC2024_1
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var historianA = new List<int>();
			var historianB = new List<int>();
			
			var input = File.ReadLines("input.txt");

			foreach (var row in input)
			{
				var a = row.Substring(0, 5);
				var b = row.Substring(5).TrimStart();

				historianA.Add(int.Parse(a));
				historianB.Add(int.Parse(b));

			}

			historianA.Sort();
			historianB.Sort();

			var sumDif = 0;

			for (int i = 0; i < historianA.Count; i++)
			{
				var difference = historianA[i] - historianB[i];

				sumDif += Math.Abs(difference);
			}

			var similarityScores = new List<int>();

			foreach(var number in historianA)
			{
				var countOccurrences = historianB.Where(x => x == number).Count();
				similarityScores.Add(number * countOccurrences);

			}

			//p1
			Console.WriteLine(sumDif);

			//p2
			Console.WriteLine(similarityScores.Sum());
		}
	}
}