using System;
using System.IO;

// Day 8: Resonant Collinearity.
namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse antenna map.
            var mapLines = File.ReadAllLines("Day08Input.txt");
            var map = new AntennaMap(mapLines);

            Console.WriteLine($"Part 1 answer: {CountAntinodes(map, false)}");
            Console.WriteLine($"Part 2 answer: {CountAntinodes(map, true)}");
        }

        static int CountAntinodes(AntennaMap map, bool includeTNodes)
        {
            for (var r = 0; r < map.Grid.Length; r++)
                for (var c = 0; c < map.Grid[0].Length; c++)
                {
                    var symbol = map.Grid[r][c];
                    if (symbol != '#' && symbol != '.')
                    {
                        var antennaPairs = map.GetAntennaPairs((c, r));
                        foreach (var ap in antennaPairs)
                        {
                            var antinodes = map.GetAntinodes(ap.Item1, ap.Item2);                                                        
                            map.PlaceAntinodes(antinodes.Item1, antinodes.Item2);
                            if (includeTNodes)
                            {
                                var tnodes = map.GetTNodes(ap.Item1, ap.Item2);
                                foreach (var tn in tnodes)
                                    map.PlaceAntinodes(tn, tn);
                            }
                        }
                    }
                }
            return map.Antinodes.Count;
        }
    }
}