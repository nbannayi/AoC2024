using System;
using System.IO;
using System.Linq;

// Day 16: Reindeer Maze.
namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var mazeLines = File.ReadAllLines("Day16Input.txt");
            var maze = new ReindeerMaze(mazeLines);

            // Part 1.
            var bestScore = maze.GetBestScore();            
            Console.WriteLine($"Part 1 answer: {bestScore}");

            // Part 2.
            var bestPaths = maze.GetBestPaths();
            var noUniqueSeats = bestPaths.SelectMany(p => p).Distinct().Count();
            Console.WriteLine($"Part 2 answer: {noUniqueSeats}");
        }
    }
}