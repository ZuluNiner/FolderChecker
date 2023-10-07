using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
	public class ConsoleWriter
	{
		public static Dictionary<string, ConsoleColor> colorDictionary = new Dictionary<string, ConsoleColor>()
		{
			{"black",ConsoleColor.Black },
			{"blue",ConsoleColor.Blue },
			{"cyan",ConsoleColor.Cyan },
			{"darkblue",ConsoleColor.DarkBlue },
			{"darkcyan",ConsoleColor.DarkCyan },
			{"darkgray",ConsoleColor.DarkGray },
			{"darkgreen",ConsoleColor.DarkGreen },
			{"darkmagenta",ConsoleColor.DarkMagenta },
			{"darkred",ConsoleColor.DarkRed },
			{"darkyellow",ConsoleColor.DarkYellow },
			{"gray",ConsoleColor.Gray },
			{"green",ConsoleColor.Green },
			{"magenta",ConsoleColor.Magenta },
			{"red",ConsoleColor.Red },
			{"white",ConsoleColor.White },
			{"yellow",ConsoleColor.Yellow },

		};

		public static void Write(string text, bool autoReturn = true, string foreground = "gray", string background = "black")
		{
			if (!colorDictionary.ContainsKey(foreground))
			{
				Console.WriteLine("INVALID COLOR (" + foreground + ") FOUND FOR FOREGROUND");
				return;
			}
			if (!colorDictionary.ContainsKey(background))
			{
				Console.WriteLine("INVALID COLOR (" + background + ") FOUND FOR BACKGROUND");
				return;
			}
			foreach (KeyValuePair<string, ConsoleColor> color in colorDictionary)
			{
				if (foreground == color.Key)
					Console.ForegroundColor = color.Value;
				if (background == color.Key)
					Console.BackgroundColor = color.Value;
			}
			if (autoReturn)
				Console.WriteLine(text);
			else
				Console.Write(text);

			ResetConsole();
		}

		static void ResetConsole()
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		public static void Rewrite(string text, bool autoReturn = false, string foreground = "gray", string background = "black")
		{
			if (!colorDictionary.ContainsKey(foreground))
			{
				Console.WriteLine("INVALID COLOR (" + foreground + ") FOUND FOR FOREGROUND");
				return;
			}
			if (!colorDictionary.ContainsKey(background))
			{
				Console.WriteLine("INVALID COLOR (" + background + ") FOUND FOR BACKGROUND");
				return;
			}
			foreach (KeyValuePair<string, ConsoleColor> color in colorDictionary)
			{
				if (foreground == color.Key)
					Console.ForegroundColor = color.Value;
				if (background == color.Key)
					Console.BackgroundColor = color.Value;
			}
			var curPos = Console.GetCursorPosition();
			Console.SetCursorPosition(0, curPos.Top);
			if (autoReturn)
				Console.WriteLine(text);
			else
				Console.Write(text);

			ResetConsole();
		}

		public static void Blank()
		{
			Console.WriteLine("");
		}
	}
}
