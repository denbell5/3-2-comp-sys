using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalyzer
{
	public static class CustomEncoder
	{
		public const string Base64Symbols =
			"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
		public static string ConvertToBitString(byte[] bytes)
		{
			var bits = bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'));
			var bitsJoined = string.Join("", bits);
			return bitsJoined;
		}

		public static string ToBase64(byte[] bytes)
		{
			string result = "";

			string bitString = ConvertToBitString(bytes);
			int bitLength = bitString.Length;

			int requiredLength = bitLength + (6 - bitLength % 6);
			bitString = bitString.PadRight(requiredLength, '0');

			int sextetLength = bitLength / 6 + (requiredLength == bitLength ? 0 : 1);

			for (int i = 0; i < sextetLength; i += 1)
			{
				int sextetIndex = i * 6;
				string sextetString = bitString.Substring(sextetIndex, 6);
				int symbolIndex = Convert.ToInt32(sextetString, 2);
				result += Base64Symbols[symbolIndex];
			}

			int da = bitLength % 24;
			int padCount = da == 0 ? 0 : (int)Math.Pow((bitLength % 24) / 16.0, -1);
			result = result.PadRight(result.Length + padCount, Base64Symbols[64]);

			return result;
		}
	}
}
