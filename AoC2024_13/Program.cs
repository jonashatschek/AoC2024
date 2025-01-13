using System.Text.RegularExpressions;

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
			double[] clawMachineResult;

			for (int part = 1; part <= 2; part++)
			{
				for (int j = 0; j < input.Length; j += 3)
				{
					var buttonA = Regex.Matches(input[j], findDigitsRegex);
					var buttonB = Regex.Matches(input[j + 1], findDigitsRegex);
					var prize = Regex.Matches(input[j + 2], findDigitsRegex);

					var clawMachine = new ClawMachine();

					clawMachine.ButtonA = new Coordinate(int.Parse(buttonA[0].Value), int.Parse(buttonA[1].Value));
					clawMachine.ButtonB = new Coordinate(int.Parse(buttonB[0].Value), int.Parse(buttonB[1].Value));
					clawMachine.Prize = new Coordinate(
						part == 1 ? int.Parse(prize[0].Value) : int.Parse(prize[0].Value) + 10000000000000,
						part == 1 ? int.Parse(prize[1].Value) : int.Parse(prize[1].Value) + 10000000000000);


					clawMachineResult = GetShortestDistance(clawMachine.ButtonA, clawMachine.ButtonB, clawMachine.Prize);

					var a = clawMachineResult[0];
					var b = clawMachineResult[1];

					//p1
					if (part == 1 && a % 1 == 0 && a <= 100 && b % 1 == 0 && b <= 100)
					{
						p1Result += (long)clawMachineResult[0] * 3;
						p1Result += (long)clawMachineResult[1];
					}

					//p2
					if (part == 2 && a % 1 == 0 && b % 1 == 0)
					{
						p2Result += (long)clawMachineResult[0] * 3;
						p2Result += (long)clawMachineResult[1];
					}
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
