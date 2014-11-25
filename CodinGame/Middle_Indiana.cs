using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Middle_Indiana
{
	private enum Direction
	{
		TOP,
		RIGHT,
		LEFT,
		DOWN
	};

	#region Types de pieces

	private static Dictionary<int, Dictionary<Direction, Direction>> types = new Dictionary<int, Dictionary<Direction, Direction>>()
	{
		{0, new Dictionary<Direction, Direction>()},
		{
			1, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.DOWN},
				{Direction.LEFT, Direction.DOWN},
				{Direction.RIGHT, Direction.DOWN}
			}
		},
		{
			2, new Dictionary<Direction, Direction>()
			{
				{Direction.LEFT, Direction.RIGHT},
				{Direction.RIGHT, Direction.LEFT}
			}
		},
		{
			3, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.DOWN}
			}
		},
		{
			4, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.LEFT},
				{Direction.RIGHT, Direction.DOWN}
			}
		},
		{
			5, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.RIGHT},
				{Direction.LEFT, Direction.DOWN}
			}
		},
		{
			6, new Dictionary<Direction, Direction>()
			{
				{Direction.LEFT, Direction.RIGHT},
				{Direction.RIGHT, Direction.LEFT}
			}
		},
		{
			7, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.DOWN},
				{Direction.RIGHT, Direction.DOWN}
			}
		},
		{
			8, new Dictionary<Direction, Direction>()
			{
				{Direction.LEFT, Direction.DOWN},
				{Direction.RIGHT, Direction.DOWN}
			}
		},
		{
			9, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.DOWN},
				{Direction.LEFT, Direction.DOWN}
			}
		},
		{
			10, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.LEFT},
			}
		},
		{
			11, new Dictionary<Direction, Direction>()
			{
				{Direction.TOP, Direction.RIGHT},
			}
		},
		{
			12, new Dictionary<Direction, Direction>()
			{
				{Direction.RIGHT, Direction.DOWN},
			}
		},
		{
			13, new Dictionary<Direction, Direction>()
			{
				{Direction.LEFT, Direction.DOWN},
			}
		}
	};

	#endregion


	static void Main(String[] args)
	{
		string[] inputs;
		inputs = Console.ReadLine().Split(' ');
		int W = int.Parse(inputs[0]); // number of columns.
		int H = int.Parse(inputs[1]); // number of rows.
		int[,] map = new int[W,H];
		for (int i = 0; i < H; i++)
		{
			String LINE = Console.ReadLine(); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
			int[] line = LINE.Split(' ').Select(int.Parse).ToArray();
			for (int j = 0; j < W; ++j)
			{
				map[j, i] = line[j];
			}
		}
		int EX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).
		
		// game loop
		while (true)
		{
			inputs = Console.ReadLine().Split(' ');
			int XI = int.Parse(inputs[0]);
			int YI = int.Parse(inputs[1]);
			String POS = inputs[2];

			Direction from;
			if (Enum.TryParse(POS, true, out from))
			{
				int piece = map[XI, YI];
				if (types.ContainsKey(piece))
				{
					var directions = types[piece];
					if (directions.ContainsKey(from))
					{
						Direction nextDirection = directions[from];
						int x = XI, y = YI;
						switch (nextDirection)
						{
							case Direction.LEFT:
								x--;
								break;
							case Direction.RIGHT:
								x++;
								break;
							case Direction.DOWN:
								y++;
								break;
							case Direction.TOP:
								y--;
								break;
						}

						Console.WriteLine("{0} {1}", x, y);
					}
					else
					{
						Console.WriteLine("Blocked");
					}
				}
				else
				{
					Console.WriteLine("Invalid piece");
				}
			}
			else
			{
				Console.WriteLine("Can not parse " + POS);
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");

			//Console.WriteLine("0 0"); // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
		}
	}
}