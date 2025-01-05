using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 19: Linen Layout.
namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var (patterns, towels) = ParseInput("Day19Input.txt");

            // Part 1.            
            var noPossibleTowels = 0;
            foreach (var towel in towels)
            {                
                if (IsTowelPossible(towel, patterns))
                    noPossibleTowels++;                
            }
            Console.WriteLine($"Part 1 answer: {noPossibleTowels}");

            // Part 2.
            long totalNoTowelCombos = 0;
            foreach (var towel in towels)
                totalNoTowelCombos += FindNoTowelCombinations(towel, patterns);
            Console.WriteLine($"Part 2 answer: {totalNoTowelCombos}");
        }

        // Parse linen input.
        static (string[], string[]) ParseInput(string inputfile)
        {
            var inputText = File.ReadAllText(inputfile);
            var inputChunks = inputText.Split("\n\n");
            var patterns = inputChunks[0].Split(", ");
            var towels = inputChunks[1].Split("\n");
            return (patterns, towels);
        }

        // Get total number of towels that a set of patterns can make.
        // Need more efficient dynamic programming approach for part 2 vv
        static long FindNoTowelCombinations(string towel, string[] patterns)
        {            
            HashSet<string> patternsSet = new HashSet<string>(patterns);
            int towelLen = towel.Length;
            long[] dp = new long[towelLen + 1];
            dp[0] = 1;

            for (int i = 1; i <= towelLen; i++)
            {
                for (int length = 1; length <= i; length++) // Check substrings ending at i
                {
                    string substring = towel.Substring(i - length, length);
                    if (patternsSet.Contains(substring))                    
                        dp[i] += dp[i - length];                    
                }
            }

            return dp[towelLen];
        }    

        // Returns true if given towel is possible, false otherwise.
        static bool IsTowelPossible(string towel, string[] patterns)
        {
            var stack = new Stack<(string, int)>();
            var visited = new List<(string, int)>();
            var maxPatternLength = patterns.Max(p => p.Length);
            for (int i = 1; i <= maxPatternLength; i++) stack.Push((towel, i));
            while (stack.Count > 0)
            {
                var (towelRemaining, patternLen) = stack.Pop();                                               
                var towelSegment = towelRemaining.Substring(0, patternLen);
                visited.Add((towelRemaining, patternLen));
                if (patterns.Contains(towelSegment))
                {
                    towelRemaining = towelRemaining.Substring(patternLen);
                    // Found a valid combo - so possible in at least 1 way.
                    if (towelRemaining == "") return true;
                    // Now try all others.
                    for (int i = 1; i <= maxPatternLength; i++)
                    {
                        // Only add if there are possible substrings.
                        if (i <= towelRemaining.Length &&
                            patterns.Contains(towelRemaining.Substring(0, i))
                            && !visited.Contains((towelRemaining, i)))
                        {
                            stack.Push((towelRemaining, i));
                        }
                    }
                }                
            }
            // Not possible - exhausted all possibilities.
            return false;
        }
    }
}