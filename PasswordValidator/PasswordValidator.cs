using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PasswordValidator
{
    internal class PasswordValidator
    {
        private const string PasswordValidString = "Password Is Valid";
        private const string PasswordInvalidString = "Password Is Invalid";
        private const string StringIsInInvalidFormat = "String Is In Invalid Format";
        private const string FileIsEmpty = "File is empty. Press any key and try again.";
        private const char LineSeparator = ' ';
        private static readonly char[] SymbolsCountSeparators = { '-', ':' };

        public static void Main()
        {
            var passwordValidator = new PasswordValidator();
            passwordValidator.CheckForInputAndValidate();
        }

        private IEnumerable<string> ReadFile(string filePath) => File.ReadLines(filePath);

        private void CheckForInputAndValidate()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter filename: ");
                var fileName = Console.ReadLine();

                if (File.Exists(fileName))
                {
                    CheckForValidPassword(fileName);
                    break;
                }

                Console.WriteLine("File not found. Press any key and try again.");
                Console.ReadKey();
            }
        }

        private void CheckForValidPassword(string fileName)
        {
            var lines = ReadFile(fileName).ToList();

            if (!lines.Any())
            {
                Console.WriteLine(FileIsEmpty);
                Console.ReadKey();
                return;
            }
            foreach (var line in lines)
            {
                var validResult = ParseData(line);

                Console.Write($"{line} ");

                Console.ForegroundColor = validResult == PasswordValidString ? ConsoleColor.Green : ConsoleColor.Red;

                Console.WriteLine(validResult);
                Console.ResetColor();
            }
            Console.ReadLine();
        }

        private static string ParseData(string line)
        {
            var lineResult = line.Split(LineSeparator);

            if (lineResult.Length < 3
                || string.IsNullOrWhiteSpace(lineResult[0])
                || string.IsNullOrWhiteSpace(lineResult[1])
                || string.IsNullOrWhiteSpace(lineResult[2]))
                return StringIsInInvalidFormat;

            var symbolCount = lineResult[1].Split(SymbolsCountSeparators, StringSplitOptions.RemoveEmptyEntries);

            if (symbolCount.Length != 2
                || lineResult[0].Length != 1
                || !int.TryParse(symbolCount[0], out var minCount)
                || !int.TryParse(symbolCount[1], out var maxCount))
                return StringIsInInvalidFormat;

            var count = lineResult[2].Count(c => c == lineResult[0][0]);

            return count >= minCount && count <= maxCount ? PasswordValidString : PasswordInvalidString;
        }
    }
}