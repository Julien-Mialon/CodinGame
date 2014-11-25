using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGame
{
	class Challenge_Nintendo
	{
		static void MainNintendo(string[] args)
		{
			string encoded = Encode("00000001000073af");
			string simpleEncoded = SimplifiedEncode("00000001000073af");

			Console.WriteLine(encoded);
			Console.WriteLine(simpleEncoded);
			Console.ReadKey();

			//Console.WriteLine("Expect");
			//Console.WriteLine("46508fb76677e201".ToUpperInvariant());
			Decode(32, "46508fb76677e201");

			Console.WriteLine(("\r\n" + "b0c152f9 ebf2831f" + "\r\n" + "ebf2831f b0c152f9").ToUpperInvariant());
			Console.ReadKey();
		}

		static int ToInt(string input)
		{
			int n = 0;

			for (int i = 1; i <= 8; ++i)
			{
				char c = input[i - 1];

				if (c >= '0' && c <= '9')
				{
					c -= '0';
				}
				else
				{
					c -= 'A';
					c += (char)10;
				}

				n += (c << ((8 - i) * 4));
			}

			return n;
		}

		static string ToHex(int input)
		{
			string result = "";

			for (int i = 7; i >= 0; --i)
			{
				int n = (input >> (i * 4)) & 0xF;
				result += n.ToString("X1");
			}

			return result;
		}

		static string SimplifiedEncode(string input)
		{
			string result = "";
			input = input.ToUpperInvariant();
			int size = input.Length * 2;

			int[] a = new int[size / 16];
			int[] b = new int[size / 16];

			for (int i = 0; i < size / 16; ++i)
			{
				a[i] = ToInt(input.Substring(i * 8, 8));
				b[i] = 0;
			}

			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j <= i; ++j)
				{
					b[(i + j) / 32] =
					(
						(							//equivalent i ou j
							(a[i / 32] >> (i % 32)) &
							(a[j / 32 + size / 32] >> (j % 32)) &
							1
						) << ((i + j) % 32)
					)
					^
					(
						(							//equivalent i ou j
							(a[j / 32] >> (j % 32)) &
							(a[i / 32 + size / 32] >> (i % 32)) &
							1
						) << ((i + j) % 32)
					);						//equivalent i ou j
				}
			}

			for (int i = 0; i < size / 16; ++i)
			{
				result += ToHex(b[i]) + " ";
			}

			return result;
		}

		static string Encode(string input)
		{
			string result = "";
			input = input.ToUpperInvariant();
			int size = input.Length * 2;

			int[] a = new int[size / 16];
			int[] b = new int[size / 16];

			for (int i = 0; i < size / 16; ++i)
			{
				a[i] = ToInt(input.Substring(i * 8, 8));
				b[i] = 0;
			}

			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j < size; ++j)
				{
					b[(i + j) / 32] ^= (							//equivalent i ou j
						(a[i / 32] >> (i % 32)) &
						(a[j / 32 + size / 32] >> (j % 32)) &
						1
					) << ((i + j) % 32);						//equivalent i ou j
				}
			}

			for (int i = 0; i < size / 16; ++i)
			{
				result += ToHex(b[i]) + " ";
			}

			return result;
		}

		static void Decode(int size, string input)
		{
			input = input.ToUpperInvariant();
			int[] a = new int[size / 16];
			int[] b = new int[size / 16];

			for (int i = 0; i < size / 16; ++i)
			{
				b[i] = ToInt(input.Substring(i * 8, 8));
				a[i] = 0;
			}

			int zero = 0;
			for (int i = 0; i < size; ++i)
			{
				for (int j = 0; j < size; ++j)
				{
					int n = b[(i + j) / 32];
					int bit = (n >> ((i + j) % 32)) & 1;

					if (bit == 1)
					{
						a[i / 32] |= 1 << (i % 32);
						a[j / 32 + size / 32] |= 1 << (j % 32);
					}
					else
					{
						zero++;
					}
				}
			}
			Console.WriteLine("zero = " + zero);

			string result = "";

			for (int i = 0; i < size / 16; ++i)
			{
				result += ToHex(a[i]) + " ";
			}

			Console.WriteLine(result);
		}
	}
}
