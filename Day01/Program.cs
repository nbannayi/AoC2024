using System;
using System.Collections.Generic;
using System.IO;

// Day 1: Historian Hysteria
namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse data into two sorted id lists.
            var idLines = File.ReadAllLines("Day01Input.txt");
            var idList1 = new List<int>();
            var idList2 = new List<int>();
            foreach (var idLine in idLines)
            {
                var ids = idLine.Split("   ");
                idList1.Add(int.Parse(ids[0]));
                idList2.Add(int.Parse(ids[1]));
            }
            idList1.Sort();
            idList2.Sort();

            // Get total distances and similarities.
            var totalDistance = 0;
            var totalSimilarity = 0;
            for (var i = 0; i < idList1.Count; i++)
            {
                totalDistance += Math.Abs(idList1[i] - idList2[i]);
                totalSimilarity += idList1[i] * getSimilarity(idList1[i], idList2);
            }

            Console.WriteLine($"Part 1 answer: {totalDistance}");
            Console.WriteLine($"Part 2 answer: {totalSimilarity}");
        }

        // Get number of times n appears in passed list.
        static int getSimilarity(int n, List<int> idList)
        {
            var noOccurrences = 0;
            foreach (var id in idList)           
                if (id == n) noOccurrences++;
            return noOccurrences;
        }
    }
}
