using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 21: Keypad Conundrum.
namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = "Day21Input.txt";
            var codes = File.ReadLines(inputFile);

            // Part 1 - this is rather slow, takes a few minutes.            
            var numericKeypad = new Keypad(KeypadType.Numeric);
            var directionalKeypad = new Keypad(KeypadType.Directional);           
            var totalComplexity = 0L;
            foreach (var code in codes)
            {
                var paths = numericKeypad.EnterCode(code);
                var paths2 = new List<string>();
                foreach (var path in paths)
                    paths2.AddRange(directionalKeypad.EnterCode(path));
                var paths3 = new List<string>();
                foreach (var path in paths2)
                    paths3.AddRange(directionalKeypad.EnterCode(path));
                totalComplexity += paths3.Select(p => Complexity(p.Length, code)).Min();
            }
            Console.WriteLine($"Part 1 answer: {totalComplexity}");

            // Part 2 - more efficient approach required so it doesn't run forever.
            long totalComplexity2 = 0L;            
            foreach (var code in codes)
            {
                var directions = numericKeypad.EnterCode(code);
                var shortest = long.MaxValue;
                foreach (var direction in directions)
                {
                    var length = GetShortestDirs(directionalKeypad, direction, 25, new Dictionary<(string, int), long>());
                    if (length < shortest) shortest = length;
                }
                totalComplexity2 += Complexity(shortest, code);
            }
            Console.WriteLine($"Part 2 answer: {totalComplexity2}");
        }

        // Recursively get shortest sequence.
        static long GetShortestDirs(Keypad kp, string directions, int depth, Dictionary<(string,int), long> cache)
        {
            if (depth == 0)
                return directions.Length;

            if (cache.ContainsKey((directions, depth)))
                return cache[(directions, depth)];

            // Chunk up into sections ending in A.
            var directionChunks = directions.Split("A").Select(dc => dc + "A").ToList();
            directionChunks.RemoveAt(directionChunks.Count - 1);

            var total = 0L;
            foreach (var directionChunk in directionChunks)
            {
                var sequenceList = kp.EnterCode(directionChunk);
                var shortest = long.MaxValue;
                foreach (var sequence in sequenceList)
                {
                    var size = GetShortestDirs(kp, sequence, depth - 1, cache);
                    if (size < shortest) shortest = size;
                }
                total += shortest;
            }

            cache[(directions, depth)] = total;
            return total;
        }

        // Get the complexity which is product of directions x numeric part of code.
        static long Complexity(long noDirections, string code)
        {
            return int.Parse(code.Replace("A", "")) * noDirections;
        }
    }
}