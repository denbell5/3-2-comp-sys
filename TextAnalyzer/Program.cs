using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
			var subdirNames = Directory.GetDirectories(FileHelper.TextFolderPath);
			var dirInfo = new DirectoryInfo(FileHelper.TextFolderPath);
			var subDirs = dirInfo.GetDirectories();

			foreach (var subDir in subDirs)
			{
				var files = subDir.GetFiles();
				foreach (var file in files)
				{
					var isTxt = file.Extension == FileHelper.Txt;
					TextAnalyzer defaultTextAnalyzer = null;
					if (isTxt)
					{
						var text = file.OpenText().ReadToEnd();
						defaultTextAnalyzer = new TextAnalyzer(text);
					}

					var bytes = FileHelper.ReadBytes(file);
					var base64String = CustomEncoder.ToBase64(bytes);
					var base64StringAnalyzer = new TextAnalyzer(base64String);

					WriteWithColor(ConsoleColor.Green, () => 
						Console.WriteLine(file.FullName));

					Console.WriteLine($"File size: {file.Length} bytes");
					if (defaultTextAnalyzer != null)
					{
						defaultTextAnalyzer.PrintInfo();
					}

					WriteWithColor(ConsoleColor.Green, () =>
						Console.WriteLine("Base64 info:"));

					base64StringAnalyzer.PrintInfo();
					Console.WriteLine();
				}
			}
		}
	}

	public static class FileHelper
	{
		public const string TextFolderPath = "../../../../texts/";
		public const string Txt = ".txt";

		public static string BuildPath(string fileName, string fileExtension)
		{
			return $"{TextFolderPath}{fileName}/{fileName}{fileExtension}";
		}

		public static long GetFileSize(string fileName, string fileExtension)
		{
			return new FileInfo(BuildPath(fileName, fileExtension)).Length;
		}

		public static byte[] ReadBytes(FileInfo fileInfo)
		{
			var fileSize = fileInfo.Length;
			byte[] buffer = new byte[fileSize];

			var stream = fileInfo.OpenRead();
			stream.Read(buffer);

			return buffer;
		}

		public static string ReadText(string fileName, string fileExtension)
		{
			var path = BuildPath(fileName, fileExtension);
			string text = null;
			using (var reader = new StreamReader(path))
			{
				text = reader.ReadToEnd();
			}
			return text;
		}
	}
}
