using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 23: LAN Party.
namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "Day23Input.txt";
            var connectionLines = File.ReadAllLines(fileName);

            // Create adjacency graph.
            var lanGraph = new Dictionary<string, List<string>>();
            foreach (var cl in connectionLines)
            {
                var tokens = cl.Split("-");
                var (sourceComputer, targetComputer) = (tokens[0], tokens[1]);
                if (!lanGraph.ContainsKey(sourceComputer))
                    lanGraph.Add(sourceComputer, new List<string>());
                lanGraph[sourceComputer].Add(targetComputer);
                if (!lanGraph.ContainsKey(targetComputer))
                    lanGraph.Add(targetComputer, new List<string>());
                lanGraph[targetComputer].Add(sourceComputer);
            }

            // Part 1.
            var triangles = FindTriangles(lanGraph);
            var historianCount = 0;
            foreach (var triangle in triangles)
            {
                var (a, b, c) = (triangle[0], triangle[1], triangle[2]);
                if (a.StartsWith('t') || b.StartsWith('t') || c.StartsWith('t'))
                    historianCount++;
            }
            Console.WriteLine($"Part 1 answer: {historianCount}");

            // Part 2.
            var maximalClique = FindMaximalCliques(lanGraph).ToList().First();
            maximalClique.Sort();
            var password = string.Join(',', maximalClique);
            Console.WriteLine($"Part 2 answer: {password}");
        }

        // Bron-Kerbosch algorithm for finding all maximal cliques.
        // (I can't lie - I had to research ths one, brute force returned no results!)
        public static List<List<string>> FindMaximalCliques(Dictionary<string, List<string>> graph)
        {
            var cliques = new List<List<string>>();
            var allNodes = graph.Keys.ToList();
            BronKerboschRecursive(new HashSet<string>(), allNodes.ToHashSet(), new HashSet<string>(), graph, cliques);

            var maxSize = cliques.Select(c => c.Count).Max();
            return cliques.Where(c => c.Count == maxSize).ToList();

            void BronKerboschRecursive(
                HashSet<string> R,
                HashSet<string> P,
                HashSet<string> X,
                Dictionary<string, List<string>> graph,
                List<List<string>> cliques)
            {
                if (P.Count == 0 && X.Count == 0)
                {
                    // Found a maximal clique.
                    cliques.Add(R.ToList());
                    return;
                }
                // Try each node in P.
                foreach (var node in P.ToList())
                {
                    var newR = new HashSet<string>(R) { node };
                    var newP = new HashSet<string>(P.Intersect(graph[node]));
                    var newX = new HashSet<string>(X.Intersect(graph[node]));
                    BronKerboschRecursive(newR, newP, newX, graph, cliques);

                    // Move node from P to X.
                    P.Remove(node);
                    X.Add(node);
                }
            }            
        }

        // Get sets of three connected nodes (i.e. triangles.)
        // (Yes it's dumb!)
        static List<List<string>> FindTriangles(Dictionary<string, List<string>> graph)
        {
            var triangles = new HashSet<string>(); // To avoid duplicates.
            var results = new List<List<string>>();
            foreach (var nodeA in graph.Keys)
            {
                var neighboursA = graph[nodeA];
                foreach (var nodeB in neighboursA)
                {
                    // Get neighbours of B, skip if same as A.
                    if (!graph.ContainsKey(nodeB) || nodeB == nodeA) continue;
                    var neighboursB = graph[nodeB];
                    foreach (var nodeC in neighboursB)
                    {
                        // Ensure C is not A or B and C connects back to A.
                        if (nodeC != nodeA && nodeC != nodeB && neighboursA.Contains(nodeC))
                        {
                            // Create a sorted triangle to avoid duplicates.
                            var triangle = new List<string> { nodeA, nodeB, nodeC };
                            triangle.Sort();
                            var triangleKey = string.Join(",", triangle);
                            if (!triangles.Contains(triangleKey))
                            {
                                triangles.Add(triangleKey);
                                results.Add(triangle);
                            }
                        }
                    }
                }
            }
            return results;
        }
    }
}