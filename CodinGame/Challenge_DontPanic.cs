using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	class Challenge_DontPanic
	{
		class Node
		{
			public int Pos;
			public int Floor;

			public int Distance = 0;
			public bool NeedCreation = false;

			public List<Node> Nodes = new List<Node>();

			public Node Next
			{
				get { return Nodes[0]; }
			}

			public bool HasNext
			{
				get { return Nodes.Any(); }
			}

			public Node()
			{

			}
		}

		const string RIGHT = "RIGHT";
		const string LEFT = "LEFT";
		const string WAIT = "WAIT";
		const string BLOCK = "BLOCK";
		const string ELEVATOR = "ELEVATOR";

		static List<List<int>> elevators = new List<List<int>>();
		static int nbAdditionalElevators;

		static int m_exitFloor;
		static int m_exitPos;
		static int nbTotalClones;

		static Node currentNode = null;

		public static void Main(String[] args)
		{
			string[] inputs;
			string temp = Console.ReadLine();
			inputs = temp.Split(' ');
			int nbFloors = int.Parse(inputs[0]); // number of floors
			for (int i = 0; i < nbFloors; ++i)
			{
				elevators.Add(new List<int>());
			}

			int width = int.Parse(inputs[1]); // width of the area
			int nbRounds = int.Parse(inputs[2]); // maximum number of rounds

			int exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
			int exitPos = int.Parse(inputs[4]); // position of the exit on its floor
			m_exitFloor = exitFloor;
			m_exitPos = exitPos;


			nbTotalClones = int.Parse(inputs[5]); // number of generated clones
			nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
			int nbElevators = int.Parse(inputs[7]); // number of elevators
			bool path = false;

			Console.Error.WriteLine(temp);

			for (int i = 0; i < nbElevators; i++)
			{
				temp = Console.ReadLine();
				inputs = temp.Split(' ');
				int elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
				int elevatorPos = int.Parse(inputs[1]); // position of the elevator on its floor

				elevators[elevatorFloor].Add(elevatorPos);

				Console.Error.WriteLine(temp);
			}

			// game loop
			while (true)
			{
				inputs = Console.ReadLine().Split(' ');
				int cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
				int clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
				String direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT

				if (!path)
				{
					Path(clonePos, exitFloor, exitPos);
					path = true;
				}

				if (currentNode.Floor == cloneFloor)
				{
					if (currentNode.Pos == clonePos)
					{
						if (currentNode.NeedCreation)
						{
							currentNode.NeedCreation = false;
							Console.WriteLine(ELEVATOR);
							continue;
						}
						else if (currentNode.HasNext && currentNode.Next.Floor == currentNode.Floor)
						{
							Console.Error.WriteLine("Move from floor " + currentNode.Pos + " => " + currentNode.Next.Pos);
							if ((direction == RIGHT && currentNode.Next.Pos < currentNode.Pos) ||
								(direction == LEFT && currentNode.Next.Pos > currentNode.Pos))
							{
								Console.WriteLine(BLOCK);
							}
							else
							{
								Console.WriteLine(WAIT);
							}
						}
						else
						{
							Console.WriteLine(WAIT);
						}

						currentNode = currentNode.Next;
						continue;
					}
					else
					{
						if ((direction == RIGHT && currentNode.Pos < clonePos) ||
							(direction == LEFT && currentNode.Pos > clonePos))
						{
							Console.WriteLine(BLOCK);
							continue;
						}
					}
				}

				Console.WriteLine(WAIT);
			}
		}

		static void Path(int generatorPos, int exitFloor, int exitPos)
		{
			Node generator = new Node() { Pos = generatorPos, Floor = 0, Distance = 0 };

			Generate(generator, exitFloor, exitPos, nbAdditionalElevators);

			CalcDistance(generator, RIGHT, nbTotalClones);
			Clean(generator);
			currentNode = generator;

			//Display(currentNode);
		}

		static void Display(Node node)
		{
			Console.Error.WriteLine("Node " + " floor = " + node.Floor + " Pos = " + node.Pos + " create = " + node.NeedCreation);
			if (node.HasNext)
				Display(node.Next);
		}

		static void AddNext(List<int> elevs, List<int> nextElevs, Node root, bool canCreate)
		{
			bool hasLeft = false, hasRight = false;
			int leftPos = 0, rightPos = 0;
			int leftMin = int.MaxValue, rightMin = int.MaxValue;
			int dist;

			int pos = root.Pos;
			foreach (int e in elevs)
			{
				dist = Math.Abs(e - pos);
				if (e < pos) //left
				{
					if (dist < leftMin)
					{
						leftMin = dist;
						leftPos = e;
						hasLeft = true;
					}
				}
				else if (e > pos)
				{
					if (dist < rightMin)
					{
						rightMin = dist;
						rightPos = e;
						hasRight = true;
					}
				}
				else //=, pas le choix
				{
					Node sub = new Node() { Pos = root.Pos, Floor = root.Floor };
					Node next = new Node() { Pos = root.Pos, Floor = root.Floor + 1 };

					root.Nodes.Add(sub);
					sub.Nodes.Add(next);

					return;
				}
			}

			if (hasLeft)
			{
				Node sub = new Node() { Pos = leftPos, Floor = root.Floor };
				Node next = new Node() { Pos = leftPos, Floor = root.Floor + 1 };

				root.Nodes.Add(sub);
				sub.Nodes.Add(next);
			}
			if (hasRight)
			{
				Node sub = new Node() { Pos = rightPos, Floor = root.Floor };
				Node next = new Node() { Pos = rightPos, Floor = root.Floor + 1 };

				root.Nodes.Add(sub);
				sub.Nodes.Add(next);
			}

			int min = (hasLeft ? leftPos : int.MinValue);
			int max = (hasRight ? rightPos : int.MaxValue);

			if (nextElevs != null && canCreate)
			{
				List<int> selected = nextElevs.Where(x => min < x && x < max).ToList();
				if (selected.Any())
				{
					int last = selected.Last() + 1;
					if (last < max)
					{
						selected.Add(last);
					}
					int first = selected.First() - 1;
					if (min < first)
					{
						selected.Add(first);
					}
				}

				foreach (int e in selected)
				{
					Node sub = new Node() { Pos = e, Floor = root.Floor };
					Node next = new Node() { Pos = e, Floor = root.Floor + 1 };

					root.Nodes.Add(sub);
					sub.Nodes.Add(next);
					sub.NeedCreation = true;
				}
			}
			if (canCreate)
			{
				Node sub = new Node() { Pos = root.Pos, Floor = root.Floor };
				Node next = new Node() { Pos = root.Pos, Floor = root.Floor + 1 };

				root.Nodes.Add(sub);
				sub.Nodes.Add(next);
				sub.NeedCreation = true;
			}
			if (canCreate && min < m_exitPos && m_exitPos < max)
			{
				Node sub = new Node() { Pos = m_exitPos, Floor = root.Floor };
				Node next = new Node() { Pos = m_exitPos, Floor = root.Floor + 1 };

				root.Nodes.Add(sub);
				sub.Nodes.Add(next);
				sub.NeedCreation = true;
			}
		}

		static void Generate(Node root, int exitFloor, int exitPos, int moreElevators)
		{
			int floor = root.Floor;

			List<int> elevs = elevators[floor];

			if (floor == exitFloor)
			{
				if (root.Pos < exitPos)
				{
					if (elevs.Any(x => root.Pos <= x && x < exitPos))
					{
						return;
					}
				}
				else if (root.Pos > exitPos)
				{
					if (elevs.Any(x => exitPos < x && x <= root.Pos))
					{
						return;
					}
				}

				root.Nodes.Add(new Node() { Pos = exitPos, Floor = exitFloor });
			}
			else
			{
				AddNext(elevs, (floor + 1 < elevators.Count) ? elevators[floor + 1] : null, root, moreElevators > 0);
				int minus = moreElevators - 1;
				foreach (Node child in root.Nodes)
				{
					Generate(child.Next, exitFloor, exitPos, (child.NeedCreation) ? minus : moreElevators);
				}
			}
		}

		static void CalcDistance(Node root, string direction, int clones)
		{
			if (root.Nodes.Any() && clones > 0)
			{
				//trouver le min en calculant avec la distance
				Node nextOne = null;
				int min = int.MaxValue;

				foreach (Node n in root.Nodes)
				{
					bool changeDirection = false;
					bool useClone = false;
					if (root.Floor == n.Floor)
					{
						if ((direction == RIGHT && n.Pos < root.Pos) ||
							(direction == LEFT && n.Pos > root.Pos))
						{
							changeDirection = true;
							useClone = true;
						}
					}
					if (root.NeedCreation)
					{
						useClone = true;
					}


					CalcDistance(n, (changeDirection) ? ((direction == RIGHT) ? LEFT : RIGHT) : direction, (useClone ? clones - 1 : clones));
					int distance = n.Distance;
					if (distance >= 0)
					{
						if (n.Floor == root.Floor)
						{
							distance += (Math.Abs(n.Pos - root.Pos));
						}
						else
						{
							if (n.NeedCreation)
							{
								distance += 3; //tour perdu
							}
							else
							{
								distance++;
							}
						}
						if (changeDirection)
						{
							distance += 3;
						}


						if (distance < min)
						{
							min = distance;
							nextOne = n;
						}
					}
				}

				if (nextOne != null)
				{
					root.Nodes.Clear();
					root.Nodes.Add(nextOne);
					root.Distance = min;
				}
				else
				{
					root.Distance = -100;
				}
			}
			else if (root.Floor != m_exitFloor || root.Pos != m_exitPos || clones < 1)
			{
				root.Distance = -100;
			}
		}

		static void Clean(Node root)
		{
			if (root.HasNext)
			{
				if (root.Floor == root.Next.Floor && root.Pos == root.Next.Pos)
				{
					Node next = root.Next.HasNext ? root.Next.Next : null;

					root.NeedCreation = root.NeedCreation || root.Next.NeedCreation;

					root.Nodes.Clear();
					if (next != null)
					{
						root.Nodes.Add(next);
					}
				}

				if (root.HasNext)
					Clean(root.Next);
			}
		}
	}
}
