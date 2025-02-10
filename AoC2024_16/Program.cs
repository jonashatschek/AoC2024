using System.Drawing;

namespace AoC2024_16
{
	internal class Program
	{
		//public static List<int[]> Directions = [[1, 0], [-1, 0], [0, 1], [0, -1]]; //e, w, s, n
		public static List<int[]> Directions = [[0, -1], [1, -1], [1, 0], [1, 1], [0, 1], [-1, 1], [-1, 0], [-1, -1]]; //n, ne, e, se, s, sw, w, nw

		private const bool useTestInput = false;

		private static Map map = new();

		private static int Y = 1;
		private static int X = 0;
		static void Main(string[] args)
		{
			var path = useTestInput ? "Test" : "Real";

			var input = File.ReadAllLines($"{path}/map.txt");

			map.Open = [];
			for (var y = 0; y < input.Length; y++)
			{
				for (var x = 0; x < input[y].Length; x++)
				{
					var letter = input[y][x];
					if (letter == '.')
					{
						map.Open.Add(new int[x, y]);
					}
					else if (letter == 'E')
					{
						map.End = new int[x, y];
					}
					else if (letter == 'S')
					{
						map.Start = new int[x, y];
					}
				}
			}

			while (true)
			{
				//Begin at the starting point A
				//add it to an “open list”


				//look at all the reachable or walkable squares adjacent to the starting point
				//Add them to the open list, too. For each of these squares, save point A as its “parent square”.

				//Drop the starting square A from your open list, and add it to a “closed list”

				//move through each neighbouring square. add the one with lowest cost (F)

				//F = G + H
				//G: The length of the path from the start node to this node.
				//H: The straight-line distance from this node to the end node.
				//F: Estimated total distance / cost.
				//Open / closed state: Can be one of three states: not tested yet; open; closed.
				//Parent node: The previous node in this path.Always null for the starting node.
				//Is walkable: Boolean value indicating whether the node can be used.
				//Location: Keep a record of this node’s location in order to calculate distance to other locations.


				//move until shortest path has been found
			}

			//todo implement a*




			Console.WriteLine("Hello, World!");
		}

		public bool aStar(int[,] start, int[,] end, int cost)
		{

		}

		private List<Point> GetAdjacentLocations(Point location)
		{
			foreach (var direction in Directions)
			{
				var adjacentX = location.X + direction[0];
				var adjacentY = location.Y + direction[1];

				if (adjacentX >= 0 && adjacentX <= map.Open.Max(walkable => walkable.X))
				{

				}

				if (adjacentY >= 0 && adjacentY <= map.Open.Max(walkable => walkable.X))
				{

				}
			}
		}

		private List<Node> GetAdjacentWalkableNodes(Node fromNode)
		{
			List<Node> walkableNodes = new List<Node>();
			IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

			foreach (var location in nextLocations)
			{
				int x = location.X;
				int y = location.Y;

				// Stay within the grid's boundaries
				if (x < 0 || x >= this.width || y < 0 || y >= this.height)
					continue;

				Node node = this.nodes[x, y];
				// Ignore non-walkable nodes
				if (!node.IsWalkable)
					continue;

				// Ignore already-closed nodes
				if (node.State == NodeState.Closed)
					continue;

				// Already-open nodes are only added to the list if their G-value is lower going via this route.
				if (node.State == NodeState.Open)
				{
					float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
					float gTemp = fromNode.G + traversalCost;
					if (gTemp < node.G)
					{
						node.ParentNode = fromNode;
						walkableNodes.Add(node);
					}
				}
				else
				{
					// If it's untested, set the parent and flag it as 'Open' for consideration
					node.ParentNode = fromNode;
					node.State = NodeState.Open;
					walkableNodes.Add(node);
				}
			}

			return walkableNodes;
		}
	}

	public class Map
	{
		public Point Start { get; set; }
		public Point End { get; set; }
		public List<Point> Open { get; set; }
		public static bool[,] Visited { get; set; }

		//public Tile 
	}

	public enum Tile
	{
		Wall,
		Path,
		Start,
		Goal
	}

	public class Reindeer
	{
		public Point Position { get; set; }
		public Direction Direction { get; set; }

	}

	public class Node
	{
		public Point Location { get; private set; }
		public bool IsWalkable { get; set; }
		public float G { get; set; }
		public float H { get; set; }
		public float F => G + H;
		public NodeState State { get; set; }
		public Node ParentNode { get { ... } set { ... } }
    }

	public enum NodeState
	{ Untested, Open, Closed }

	public enum Direction
	{
		East,
		West,
		South,
		North
	}
}
