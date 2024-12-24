using System;
using System.IO;

// Day 12: Garden Groups.
namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapFile = "Day12Input.txt";
            var mapLines = File.ReadAllLines(mapFile);
            var garden = new Garden(mapLines);

            Console.WriteLine($"Part 1 answer: {garden.GetTotalCost1()}");
            Console.WriteLine($"Part 2 answer: {garden.GetTotalCost2()}");            
        }
    }
}
