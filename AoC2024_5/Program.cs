using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace AoC2025_5
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var test = false;

			var path = test ? "Test" : "Real";

			var pageOrderingRulesInput = File.ReadAllLines($"{path}/por.txt");
			var printOrders = File.ReadAllLines($"{path}/printOrders.txt");

			var pageOrderingRules = new List<PageOrderingRule>();

			foreach (var por in pageOrderingRulesInput)
			{
				var splitRule = por.Split('|');

				pageOrderingRules.Add(new PageOrderingRule()
				{
					Page = int.Parse(splitRule[0]),
					AfterPage = int.Parse(splitRule[1])
				});
			}

			var resultOfAlreadySortedOrders = 0;
			var resultOfCorrectedOrders = 0;

			foreach (var order in printOrders)
			{
				Console.WriteLine("looking at...");
				Console.WriteLine(order);
				var splitOrders = Array.ConvertAll(order.Split(','), int.Parse);

				var relevantPageOrderingRules = pageOrderingRules.Where(x => splitOrders.Contains(x.Page) && splitOrders.Contains(x.AfterPage)).ToList();

				Console.WriteLine(string.Join('\n', relevantPageOrderingRules.OrderBy(x => x.Page).Select(x => $"{x.Page}|{x.AfterPage}")));
				Console.WriteLine();

				var correctlySorted = !splitOrders.Where((t, position) => relevantPageOrderingRules.Count(x => x.Page == t) != splitOrders.Length - position - 1).Any();

				if (correctlySorted)
				{
					resultOfAlreadySortedOrders += splitOrders[splitOrders.Length / 2];
				}
				else
				{
					var sorted = splitOrders.OrderBy(t => relevantPageOrderingRules.Count(x => x.AfterPage == t)).ToArray();
					resultOfCorrectedOrders += sorted[sorted.Length / 2];

				}

			}

			//p1
			Console.WriteLine(resultOfAlreadySortedOrders);

			//p2
			Console.WriteLine(resultOfCorrectedOrders);
		}
	}

	public class PageOrderingRule
	{
		public int Page { get; set; }
		public int AfterPage { get; set; }
	}
}