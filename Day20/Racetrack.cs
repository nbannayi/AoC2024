using System;
using System.Collections.Generic;
using System.IO;

namespace Day20
{
    public struct Cheat
    {
        public (int, int) Start;
        public (int, int) End;
        public int Saving;

        public Cheat((int, int) start, (int, int) end, int saving)
        {
            Start = start;
            End = end;
            Saving = saving;
        }
    }

    public class Racetrack
    {
        public char[,] Track { get; set; }
        private int[,] _trackWeights;
        private int _shortestDistance;
        public (int, int) Start, End;
        private int _noRows, _noCols;
        public List<(int,int)> Path { get; set; }        

        /// <summary>
        /// Represents a Racetrack for pico second racing.
        /// </summary>
        /// <param name="inputfile">Maze layout.</param>
        public Racetrack(string inputfile)
        {
            // Build track.
            var trackLines = File.ReadAllLines(inputfile);
            _noRows = trackLines.Length;
            _noCols = trackLines[0].Length;
            Track = new char[_noRows, _noCols];
            for (var row = 0; row < _noRows; row++)
            {
                for (var col = 0; col < _noCols; col++)
                {
                    var symbol = trackLines[row][col];
                    if (symbol == 'S') Start = (row, col);
                    if (symbol == 'E') End = (row, col);
                    Track[row, col] = trackLines[row][col];
                }
            }

            // Set up path and track weight for cheat determination.
            _trackWeights = new int[_noRows,_noCols];
            Path = new List<(int, int)>();
            SetupPathAndWeights();
        }

        /// <summary>
        /// Get all cheats available for given position.
        /// </summary>
        /// <param name="pos">Position to cheat from.</param>
        /// <returns>List of available cheats.</returns>
        public List<Cheat> GetCheats1((int, int) pos)
        {
            var cheats = new List<Cheat>();
            if (GetCheat(pos, (-1, 0), out Cheat? cheat) && cheat != null) cheats.Add(cheat.Value); // Up.
            if (GetCheat(pos, (1, 0), out cheat) && cheat != null) cheats.Add(cheat.Value);         // Down.
            if (GetCheat(pos, (0, -1), out cheat) && cheat != null) cheats.Add(cheat.Value);        // Left.
            if (GetCheat(pos, (0, 1), out cheat) && cheat != null) cheats.Add(cheat.Value);         // Right.
            return cheats;

            bool GetCheat((int,int) pos, (int, int) delta, out Cheat? cheat)
            {
                cheat = null;
                var (newRow1, newCol1) = (pos.Item1 + delta.Item1, pos.Item2 + delta.Item2);
                var (newRow2, newCol2) = (pos.Item1 + 2*delta.Item1, pos.Item2 + 2*delta.Item2);
                if (InBounds(newRow1, newCol1) && Track[newRow1,newCol1] == '#' &&
                    InBounds(newRow2, newCol2) && Track[newRow2, newCol2] != '#')
                {
                    var saving = _trackWeights[pos.Item1, pos.Item2] - _trackWeights[newRow2, newCol2] - 2;
                    cheat = new Cheat(pos, (newRow2, newCol2), saving);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get all cheats available for given position with a larger picosecond limit.
        /// </summary>
        /// <param name="pos">Position to cheat from.</param>
        /// <param name="maxPicosecs">Maximum picosecs for search.</param>
        /// <returns>List of available cheats.</returns>
        public List<Cheat> GetCheats((int, int) pos, int maxPicosecs)
        {
            // TODO.
            return new List<Cheat>();
        }

        /// <summary>
        /// Get summary of picosecond savings and their numbers.
        /// </summary>
        /// <param name="part2">Set to true for modified cheat search in pt 2.</param>
        /// <returns>List if all cheats.</returns>
        public SortedDictionary<int, int> GetCheatsSummary(bool part2)
        {
            var summary = new SortedDictionary<int, int>();
            foreach (var pos in Path)
            {
                var cheats = part2 ? GetCheats(pos, 20) : GetCheats1(pos);
                foreach (var cheat in cheats)
                {
                    if (summary.ContainsKey(cheat.Saving))
                        summary[cheat.Saving]++;
                    else
                        summary[cheat.Saving] = 1;
                }
            }
            return summary;
        }

        /// <summary>
        /// Display the race track.
        /// </summary>
        public void Display()
        {
            for (var row = 0; row < _noRows; row++)
            {
                for (var col = 0; col < _noRows; col++)
                    Console.Write(Track[row, col]);
                Console.WriteLine();
            }
        }

        // Check position is inside maze track.
        private bool InBounds(int row, int col)
        {
            return (row >= 0 && row < _noRows && col >= 0 && col < _noCols);
        }

        // Walk round from start to end and set track weights.
        private void SetupPathAndWeights()
        {
            var curPos = Start;                        
            var visited = new bool[_noRows, _noCols];

            // Get path through maze.            
            while (curPos != End)
            {
                var (row, col) = curPos;
                visited[row, col] = true;
                Path.Add(curPos);
                curPos = (row + 1, col);
                if (IsValidDirection(curPos, visited)) continue;
                curPos = (row - 1, col);
                if (IsValidDirection(curPos, visited)) continue;
                curPos = (row, col + 1);
                if (IsValidDirection(curPos, visited)) continue;
                curPos = (row, col - 1);
                if (IsValidDirection(curPos, visited)) continue;

                bool IsValidDirection((int row, int col) pos, bool[,] visited)
                {
                    return (InBounds(pos.row, pos.col) &&
                        !visited[pos.row, pos.col] &&
                        Track[pos.row, pos.col] != '#');
                }
            }
            Path.Add(End);
            _shortestDistance = Path.Count - 1;

            // Now update trackweights.
            var weight = _shortestDistance;

            // Init to max.
            for (var row = 0; row < _noRows; row++)
                for (var col = 0; col < _noCols; col++)
                    _trackWeights[row, col] = int.MaxValue;

            // Trace through and update weights.
            foreach (var pos in Path)
            {                
                var (row, col) = pos;
                _trackWeights[row, col] = weight;
                weight--;
            }
        }
    }
}