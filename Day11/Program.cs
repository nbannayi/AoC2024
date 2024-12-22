using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 11: Plutonian Pebbles.
namespace Day11
{
    public class Program
    {
        static Dictionary<long, List<long>> _cache = new Dictionary<long, List<long>>();

        static void Main(string[] args)
        {
            var fileName = "Day11Input.txt";

            // Part 1.
            var stones1 = GetStones(fileName);
            var count1 = Blink1(stones1, 25);
            Console.WriteLine($"Part 1 answer: {count1}");

            // Part 2.
            var stones2 = GetStones(fileName);
            var count2 = Blink2(stones2, 75);
            Console.WriteLine($"Part 2 answer: {count2}");
        }

        static Queue<long> GetStones(string fileName)
        {
            return new Queue<long>(
                File.ReadAllText(fileName).Split(" ").
                Select(s => long.Parse(s)));
        }

        // Initial implementation - much too slow and inefficient for part 2.
        // (Believe me I tried - it ran overnight and eventually crashed!)
        static int Blink1(Queue<long> stones, int n)
        {            
            for (var i = 1; i <= n; i++)
            {
                var outputQueue = new Queue<long>();
                while (stones.Count() > 0)
                {
                    var stone = stones.Dequeue();
                    GetSplitStones(stone).ForEach(s => outputQueue.Enqueue(s));
                }
                stones = outputQueue;
            }
            return stones.Count();
        }

        // More optimal implementation for part 2.
        static long Blink2(Queue<long> stones, int n)
        {
            Dictionary<long, long> stonesDict = new Dictionary<long, long>();
            Dictionary<long, long> stonesDictTemp;

            // Transfer contents of stones to dictionary.
            foreach (var stone in stones)
                stonesDict.Add(stone, 1);

            for (var i = 1; i <= n; i++)
            {
                // Get all splits.
                stonesDictTemp = new Dictionary<long, long>();

                // Now aggregate.
                foreach (var kvp in stonesDict)
                {
                    var splitStones = GetSplitStones(kvp.Key);
                    foreach (var splitStone in splitStones)
                    {
                        if (stonesDictTemp.ContainsKey(splitStone))
                            stonesDictTemp[splitStone] += kvp.Value;
                        else
                            stonesDictTemp[splitStone] = kvp.Value;
                    }
                }
                stonesDict = stonesDictTemp;             
            }

            // Sum of all values in aggregation is total count.
            return stonesDict.Sum(s => s.Value);
        }

        // Pebble splitting logic.
        static List<long> GetSplitStones(long stone)
        {
            List<long> splitStones = new List<long>();
            if (_cache.ContainsKey(stone))
                splitStones = _cache[stone];
            else
            {
                if (stone == 0)
                    splitStones.Add(1);
                else
                {
                    var stoneStr = stone.ToString();
                    var stoneStrLen = stoneStr.Length;
                    if (stoneStrLen > 1 && stoneStrLen % 2 == 0)
                    {
                        var splitPoint = stoneStrLen / 2;
                        var stoneSplit1 = long.Parse(stoneStr.Substring(0, splitPoint));
                        var stoneSplit2 = long.Parse(stoneStr.Substring(splitPoint, splitPoint));
                        splitStones.Add(stoneSplit1);
                        splitStones.Add(stoneSplit2);
                    }
                    else
                        splitStones.Add(stone * 2024L);
                }
                _cache[stone] = splitStones;
            }
            return splitStones;
        }
    }
}