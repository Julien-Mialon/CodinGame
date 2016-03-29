using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	public class Middle_APU
	{
		class Point
		{
			public Point(int x, int y)
			{
				X = x;
				Y = y;
			}

			public int X { get; set; }

			public int Y { get; set; }
		}

		static void Main_Middle_APU(string[] args)
		{
			int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
			int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
			List<string> lines = new List<string>();
			for (int i = 0; i < height; i++)
			{
				string line = Console.ReadLine(); // width characters, each either 0 or .
				lines.Add(line);
			}

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					if (lines[y][x] == '0')
					{
						Point right = FindNext(lines, x+1, y, 1, 0, width, height);
						Point bot = FindNext(lines, x, y+1, 0, 1, width, height);

						Console.WriteLine("{0} {1} {2} {3} {4} {5}", x, y, right.X, right.Y, bot.X, bot.Y);
					}
				}
			}

			// Write an action using Console.WriteLine()
			// To debug: Console.Error.WriteLine("Debug messages...");

			//Console.WriteLine("0 0 1 0 0 1"); // Three coordinates: a node, its right neighbor, its bottom neighbor
		}

		private static Point FindNext(List<string> lines, int x, int y, int xInc, int yInc, int width, int height)
		{
			for (; x < width && y < height; x += xInc, y += yInc)
			{
				if (lines[y][x] == '0')
				{
					return new Point(x, y);
				}
			}
			return new Point(-1, -1);
		}
	}
}
