using System;
using System.IO;

// Day 9: Disk Fragmenter.
namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            var diskContents = File.ReadAllText("Day09Input.txt");
            var disk1 = new Disk(diskContents);
            var disk2 = new Disk(diskContents);

            // Comapct it all.
            disk1.Compact1();
            disk2.Compact2();

            // Part 1.
            var checksum1 = disk1.GetChecksum();
            Console.WriteLine($"Part 1 answer: {checksum1}");

            // Part 2.
            var checksum2 = disk2.GetChecksum();
            Console.WriteLine($"Part 2 answer: {checksum2}");
        }
    }   
}