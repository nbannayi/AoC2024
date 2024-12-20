using System;
using System.Collections.Generic;
using System.IO;

// Day 4: Ceres Search.
namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse input into an array.
            var wordSearch = File.ReadAllLines("Day04Input.txt");

            var totalXmasOccurrences = 0;
            totalXmasOccurrences += GetXmasOccurrences(GetHorizontalStripes(wordSearch, false));
            totalXmasOccurrences += GetXmasOccurrences(GetHorizontalStripes(wordSearch, true));
            totalXmasOccurrences += GetXmasOccurrences(GetVerticalStripes(wordSearch, false));
            totalXmasOccurrences += GetXmasOccurrences(GetVerticalStripes(wordSearch, true));
            totalXmasOccurrences += GetXmasOccurrences(GetLeftDownDiagonalStripes(wordSearch, false));
            totalXmasOccurrences += GetXmasOccurrences(GetLeftDownDiagonalStripes(wordSearch, true));
            totalXmasOccurrences += GetXmasOccurrences(GetRightDownDiagonalStripes(wordSearch, false));
            totalXmasOccurrences += GetXmasOccurrences(GetRightDownDiagonalStripes(wordSearch, true));

            Console.WriteLine($"Part 1 answer: {totalXmasOccurrences}");
            Console.WriteLine($"Part 2 answer: {GetX_masOccurrences(wordSearch)}");
        }

        // Determine number of times XMAS appears.
        static int GetXmasOccurrences(List<string> stripes)
        {
            var total = 0;
            foreach (string stripe in stripes)
                total += stripe.Split("XMAS").Length - 1;
            return total;
        }

        // Get X_mas blocks in word search.
        static int GetX_masOccurrences(string[] wordSearch)
        {
            var totalOccurences = 0;
            for (var r = 0; r < wordSearch.Length - 2; r++)
            {
                for (var c = 0; c < wordSearch[0].Length - 2; c++)
                {
                    var block = new List<string>();
                    block.Add(wordSearch[r].Substring(c, 3));
                    block.Add(wordSearch[r+1].Substring(c, 3));
                    block.Add(wordSearch[r+2].Substring(c, 3));
                    if (ContainsX_mas(block))
                        totalOccurences++;
                }                
            }            
            return totalOccurences;
        }

        // Determine if a 3x3 grid contains X_Mas in any orientation.
        static bool ContainsX_mas(List<string> threeByThreeGrid)
        {
            var x_masMask =
                threeByThreeGrid[0][0].ToString() +
                threeByThreeGrid[0][2].ToString() +
                threeByThreeGrid[1][1].ToString() +
                threeByThreeGrid[2][0].ToString() +
                threeByThreeGrid[2][2].ToString();

            bool contains;
            switch (x_masMask)
            {
                case "MSAMS":
                    contains = true;
                    break;
                case "SSAMM":
                    contains = true;
                    break;
                case "SMASM":
                    contains = true;
                    break;
                case "MMASS":
                    contains = true;
                    break;
                default:
                    contains = false;
                    break;
            }
            return contains;
        }

        // Get all horizontal stripes, go in reverse if flag passed.
        static List<string> GetHorizontalStripes(string[] wordSearch, bool reverse)
        {
            List<string> stripes = new List<string>();
            foreach (string stripe in wordSearch)            
                stripes.Add(reverse ? GetReverse(stripe) : stripe);            
            return stripes;
        }

        // Get all vertical stripes, go in reverse if flag passed.
        static List<string> GetVerticalStripes(string[] wordSearch, bool reverse)
        {
            List<string> stripes = new List<string>();            
            for (var c = 0; c < wordSearch[0].Length; c++)
            {
                var stripe = "";
                for (var r = 0; r < wordSearch.Length; r++)
                    stripe += wordSearch[r][c].ToString();
                stripes.Add(reverse ? GetReverse(stripe) : stripe);
            }
            return stripes;
        }

        // Get all diagonal left down stripes, go in reverse if flag passed.
        static List<string> GetLeftDownDiagonalStripes(string[] wordSearch, bool reverse)
        {
            List<string> stripes = new List<string>();
            var row = 0; var col = 0;
            while (row < wordSearch.Length)
            {
                var r = row; var c = col;
                var stripe = "";
                while (r >= 0 && c < wordSearch[0].Length)
                {
                    stripe += wordSearch[r][c].ToString();
                    r--; c++;
                }
                stripes.Add(reverse ? GetReverse(stripe) : stripe);
                row++; col = 0;
            }
            row = wordSearch.Length - 1; col = 1;
            while (col < wordSearch[0].Length)
            {
                var r = row; var c = col;
                var stripe = "";
                while (r >= 0 && c < wordSearch[0].Length)
                {
                    stripe += wordSearch[r][c].ToString();
                    r--; c++;
                }
                stripes.Add(reverse ? GetReverse(stripe) : stripe);
                row = wordSearch.Length - 1; col++;
            }
            return stripes;
        }

        // Get all diagonal right down stripes, go in reverse if flag passed.
        static List<string> GetRightDownDiagonalStripes(string[] wordSearch, bool reverse)
        {
            List<string> stripes = new List<string>();
            var row = 0; var col = wordSearch[0].Length - 1;
            while (row < wordSearch.Length)
            {
                var r = row; var c = col;
                var stripe = "";
                while (r >= 0 && c >= 0)
                {
                    stripe += wordSearch[r][c].ToString();
                    r--; c--;
                }
                stripes.Add(reverse ? GetReverse(stripe) : stripe);
                row++; col = wordSearch[0].Length - 1;
            }
            row = wordSearch.Length - 1; col = wordSearch[0].Length - 2;
            while (col >= 0)
            {
                var r = row; var c = col;
                var stripe = "";
                while (r >= 0 && c >= 0)
                {
                    stripe += wordSearch[r][c].ToString();
                    r--; c--;
                }
                stripes.Add(reverse ? GetReverse(stripe) : stripe);
                row = wordSearch.Length - 1; col--;
            }
            return stripes;
        }

        // Reverse passed string.
        static string GetReverse(string s)
        {
            var revStr = "";
            foreach (var c in s)
                revStr = c.ToString() + revStr;
            return revStr;
        }
    }
}