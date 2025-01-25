using System;
using System.Collections.Generic;
using System.IO;

// Day 25: Code Chronicle.
namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputfile = "Day25Input.txt";
            var (locks, keys) = ParseLocksAndKeys(inputfile);

            // Part 1.
            var noSuccessfulFits = 0;
            foreach (var keyLock in locks)
                foreach (var key in keys)
                    if (KeyFitsLock(key, keyLock)) noSuccessfulFits++;
            Console.WriteLine($"Part 1 answer: {noSuccessfulFits}");

            // Part 2.
            Console.WriteLine("Part 2 answer: there is no part 2 - hooray!");
        }

        // Check if a given key fits a given lock.
        static bool KeyFitsLock(int[] key, int[] keyLock)
        {
            bool fitsLock = true;
            for (var i = 0; i < 5; i++)
            {
                if (key[i] + keyLock[i] > 7)
                    return false;
            }
            return fitsLock;
        }

        // Parse all inputs into locks and keys.
        static (List<int[]> locks, List<int[]> keys) ParseLocksAndKeys(string inputfile)
        {
            var rawtext = File.ReadAllText(inputfile);
            var blocks = rawtext.Split("\n\n");
            var locks = new List<int[]>();
            var keys = new List<int[]>();
            foreach (var block in blocks)
            {
                var blockLines = block.Split("\n");
                var blockHeights = new int[5];
                for (var col = 0; col < blockLines[0].Length; col++)
                {
                    var colHeight = 0;
                    for (var row = 0; row < blockLines.Length; row++)
                        if (blockLines[row][col] == '#') colHeight++;
                    blockHeights[col] = colHeight;
                }
                if (blockLines[0][0] == '#')
                    locks.Add(blockHeights);
                else
                    keys.Add(blockHeights);
            }
            return (locks, keys);
        }
    }
}