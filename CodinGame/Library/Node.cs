using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	public class Node
	{
		public int Id { get; set; }

		public List<Node> Nodes { get; private set; }

		public bool State { get; set; }

		public Node()
		{
			this.Nodes = new List<Node>();
		}
	}
}
