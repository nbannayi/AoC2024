using System;
using System.IO;

// Day 14: Restroom Redoubt.
namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "Day14Input.txt";
            var robotLines = File.ReadAllLines(fileName);
            var bathroom = new Bathroom(robotLines, 103, 101);

            // Part 1.
            for (int n1 = 1; n1 <= 100; n1++)            
                bathroom.MoveRobots();
            Console.WriteLine($"Part 1 answer: {bathroom.GetSafetyFactor()}");

            // Part 2.
            var n2 = 100;            
            while (!bathroom.FoundEasterEgg())
            {
                bathroom.MoveRobots();
                n2++;
            }
            Console.WriteLine($"Part 2 answer: {n2}");
            // Uncomment to see the tree! vv
            //bathroom.Display();
        }
    }
}