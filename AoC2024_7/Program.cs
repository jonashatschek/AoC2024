namespace AoC2024_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = false;
            var path = test ? "Test" : "Real";
            var input = File.ReadAllLines($"{path}/input.txt");
            var p1Result = new long();

            foreach (var line in input)
            {
                var splitLine = line.Split(':');

                var id = Int128.Parse(splitLine[0]);

                var splitNumbers = Array.ConvertAll(splitLine[1].Trim().Split(' '), long.Parse);
                
                var results = new List<long>();

                foreach (var number in splitNumbers)
                {
                    if (results.Count == 0)
                    {
                        results.Add(number);
                        continue;
                    }

                    var resultsToAdd = new List<long>();

                    foreach (var result in results)
                    {
                        var add = result + number;
                        var mul = result * number;

                        //todo fixa så man kan göra bägge parter
                        var concat = long.Parse($"{result}{number}");

                        if (add <= id)
                        {
                            resultsToAdd.Add(add);
                        }

                        if (mul <= id)
                        {
                            resultsToAdd.Add(mul);
                        }

                        if (concat <= id)
                        {
                            resultsToAdd.Add(concat);
                        }

                        results = resultsToAdd;
                    }
                }

                var match = results.FirstOrDefault(x => x == id);
                p1Result += match;
                
            }

            Console.WriteLine(p1Result);

        }

    }

}
