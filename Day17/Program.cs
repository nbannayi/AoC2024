using System;
using System.IO;

// Day 17: Chronospatial Computer.
namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up computer and load program.
            // Parse input data.
            var inputData = "Day17Input.txt";
            var (registers, program) = ParseInput(inputData);
            var (a, b, c) = (registers[0], registers[1], registers[2]);

            // Create and initialise computer.
            var computer = new ThreeBitComputer(a, b, c);
            computer.LoadProgram(program);

            // Part 1.            
            computer.Run(false);
            Console.WriteLine($"Part 1 answer: {computer.GetOutput()}");            

            // Part 2 - quine.
            // Target: 2,4,1,1,7,5,4,4,1,4,0,3,5,5,3,0
            Console.WriteLine($"Part 2 answer: TODO");
        }

        // Get all details to load the computer.
        static (int[], string) ParseInput(string filename)
        {
            var filelines = File.ReadAllLines(filename);
            var registers = new int[3];
            registers[0] = int.Parse(filelines[0].Split(' ')[2]);
            registers[1] = int.Parse(filelines[1].Split(' ')[2]);
            registers[2] = int.Parse(filelines[2].Split(' ')[2]);
            var program = filelines[4].Split(' ')[1];
            return (registers, program);
        }
    }
}