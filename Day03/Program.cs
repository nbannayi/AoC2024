using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// Day 3: Mull It Over.
namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse input into a list of mul calcs.            
            var memoryLines = File.ReadAllLines("Day03Input.txt");

            // Process and add up all the calcs.
            var calcsList1 = getCalcList(memoryLines, false);
            var calcsList2 = getCalcList(memoryLines, true);

            Console.WriteLine($"Part 1 answer: {getTotalCalcs(calcsList1)}");
            Console.WriteLine($"Part 2 answer: {getTotalCalcs(calcsList2)}");
        }

        // Get total of all calculations.
        static long getTotalCalcs(List<string[]> calcsList)
        {
            var totalCalcs = 0L;
            foreach (var calcs in calcsList)
                foreach (var calc in calcs)
                    totalCalcs += processCalc(calc);
            return totalCalcs;
        }

        // Get calcs list.
        static List<string[]> getCalcList(string[] memoryLines, bool conditional)
        {
            var regex = new Regex(@"do\(\)|don't\(\)|mul\(\d{1,3},\d{1,3}\)");
            var calcsList = new List<string[]>();
            var enabled = true;
            foreach (var memoryLine in memoryLines)
            {
                var matches = regex.Matches(memoryLine);                
                var calcs = new List<string>();
                foreach (Match match in matches)
                {
                    if (match.Value == "don't()")
                        enabled = false;
                    else if (match.Value == "do()")
                        enabled = true;
                    else
                    {
                        if ((conditional && enabled) || !conditional)                        
                            calcs.Add(match.Value);
                    }
                }
                calcsList.Add(calcs.ToArray());
            }
            return calcsList;
        }

        // Return the value of a mul(x,y) calc.
        static long processCalc(string calc)
        {
            var nums = calc[4..^1].Split(",");
            return int.Parse(nums[0]) * int.Parse(nums[1]);
        }
    }
}