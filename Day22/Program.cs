using System;
using System.Collections.Generic;
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
            var numbers = File.ReadAllLines(fileName).Select(sn => long.Parse(sn)).ToList();
            var secretNumbers = new List<SecretNumber>();            

            // Part 1.            
            var total = 0L;
            foreach (var n in numbers)
            {
                var sn = new SecretNumber(n);
                sn.Evolve(2000);
                total += sn.Number;
                secretNumbers.Add(sn);
            }
            Console.WriteLine($"Part 1 answer: {total}");

            // Part 2 - did first three iterations which was enough to cover all needed sequences.
            // Note: this part takes ages to run (about 6 mins) - but can't be bothered to try to
            // optimise further. A star is a star :P
            var cache = new Dictionary<(int, int, int, int), long>();
            var maxPrice = 0L;            
            for (int i = 0; i <= 2; i++)
            {
                var sn1 = secretNumbers[i];                
                foreach (var dsp in sn1.DiffSeqPrices)
                {
                    if (!cache.ContainsKey(dsp.Item1))
                    {
                        total = 0L;
                        foreach (var sn2 in secretNumbers)
                            total += sn2.GetDiffSeqPrice(dsp.Item1);
                        cache[dsp.Item1] = total;
                    }
                    var price = cache[dsp.Item1];
                    if (price > maxPrice) maxPrice = price;
                }
            }
            Console.WriteLine($"Part 2 answer: {maxPrice}");
        }
    }
}