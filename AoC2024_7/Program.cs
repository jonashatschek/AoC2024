using System.Data;
using System.Diagnostics.SymbolStore;
using System.IO.Pipes;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Runtime.InteropServices.JavaScript;

namespace AoC2024_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = true;
            var path = test ? "Test" : "Real";
            var input = File.ReadAllLines($"{path}/input.txt");
            var result = new Int128();
            foreach (var line in input)
            {

                var splitLine = line.Split(':');

                var id = Int128.Parse(splitLine[0]);
                var answer = 0;

                var splitNumbers = Array.ConvertAll(splitLine[1].Trim().Split(' '), int.Parse);

                char[] operators = ['+', '*'];


                var numCombinations = (int)Math.Pow(operators.Length, splitNumbers.Length - 1);


                for (int i = 0; i < numCombinations; i++)
                {
                    string expression = splitNumbers[0].ToString();
                    var queue = new Queue<string>();
                    queue.Enqueue(splitNumbers[0].ToString());
                    int temp = i;

                    if (id == 292)
                    {
                        Console.Write("");
                    }

                    for (int j = 0; j < splitNumbers.Length - 1; j++)
                    {
                        char op = operators[temp % operators.Length];
                        temp /= operators.Length;

                        expression += $"{op}{splitNumbers[j + 1]}";

                        var res = Evaluate(expression, splitNumbers);

                        if (id == res)
                        {
                            result += id;
                        }
                    }
                }


                //todo organize
                //var calculations = new List<Calculation>();

                //for (int i = 0; i < splitNumbers.Length - 1; i++)
                //{
                //    calculations.Add(new Calculation(splitNumbers[i], splitNumbers[i + 1]));
                //}

                //antalet kombinationer...
                //for (int i = 0; i < numCombinations; i++)
                //{
                //    int temp = i;

                //för varje calculation finns två kombinationer
                //for (int j = 0; j < calculations.Count; j++)
                {
                    //bool op = operators[temp % operators.Length] == 0;
                    //temp /= operators.Length;

                    //vahvha += op ? calculations[j].AdditionResult : calculations[j].MultiplicationResult;

                    //if (op)
                    //{
                    //    vahvha += calculations[j].AdditionResult
                    //}

                    ////expression += $"{op}{splitNumbers[j + 1]}";

                    ////var res = Evaluate(expression, splitNumbers);

                    //if (id == vahvha)
                    //{
                    //    result += id;
                    //}

                    //var calculations[0]
                    //}

                    //}

                }

                Console.WriteLine(result);
            }


            //public static Int128[] Evaluate(Int128 a, Int128 b, char[] ops)
            //{


            //    var res = new Int128[ops.Length];

            //    for (int i = 0; i < ops.Length; i++)
            //    {
            //        var op = ops[i];

            //        DataTable table = new DataTable();
            //        table.Columns.Add("expression", typeof(string), $"{a}{op}{b}");
            //        DataRow row = table.NewRow();
            //        table.Rows.Add(row);

            //        res[i] = Int128.Parse((string)row["expression"]);

            //    }


            //    return res;


            //    //return Int128.Parse((string)row["expression"]);
            //}

            public static Int128 Evaluate(string expression, int[] splitNumbers)
            {

                if (expression == "11 + 6 * 16 + 20")
                {
                    Console.Write("ghsa");
                }

                //var splitWithOperator;
                //for (int i = 0; i < splitNumbers.Length; i++)s
                //{
                //    var indexOfFirstNumber = expression.IndexOf(splitNumbers[i]);

                //    if()
                //}




                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return Int128.Parse((string)row["expression"]);
            }

        }


    }

    public class Calculation(Int128 a, Int128 b)
    {
        public Int128 A { get; set; } = a;
        public Int128 B { get; set; } = b;
        public Int128 MultiplicationResult => A * B;
        public Int128 AdditionResult => A + B;
    }

}
