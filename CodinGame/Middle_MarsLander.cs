using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Middle_MarsLander
{
	static void Main(String[] args)
	{
		string[] inputs;
		int n = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.

		const int VERTICAL_MAX = 30;
		const int HORIZONTAL_MAX = 15;

		int landHeight = -9999;
		int landStart = -9999;
		int landEnd = 9999;
		bool found = false;
		for (int i = 0; i < n; i++)
		{
			inputs = Console.ReadLine().Split(' ');

			int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
			int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.

			Console.Error.WriteLine("X {0} Y {1}", landX, landY);
			if (!found)
			{
				if (landY == landHeight)
				{
					landEnd = landX;
					found = true;
				}
				else
				{
					landStart = landX;
					landHeight = landY;
				}
			}
		}

		Console.Error.WriteLine("Land [{0} ; {1}]", landStart, landEnd);

		// game loop
		while (true)
		{
			inputs = Console.ReadLine().Split(' ');
			int X = int.Parse(inputs[0]);
			int Y = int.Parse(inputs[1]);
			int HS = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
			int VS = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
			int F = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
			int R = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
			int P = int.Parse(inputs[6]); // the thrust power (0 to 4).


			int expectedX = X + (HS*(Math.Abs(HS)/3 + Math.Abs(R)/15 + 1));

			int rotation = R;
			if (X < landStart)
			{
				if (expectedX > landStart)
				{
					rotation += 15;
				}
				else
				{
					//rotation négative
					rotation -= 15;	
				}
			}
			else if (X > landEnd)
			{
				if (expectedX < landEnd)
				{
					rotation -= 15;
				}
				else
				{
					//rotation positive
					rotation += 15;
				}
			}
			else
			{
				//stabilisation horizontal
				if (HS < -1)
				{
					// rotation has to be negative
					rotation -= 15;
				}
				else if(HS > 1)
				{
					rotation += 15;
				}
				else
				{
					rotation += (rotation > 0) ? -15 : (rotation < 0) ? 15 : 0;
				}
			}

			int max = 45;
			if (landHeight > Y*0.75 && VS < 0)
			{
				max = 10;
			}
			rotation = (rotation < -max) ? -max : (rotation > max) ? max : rotation;

			if (landHeight > (Y + VS * 4))
			{
				rotation = 0;
			}

			int speed = 4;
			if (HS >= -2 && HS <= 2 && landStart < X && X < landEnd)
			{
				if (VS > -VERTICAL_MAX)
				{
					speed = 3;
				}
			}
			Console.WriteLine("{0} {1}", rotation, speed);
		}
	}

	
}