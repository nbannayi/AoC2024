using System;
using System.Collections.Generic;
using System.Linq;

namespace Day16
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public class ReindeerMaze
    {
        public (int, int) StartPos { get; set; }
        public (int, int) EndPos { get; set; }
        private int _width, _height;
        public char[,] Map { get; set; }
        private int _bestScore;
        public int BestScore { get { return _bestScore; } }
        private Dictionary<(int, int), List<(int, int)>> _parents;
        public Dictionary<(int, int), int> Costs { get; set; }
        public SortedSet<(int, (int, int), Direction, List<(int,int)>)> Queue { get; set; }
        private static (int, int, Direction)[] _directions;

        /// <summary>
        /// Represents a new reindeer maze.
        /// </summary>
        /// <param name="mazeLines">Input maze map data.</param>
        public ReindeerMaze(string[] mazeLines)
        {
            _height = mazeLines.Length;
            _width = mazeLines[0].Length;
            _bestScore = 0;
            _parents = new Dictionary<(int, int), List<(int, int)>>();
            Costs = new Dictionary<(int, int), int>();

            // Direction vectors.
            _directions = new (int, int, Direction)[]
            {
                (-1,  0, Direction.North),
                ( 1,  0, Direction.South),
                ( 0, -1, Direction.West),
                ( 0,  1, Direction.East)
            };

            // Priority queue for Dijkstra.
            Queue = new SortedSet<(int, (int, int), Direction, List<(int,int)>)>(
                Comparer<(int, (int, int), Direction, List<(int,int)>)>.Create((a, b) =>
                {
                    if (a.Item1 != b.Item1)
                        return a.Item1.CompareTo(b.Item1);
                    if (a.Item2.Item1 != b.Item2.Item1)
                        return a.Item2.Item1.CompareTo(b.Item2.Item1);
                    if (a.Item2.Item2 != b.Item2.Item2)
                        return a.Item2.Item2.CompareTo(b.Item2.Item2);
                    return 0;
                }));

            Map = new char[_height, _width];
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                    Map[row, col] = mazeLines[row][col];
            }

            // Starts in bottom left corner.
            StartPos = (_height - 2, 1);

            // End is upper right corner.
            EndPos = (1, _width - 2);
        }

        /// <summary>
        /// Display the current map.
        /// </summary>
        public void Display(List<(int, int)> path)
        {
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                {
                    var symbol = Map[row, col];
                    if ((row, col) == StartPos) symbol = 'S';
                    if ((row, col) == EndPos) symbol = 'E';
                    if (path != null && path.Contains((row, col))) symbol = '*';
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Get Best score and store in maze and track all shortest paths.
        /// </summary>
        public int GetBestScore()
        {      
            // Add the starting position.
            Queue.Add((0, StartPos, Direction.East, null));
            Costs[StartPos] = 0;

            // Track the shortest path cost to the destination.
            int shortestPathCost = int.MaxValue;

            while (Queue.Count > 0)
            {
                var (cost, curPos, direction, _) = Queue.Min;
                Queue.Remove(Queue.Min);

                // If the cost exceeds the known shortest path cost, skip.
                if (cost > shortestPathCost)
                    continue;

                // If we reach the destination, update the shortest path cost.
                if (curPos == EndPos)
                {
                    shortestPathCost = Math.Min(shortestPathCost, cost);
                    if (_bestScore == 0) _bestScore = shortestPathCost;
                }

                // Explore neighbours.
                foreach (var (dy, dx, newDir) in _directions)
                {
                    var newPos = (curPos.Item1 + dy, curPos.Item2 + dx);

                    // Skip walls or invalid positions.
                    if (Map[newPos.Item1, newPos.Item2] == '#')
                        continue;

                    // Calculate the cost to move to the new position.
                    int newCost = cost + 1; // Base cost of moving.
                    if (direction != newDir) // Add turn penalty if changing direction.
                        newCost += 1000;

                    // If this path has a better cost, update costs and parents.
                    if (!Costs.ContainsKey(newPos) || newCost < Costs[newPos])
                    {
                        Costs[newPos] = newCost;
                        Queue.Add((newCost, newPos, newDir, null));
                    }
                }
            }
            return BestScore;
        }
   }
}