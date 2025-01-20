using System;
using System.IO;
using System.Linq;

// Day 22: Monkey Market.
namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "Day22Input.txt";
            var secretNumbers = File.ReadAllLines(fileName).Select(sn => long.Parse(sn));

            // Part 1.
            var total = 0L;
            foreach (var secretNumber in secretNumbers)
            {
                var sn = new SecretNumber(secretNumber);
                sn.Evolve(2000);
                total += sn.Number;
            }
            Console.WriteLine($"Part 1 answer: {total}");

            // Part 2.
            Console.WriteLine("Part 2 answer: TODO");
        }
    }
}