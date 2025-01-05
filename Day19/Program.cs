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
            Console.WriteLine($"Part 2 answer: TODO.");            
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

        // Returns true if given towel is possible, false otherwise.
        static bool IsTowelPossible(string towel, string[] patterns)
        {
            // Remaining towel, pattern len.
            var stack = new Stack<(string, int)>();
            var visited = new List<(string, int)>();

            // Pare down possible patterns.
            var tempPatterns = new List<string>();
            foreach (var pattern in patterns)
                if (towel.Contains(pattern)) tempPatterns.Add(pattern);
            patterns = tempPatterns.ToArray();

            // Seed intial attempts.
            var maxPatternLength = patterns.Max(p => p.Length);
            for (int i = 1; i <= maxPatternLength; i++)
                stack.Push((towel, i));

            while (stack.Count > 0)
            {
                var (towelRemaining, patternLen) = stack.Pop();                                               
                var towelSegment = towelRemaining.Substring(0, patternLen);
                visited.Add((towelRemaining, patternLen));

                if (patterns.Contains(towelSegment))
                {
                    towelRemaining = towelRemaining.Substring(patternLen);

                    // Found a valid combo - so possible in at least 1 way.
                    if (towelRemaining == "")
                    {
                        return true;
                    }

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