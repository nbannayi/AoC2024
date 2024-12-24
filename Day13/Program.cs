using System;
using System.Collections.Generic;
using System.IO;

// Day 13: Claw Contraption.
namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "Day13Input.txt";
            var clawMachines = GetClawMachines(fileName);

            Console.WriteLine($"Part 1 answer: {GetTotalCost(clawMachines, 0L)}");
            Console.WriteLine($"Part 2 answer: {GetTotalCost(clawMachines, 10000000000000L)}");
        }

        // Parse all claw machines into a list.
        static List<ClawMachine> GetClawMachines(string fileName)
        {
            var fileText = File.ReadAllText(fileName);
            var clawLines = fileText.Split("\n\n");
            var clawMachines = new List<ClawMachine>();
            foreach (var clawLine in clawLines)
            {
                var clawSettings = clawLine.Split("\n");
                var buttonATokens = clawSettings[0].Split(" ");
                var ax = int.Parse(buttonATokens[2].Replace("X+", "").Replace(",", ""));
                var ay = int.Parse(buttonATokens[3].Replace("Y+", ""));
                var buttonBTokens = clawSettings[1].Split(" ");
                var bx = int.Parse(buttonBTokens[2].Replace("X+", "").Replace(",", ""));
                var by = int.Parse(buttonBTokens[3].Replace("Y+", ""));
                var prizeTokens = clawSettings[2].Split(" ");
                var px = long.Parse(prizeTokens[1].Replace("X=", "").Replace(",", ""));
                var py = long.Parse(prizeTokens[2].Replace("Y=", ""));
                var clawMachine = new ClawMachine((ax, ay), (bx, by), (px, py));
                clawMachines.Add(clawMachine);
            }
            return clawMachines;
        }

        static double GetTotalCost(List<ClawMachine> clawMachines, long offset)
        {
            ulong totalCost = 0;
            foreach (var clawMachine in clawMachines)
            {
                var (a, b, cost) = clawMachine.WinPrize(offset);
                if (a >= 0) totalCost += (ulong)cost;
            }
            return totalCost;
        }
    }
}