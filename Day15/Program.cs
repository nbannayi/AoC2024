using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

// Day 15: Warehouse Woes.
namespace Day15
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Parse input data.
            var inputFile = "Day15Input.txt";            
            var (mapLines, directions) = ParseInput(inputFile);

            // Part 1.
            
            var warehouse1 = new Warehouse(mapLines, false);
            foreach (Direction dir in directions)
            {
                warehouse1.MoveRobot(dir);
                // Uncomment to see movements vv
                //Thread.Sleep(50);
                //Console.SetCursorPosition(0,0);
                //warehouse1.Display();
            }            
            Console.WriteLine($"Part 1 answer: {warehouse1.GetTotalGPS()}");            

            // Part 2.            
            var warehouse2 = new Warehouse(mapLines, true);                        
            foreach (Direction dir in directions)
            {
                warehouse2.MoveRobot(dir);
                // Uncomment to see movements vv
                //Thread.Sleep(50);
                //Console.SetCursorPosition(0,0);
                //warehouse2.Display();
            }
            Console.WriteLine($"Part 2 answer: {warehouse2.GetTotalGPS()}");
        }

        // Parse all raw text input.
        static (string[], List<Direction>) ParseInput(string inputFile)
        {
            var inputText = File.ReadAllText(inputFile);
            var inputTextTokens = inputText.Split("\n\n");
            var mapLines = inputTextTokens[0].Split("\n");
            var robotDirections = new List<Direction>();
            foreach (var d  in inputTextTokens[1])
            {
                Direction direction;
                switch (d)
                {
                    case '^':
                        direction = Direction.Up;
                        break;
                    case '>':
                        direction = Direction.Right;
                        break;
                    case 'v':
                        direction = Direction.Down;
                        break;
                    case '<':
                        direction = Direction.Left;
                        break;
                    default:
                        continue;
                }
                robotDirections.Add(direction);
            }
            return (mapLines, robotDirections);
        }
    }
}