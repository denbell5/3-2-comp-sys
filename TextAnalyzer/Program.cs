using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextAnalyzer
{
	class Program
	{
		public static void WriteWithColor(ConsoleColor color, Action action)
		{
			var initColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			action();
			Console.ForegroundColor = initColor;
		}

		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine();
				Console.Write("Enter file name (without extension): ");
				string fileName = null;
				WriteWithColor(ConsoleColor.Green, () => 
				{
					fileName = Console.ReadLine();
					Console.WriteLine($"*** Analyzing {fileName} ***");
				});
				
				var analyzer = TextAnalyzer.FromFile(fileName);
				Console.WriteLine($"File size: {analyzer.FileInfo.Length} bytes");
				Console.WriteLine($"Alphabet count: {analyzer.Alphabet.Count}");
				Console.WriteLine($"Average entropy: {analyzer.GetAvgEntropy():F3} bits");
				Console.WriteLine($"Info quantity: {analyzer.GetInfoQuantity() / 8:F3} bytes");
				Console.WriteLine();

				while (true)
				{
					Console.Write("Enter symbol to count frequency (or 2 and more symbols to quit): ");
					var str = Console.ReadLine();
					if (str.Length != 1)
						break;

					var symbol = str.ToCharArray().First();
					Console.WriteLine(analyzer.GetSymbolFrequency(symbol));
					Console.WriteLine();
				}
			}
		}
	}

	public class TextAnalyzer
	{
		public string Text { get; }
		public FileInfo FileInfo { get; }
		public HashSet<char> Alphabet { get; }

		public static TextAnalyzer FromFile(string fileName)
		{
			string path = DirectoryHelper.BuildPath(fileName);
			var fileInfo = new FileInfo(path);
			string text = null;

			using (var reader = fileInfo.OpenText())
			{
				text = reader.ReadToEnd();
			}

			return new TextAnalyzer(fileInfo, text);
		}

		private TextAnalyzer(FileInfo fileInfo, string text)
		{
			FileInfo = fileInfo;
			Text = text;
			Alphabet = GetAlphabetFrom(text);
		}

		private HashSet<char> GetAlphabetFrom(string text)
		{
			var alphabet = new HashSet<char>();
			var distinct = text.ToCharArray().Distinct().ToList();
			distinct.ForEach(ch => alphabet.Add(ch));
			return alphabet;
		}

		public double GetSymbolFrequency(char symbol)
		{
			var symbolCount = Text.Count(ch => ch == symbol);
			var textLength = (double)Text.Length;
			return symbolCount / textLength;
		}

		public double GetAvgEntropy()
		{
			double result = 0;
			foreach (var symbol in Alphabet)
			{
				var freq = GetSymbolFrequency(symbol);
				result += freq * Math.Log2(1 / freq);
			}

			return result;
		}

		public double GetInfoQuantity()
		{
			var entropy = GetAvgEntropy();
			return entropy * Text.Length;
		}
	}

	public static class DirectoryHelper
	{
		public const string TextFolderPath = "../../../../texts/";
		public const string Txt = ".txt";

		public static string BuildPath(string fileName)
		{
			return TextFolderPath + fileName + Txt;
		}
	}
}
