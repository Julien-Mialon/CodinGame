using System;
using System.Collections.Generic;
using System.Linq;

namespace CodinGame
{
	public static class Extension
	{
		public static T MinElement<T>(this IEnumerable<T> source, Func<T, int> selector)
		{
			int min = int.MaxValue;
			T result = default(T);
			foreach (T item in source)
			{
				int value = selector(item);
				if (value < min)
				{
					min = value;
					result = item;
				}
			}

			return result;
		}
	}

	public class Middle_Skynet
	{
		class Node
		{
			public int Id { get; set; }

			public List<Node> Nodes { get; private set; }

			public bool State { get; set; }

			public int Distance { get; set; }

			public Node()
			{
				Nodes = new List<Node>();
			}

			public Node(int id) : this()
			{
				Id = id;
			}
		}

		private readonly List<Node> _graphNodes = new List<Node>();
		private readonly List<int> _gateways = new List<int>();

		public Middle_Skynet(int nodes)
		{
			_graphNodes.AddRange(Enumerable.Range(0, nodes).Select(x => new Node(x)));

			for(int i = 0 ; i < nodes ; ++i)
			{
				_graphNodes.Add(new Node() { Id = i });
			}
		}

		public void AddGateway(int id)
		{
			_gateways.Add(id);
		}

		public void AddLink(int id1, int id2)
		{
			_graphNodes[id1].Nodes.Add(_graphNodes[id2]);
			_graphNodes[id2].Nodes.Add(_graphNodes[id1]);
		}

		public Tuple<int, int> Process(int skynetId)
		{
			_graphNodes.ForEach(x =>
			{
				x.Distance = -1;
				x.State = false;
			});

			Flood(_graphNodes, _graphNodes[skynetId]);
			int gatewayIndex = ClosestGateway();

			if (gatewayIndex < 0)
			{
				return Tuple.Create(-42, -42);
			}
			
			List<Node> path = new List<Node>();
			Node current = _graphNodes[gatewayIndex];
			path.Insert(0, current);
			while (current.Id != skynetId)
			{
				current = current.Nodes.MinElement(x => x.Distance);
				path.Insert(0, current);
			}

			path[0].Nodes.Remove(path[1]);
			path[1].Nodes.Remove(path[0]);

			return Tuple.Create(path[0].Id, path[1].Id);
		}

		private void FordFulkerson(int source, int destination)
		{
			int size = _graphNodes.Count;
			int[,] flow = new int[size,size];

			//List<Node> nodes = CopyNodes();
			
			//use backtracking to get a path from source to destination

			List<Node> path = new List<Node>();
			Node current = _graphNodes[destination];
			while (current.Id != source)
			{
				path.Insert(0, current);
				current = current.Nodes.MinElement(x => x.Distance);
			}
		}

		private List<Node> CopyNodes()
		{
			List<Node> result = _graphNodes.Select(x => new Node(x.Id)).ToList();

			for (int i = 0; i < _graphNodes.Count; ++i)
			{
				foreach (Node next in _graphNodes[i].Nodes)
				{
					result[i].Nodes.Add(result[next.Id]);
				}
			}

			return result;
		} 

		private int ClosestGateway()
		{
			int min = int.MaxValue;
			int minIndex = -1;
			foreach (int index in _gateways)
			{
				int distance = _graphNodes[index].Distance;
				Console.Error.WriteLine("Distance to gateway " + distance);

				if (distance >= 0 && distance < min)
				{
					min = distance;
					minIndex = index;
				}
			}

			return minIndex;
		}

		private void Flood(List<Node> nodeList, Node rootNode)
		{
			nodeList.ForEach(x => x.Distance = -1);

			rootNode.Distance = 0;

			Queue<Node> nodes = new Queue<Node>();
			nodes.Enqueue(rootNode);

			while (nodes.Count > 0)
			{
				Node node = nodes.Dequeue();
				
				foreach (Node next in node.Nodes.Where(x => x.Distance < 0))
				{
					next.Distance = node.Distance + 1;
					nodes.Enqueue(next);
				}
			}

			nodeList.ForEach(x => Console.Error.WriteLine("Node({0}) is at distance {1}", x.Id, x.Distance));
		}

		static void Main(string[] args)
		{
			string[] inputs;
			inputs = Console.ReadLine().Split(' ');
			int n = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
			int l = int.Parse(inputs[1]); // the number of links
			int e = int.Parse(inputs[2]); // the number of exit gateways

			Middle_Skynet solver = new Middle_Skynet(n);

			for (int i = 0; i < l; i++)
			{
				inputs = Console.ReadLine().Split(' ');
				int n1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
				int n2 = int.Parse(inputs[1]);

				solver.AddLink(n1, n2);
			}
			for (int i = 0; i < e; i++)
			{
				int ei = int.Parse(Console.ReadLine()); // the index of a gateway node

				solver.AddGateway(ei);
			}

			// game loop
			while (true)
			{
				int si = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

				Tuple<int, int> linkToCut = solver.Process(si);

				Console.WriteLine("{0} {1}", linkToCut.Item1, linkToCut.Item2);

				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");

				//Console.WriteLine("0 1"); // Example: 0 1 are the indices of the nodes you wish to sever the link between
			}
		}
	}
}
