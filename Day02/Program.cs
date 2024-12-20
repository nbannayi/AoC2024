using System;
using System.Collections.Generic;
using System.IO;

// Day 2: Red-Nosed Reports.
namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse data into a list of arrays containing the sequences.
            var reportLines = File.ReadAllLines("Day02Input.txt");
            var reportList = new List<int[]>(); 
            foreach (var reportLine in reportLines)
            {
                var report = reportLine.Split(" ");
                var reportNums = new List<int>();
                foreach (var id in report)                
                    reportNums.Add(int.Parse(id));
                reportList.Add(reportNums.ToArray());
            }

            // Work out report safety.
            var totalSafe = 0;
            var totalSafeWithDampener = 0;
            foreach (var report in reportList)
            {
                if (IsSafe(report, false)) totalSafe++;
                if (IsSafe(report, true)) totalSafeWithDampener++;
            }

            Console.WriteLine($"Part 1 answer: {totalSafe}");
            Console.WriteLine($"Part 2 answer: {totalSafeWithDampener}");
        }

        // Determine if a sequence is safe or not.
        static bool IsSafe(int[] report, bool applyDampener)
        {
            var first = report[0];
            var second = report[1];
            var increasing = second > first;            
            for (int i = 0; i < report.Length-1; i++)
            {
                var stillIncreasing = report[i + 1] > report[i];
                var delta = Math.Abs(report[i + 1] - report[i]);
                var safetyFlag = true;
                if (stillIncreasing != increasing)
                    safetyFlag = false;
                if (!(delta >= 1 && delta <= 3))
                    safetyFlag = false;
                if (!safetyFlag)
                {
                    // Try all combos till we find one, brute force but it works.
                    if (applyDampener)
                    {
                        for (int j = 0; j < report.Length; j++)
                        {
                            var modifiedReport = new List<int>(report);
                            modifiedReport.RemoveAt(j);
                            if (IsSafe(modifiedReport.ToArray(), false)) return true;
                        }
                    }
                    return false;
                }
            }
            // If we get this far it's safe.
            return true;
        }
    }
}