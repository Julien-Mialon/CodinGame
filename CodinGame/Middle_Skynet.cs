using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	public class Middle_Skynet
	{
		private List<Node> _graphNodes = new List<Node>();
		private List<int> _gateways = new List<int>();

		public Middle_Skynet(int nodes)
		{
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


		static void Main(String[] args)
		{
			string[] inputs;
			inputs = Console.ReadLine().Split(' ');
			int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
			int L = int.Parse(inputs[1]); // the number of links
			int E = int.Parse(inputs[2]); // the number of exit gateways

			Middle_Skynet solver = new Middle_Skynet(N);

			for (int i = 0; i < L; i++)
			{
				inputs = Console.ReadLine().Split(' ');
				int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
				int N2 = int.Parse(inputs[1]);

				solver.AddLink(N1, N2);
			}
			for (int i = 0; i < E; i++)
			{
				int EI = int.Parse(Console.ReadLine()); // the index of a gateway node

				solver.AddGateway(EI);
			}

			// game loop
			while (true)
			{
				int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

				// Write an action using Console.WriteLine()
				// To debug: Console.Error.WriteLine("Debug messages...");

				Console.WriteLine("0 1"); // Example: 0 1 are the indices of the nodes you wish to sever the link between
			}
		}
	}
}
