using System;
using System.IO;

// Day 6: Guard Gallivant.
namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(File.ReadAllLines("Day06Input.txt"));
            // Part 1 - just move and get path.
            map.MoveGuard();
            Console.WriteLine($"Part 1 answer: {map.GetNoVisited()}");

            // Part 2 - find obstructions that make the guard get stuck.
            var totalInfiniteLoops = 0;
            for (var y = 0; y < map.Grid.Length; y++)
                for (var x = 0; x < map.Grid[0].Length; x++)
                {
                    map.Init();
                    map.PlaceObstacle(x, y);
                    map.MoveGuard();
                    if (map.GuardEndState == EndState.Stuck) totalInfiniteLoops++;              
                }            
            Console.WriteLine($"Part 2 answer: {totalInfiniteLoops}");
        }
    }
}
