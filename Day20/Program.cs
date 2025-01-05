using System;

namespace Day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputfile = "Day20InputExample.txt";
            var racetrack = new Racetrack(inputfile);                        

            // Part 1.
            var totalGreaterThanOrEqual100 = 0;
            System.Collections.Generic.SortedDictionary<int, int> cheatsSummary1 = racetrack.GetCheatsSummary(false);
            foreach (var kvp in cheatsSummary1)            
                if (kvp.Key >= 64)
                    totalGreaterThanOrEqual100 += kvp.Value;
            Console.WriteLine($"Part 1 answer: {totalGreaterThanOrEqual100}");

            // Part 2.
            totalGreaterThanOrEqual100 = 0;
            var cheatsSummary2 = racetrack.GetCheatsSummary(true);
            foreach (var kvp in cheatsSummary2)
                if (kvp.Key >= 50)
                    totalGreaterThanOrEqual100 += kvp.Value;
            Console.WriteLine($"Part 2 answer: TODO"); // {totalGreaterThanOrEqual100}");
        }
    }
}
