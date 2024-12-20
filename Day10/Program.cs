using System;
using System.IO;

// Day 10: Hoof It.
namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var trailMap = new TrailMap(File.ReadAllLines("Day10Input.txt"));
            var trailHeads = trailMap.GetTrailHeads();
            var totalScore = 0;
            var totalRating = 0;

            // Part 1.
            foreach (var trailHead in trailHeads)                            
                totalScore += trailMap.GetTrailHeadScore(trailHead, false);

            // Part 2.
            foreach (var trailHead in trailHeads)
                totalRating += trailMap.GetTrailHeadScore(trailHead, true);

            Console.WriteLine($"Part 1 answer: {totalScore}");
            Console.WriteLine($"Part 2 answer: {totalRating}");
        }
    }
}
