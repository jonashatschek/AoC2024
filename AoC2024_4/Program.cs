namespace AoC2024_4
{
	internal class Program
	{

		static void Main(string[] args)
		{
			var test = false;

			var path = test ? "Test/input.txt" : "Real/input.txt";
			var input = File.ReadAllLines(path);

			var xmasCounter = 0;
			var masCounter = 0;


			for (var y = 0; y < input.Length; y++)
			{
				var row = input[y];

				for (var x = 0; x < row.Length; x++)
				{
					var curPos = row[x];

					if (curPos == 'X')
					{
						//var searchString = "MAS";
						//todo created method insted of repeating same code 8 times :)

						var lookWest = x >= 3;
						var lookEast = x < row.Length - 3;
						var lookSouth = y >= 3;
						var lookNorth = y < input.Length - 3;

						if (lookWest)
						{
							if (input[y][x - 1] == 'M')
							{

								if (input[y][x - 2] == 'A')
								{

									if (input[y][x - 3] == 'S')
									{
										xmasCounter++;

									}

								}

							}

						}

						if (lookNorth)
						{
							if (input[y + 1][x] == 'M')
							{

								if (input[y + 2][x] == 'A')
								{

									if (input[y + 3][x] == 'S')
									{
										xmasCounter++;

									}

								}

							}
						}

						if (lookEast)
						{
							if (input[y][x + 1] == 'M')
							{

								if (input[y][x + 2] == 'A')
								{

									if (input[y][x + 3] == 'S')
									{
										xmasCounter++;

									}

								}

							}
						}

						if (lookSouth)
						{
							if (input[y - 1][x] == 'M')
							{

								if (input[y - 2][x] == 'A')
								{

									if (input[y - 3][x] == 'S')
									{
										xmasCounter++;

									}

								}

							}
						}

						if (lookNorth && lookEast)
						{
							if (input[y + 1][x + 1] == 'M')
							{

								if (input[y + 2][x + 2] == 'A')
								{

									if (input[y + 3][x + 3] == 'S')
									{
										xmasCounter++;

									}

								}
							}
						}


						if (lookNorth && lookWest)
						{
							if (input[y + 1][x - 1] == 'M')
							{

								if (input[y + 2][x - 2] == 'A')
								{

									if (input[y + 3][x - 3] == 'S')
									{
										xmasCounter++;

									}

								}
							}
						}

						if (lookSouth && lookWest)
						{
							if (input[y - 1][x - 1] == 'M')
							{

								if (input[y - 2][x - 2] == 'A')
								{

									if (input[y - 3][x - 3] == 'S')
									{
										xmasCounter++;
									}

								}
							}
						}


						if (lookSouth && lookEast)
						{
							if (input[y - 1][x + 1] == 'M')
							{

								if (input[y - 2][x + 2] == 'A')
								{

									if (input[y - 3][x + 3] == 'S')
									{
										xmasCounter++;

									}

								}
							}
						}

					}

				}

				for (var x = 0; x < row.Length; x++)
				{
					var curPos = row[x];

					if (curPos == 'A')
					{
						if (x >= 1 && x < row.Length - 1 && y >= 1 && y < input.Length - 1)
						{

							var str = "";

							var northWest = input[y + 1][x - 1];
							var northEast = input[y + 1][x + 1];
							var southWest = input[y - 1][x - 1];
							var southEast = input[y - 1][x + 1];

							str += northWest;
							str += northEast;
							str += southWest;
							str += southEast;

							var countM = str.Count(m => m == 'M');
							var countS = str.Count(s => s == 'S');

							var isXMas = countS == 2 && countM == 2;

							if (isXMas)
							{
								if (northWest != southEast && southWest != northEast)
								{
									masCounter++;
								}

							}


						}

					}

				}

			}

			Console.WriteLine($"p1: {xmasCounter}");
			Console.WriteLine($"p2: {masCounter}");
		}


	}
}