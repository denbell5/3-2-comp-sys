using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalyzer
{
	public class TextAnalyzer
	{
		public string Text { get; }
		public HashSet<char> Alphabet { get; }

		public TextAnalyzer(string text)
		{
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

		public void PrintInfo()
		{
			Console.WriteLine($"Text size: {Text.Length} bytes");
			Console.WriteLine($"Alphabet count: {Alphabet.Count}");
			Console.WriteLine($"Average entropy: {GetAvgEntropy():F3} bits");
			Console.WriteLine($"Info quantity: {GetInfoQuantity() / 8:F3} bytes");
		}
	}
}
