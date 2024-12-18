using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC2024_3
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var input = File.ReadAllText("Real/input.txt");

			//p1
			var sum = GetSum(input);

			var splitDoNot = input.Split("don't()");

			//p2
			var p2Sum = 0;

			for(int j = 0; j < splitDoNot.Length; j++)
			{
				if(j == 0)
				{
					p2Sum += GetSum(splitDoNot[j]);
				}
				else
				{
					var splitDo = splitDoNot[j].Split("do()");

					for (int i = 1; i < splitDo.Length; i++)
					{
						p2Sum += GetSum(splitDo[i]);
					}
				}
			}


			Console.WriteLine(sum);
			Console.WriteLine(p2Sum);
		}

		public static int GetSum(string input)
		{
			var matches = Regex.Matches(input, "mul\\(\\d+,\\d+\\)");

			var sum = 0;
			foreach (var match in matches)
			{

				var matchString = match.ToString();
				var numbers = matchString.Substring(4, matchString.Length - 5);
				var splitNumbers = numbers.Split(',');

				var res = int.Parse(splitNumbers[0]) * int.Parse(splitNumbers[1]);

				sum += res;
			}

			return sum;
		}
	}
}