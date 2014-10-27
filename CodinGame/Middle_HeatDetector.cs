using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	public class Middle_HeatDetector
	{
		const string Up = "U", UpLeft = "UL", UpRight = "UR", Down = "D", DownLeft = "DL", DownRight = "DR", Right = "R", Left = "L";

		private Dictionary<string, Tuple<int, int>> IncrementPosition = new Dictionary<string, Tuple<int, int>>()
		{
			{Up, new Tuple<int, int>(0, -1)},
			{UpLeft, new Tuple<int, int>(-1, -1)},
			{UpRight, new Tuple<int, int>(1, -1)},
			{Down, new Tuple<int, int>(0, 1)},
			{DownLeft, new Tuple<int, int>(-1, 1)},
			{DownRight, new Tuple<int, int>(1, 1)},
			{Left, new Tuple<int, int>(-1, 0)},
			{Right, new Tuple<int, int>(1, 0)}
		};

		private int _currentX;
		private int _currentY;

		private int _width;
		private int _height;

		private int _minX = 0;
		private int _minY = 0;
		private int _maxX;
		private int _maxY;

		public Middle_HeatDetector(int startX, int startY, int width, int height)
		{
			_currentX = startX;
			_currentY = startY;

			_width = width;
			_height = height;

			_maxX = _width;
			_maxY = _height;
		}

		public Tuple<int, int> NextPosition(string direction)
		{
			/* Next
			 *	min < x < max
			 *	min < y < max
			 */
			if(IsVerticalMove(direction))
			{
				if(IsUpMove(direction))
				{
					_maxY = _currentY;
				}
				else if(IsDownMove(direction))
				{
					_minY = _currentY;
				}

				_currentY = (_minY + _maxY) / 2;
			}

			if(IsHorizontalMove(direction))
			{
				if(IsLeftMove(direction))
				{
					_maxX = _currentX;
				}
				else if(IsRightMove(direction))
				{
					_minX = _currentX;
				}

				_currentX = (_minX + _maxX) / 2;
			}

			return new Tuple<int, int>(_currentX, _currentY);
		}

		private bool IsVerticalMove(string direction)
		{
			return !(direction == Right || direction == Left);
		}

		private bool IsHorizontalMove(string direction)
		{
			return !(direction == Up || direction == Down);
		}

		private bool IsUpMove(string direction)
		{
			return direction == Up || direction == UpLeft || direction == UpRight;
		}

		private bool IsDownMove(string direction)
		{
			return direction == Down || direction == DownLeft || direction == DownRight;
		}

		private bool IsLeftMove(string direction)
		{
			return direction == Left || direction == UpLeft || direction == DownLeft;
		}

		private bool IsRightMove(string direction)
		{
			return direction == Right || direction == UpRight || direction == DownRight;
		}

		static void Main(String[] args)
		{
			string[] inputs;
			inputs = Console.ReadLine().Split(' ');
			int W = int.Parse(inputs[0]); // width of the building.
			int H = int.Parse(inputs[1]); // height of the building.
			int N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
			inputs = Console.ReadLine().Split(' ');
			int X0 = int.Parse(inputs[0]);
			int Y0 = int.Parse(inputs[1]);

			Console.Error.WriteLine(string.Format("W {0} ; H {1} ; X {2} ; Y {3}", W, H, X0, Y0));

			Middle_HeatDetector heatDetector = new Middle_HeatDetector(X0, Y0, W, H);

			// game loop
			while (true)
			{
				String BOMB_DIR = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
				Console.Error.WriteLine("Direction " + BOMB_DIR);

				Tuple<int, int> res = heatDetector.NextPosition(BOMB_DIR);

				Console.WriteLine(string.Format("{0} {1}", res.Item1, res.Item2)); 
			}
		}
	}
}
