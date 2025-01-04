using System;
using System.Collections.Generic;
using System.IO;

// Day 18: RAM Run.
namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var computer = new NorthPoleComputer(71, 71);
            var bytes = GetBytePositions("Day18Input.txt");

            // Part 1.
            computer.DropNBytes(bytes, 1024);
            var shortestPath = computer.FindShortestPath();
            Console.WriteLine($"Part 1 answer: {shortestPath}");

            // Part 2 - note takes about 30-40 secs - bit slow.
            var blockingByte = computer.FindBlockingByte(bytes, 1025);
            Console.WriteLine($"Part 2 answer: {blockingByte}");
        }

        // Parse input and return all byte positions to drop.
        static List<(int,int)> GetBytePositions(string inputfile)
        {
            var byteLines = File.ReadAllLines(inputfile);
            var bytes = new List<(int, int)>();
            foreach (var byteLine in byteLines)
            {
                var byteTokens = byteLine.Split(",");
                var x = int.Parse(byteTokens[0]);
                var y = int.Parse(byteTokens[1]);
                bytes.Add((x, y));
            }
            return bytes;
        }
    }
}