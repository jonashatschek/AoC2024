using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AoC2024_13
{
	internal class Program
	{
		public static bool useTestData = false;
		public static string path = useTestData ? "Test" : "Real";
		public static string[] input = File.ReadAllLines($"{path}/input.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


		static void Main(string[] args)
		{
			long p1Result = 0;
			long p2Result = 0;

			var findDigitsRegex = @"\d+";
			var clawMachines = new List<ClawMachine>();
			for (int i = 0; i < input.Length - 2; i+=3)
			{
				var clawMachine = new ClawMachine();

				for (int j = i; j < i + 3; j++)
				{
					var digits = Regex.Matches(input[j], findDigitsRegex);
					var x = int.Parse(digits[0].Value);
					var y = int.Parse(digits[1].Value);
					var test = input[j]; 

					if (input[j].StartsWith("Button B"))
					{
						clawMachine.ButtonB = new Coordinate(x, y);
					}
					else if (input[j].StartsWith("Prize"))
					{
						clawMachine.Prize = new Coordinate(x + 10000000000000, y + 10000000000000);
					}
					else
					{
						clawMachine.ButtonA = new Coordinate(x, y);
					}
				}

				var res = GetShortestDistance(clawMachine.ButtonA, clawMachine.ButtonB, clawMachine.Prize);
				var a = res[0];
				var b = res[1];

				//p1
				if (a % 1 == 0 && a <= 100 && b % 1 == 0 && b <= 100)
				{
					p1Result += (long) res[0] * 3;
					p1Result += (long) res[1];
				}
				
				//p2
				if (a % 1 == 0 && b % 1 == 0)
				{
					p2Result += (long)res[0] * 3;
					p2Result += (long)res[1];
				}
			}


			Console.WriteLine(p1Result);

			Console.WriteLine(p2Result);

		}


		public static double[] GetShortestDistance(Coordinate buttonA, Coordinate buttonB, Coordinate prize)
		{
			double b = ((buttonA.Y * prize.X) - (buttonA.X * prize.Y)) / ((buttonB.X * buttonA.Y) - (buttonA.X * buttonB.Y));
			double a = (prize.X - (buttonB.X * b)) / buttonA.X;

			return [a, b];
		}
	}
	


	public class ClawMachine()
	{
		public Coordinate ButtonA { get; set; }
		public Coordinate ButtonB { get; set; }
		public Coordinate Prize { get; set; }
	}


	public struct Coordinate
	{
		public double X { get; set; }
		public double Y { get; set; }
		public Coordinate(double x, double y) { X = x; Y = y; }
	}
}
