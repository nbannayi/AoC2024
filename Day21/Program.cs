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

            // Create robot keypads.
            var numericKeypad = new Keypad(KeypadType.Numeric);
            var directionalKeypad = new Keypad(KeypadType.Directional);
                                  
            // Part 1.
            var totalComplexity = 0;
            foreach (var code in codes)
            {
                var paths = numericKeypad.EnterCode(code);
                var paths2 = new List<string>();
                var paths3 = new List<string>();
                foreach (var path in paths)
                    paths2.AddRange(directionalKeypad.EnterCode(path));
                foreach (var path in paths2)
                    paths3.AddRange(directionalKeypad.EnterCode(path));                
                totalComplexity += paths3.Select(p => Complexity(p, code)).Min();
            }
            Console.WriteLine($"Part 1 answer: {totalComplexity}");            
        }

        // Get the complexity which is product of directions x numeric part of code.
        static int Complexity(string directions, string code)
        {
            return int.Parse(code.Replace("A", "")) * directions.Length;
        }
    }
}