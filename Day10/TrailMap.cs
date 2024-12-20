using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    public class TrailMap
    {
        public int[][] Map { get; set; }
        private bool[,] _visited;
        private List<(int, int)> _trail;

        public TrailMap(string[] trailMapLines)
        {
            Map = trailMapLines.
                Select(m => m.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).
                ToArray();
            _visited = new bool[Map.Length, Map[0].Length];
        }

        /// <summary>
        /// Get all trail head.
        /// </summary>
        /// <returns>List of coordinates of trail heads.</returns>
        public List<(int, int)> GetTrailHeads()
        {
            var trailHeads = new List<(int, int)>();
            for (var r = 0; r < Map.Length; r++)
                for (var c = 0; c < Map[0].Length; c++)
                    if (Map[r][c] == 0) trailHeads.Add((r, c));
            return trailHeads;
        }

        /// <summary>
        /// Get score of trail head.
        /// </summary>
        /// <param name="trailHead">Trail head to check.</param>
        /// <param name="allPaths">Set true for all paths rather than just all 9s.</param>
        /// <returns>Score or rating.</returns>
        public int GetTrailHeadScore((int, int) trailHead, bool allPaths)
        {
            for (var r = 0; r < Map.Length; r++)
                for (var c = 0; c < Map[0].Length; c++)
                    _visited[r, c] = false;
            _trail = new List<(int, int)>();
            Hike(trailHead, allPaths);
            return _trail.Count();
        }

        private bool ValidMove((int,int) curPosition, (int,int) newPosition)
        {
            var (r1, c1) = (curPosition.Item1, curPosition.Item2);
            var (r2, c2) = (newPosition.Item1, newPosition.Item2);            
            var inBounds = (r2 >= 0 && r2 < Map.Length && c2 >= 0 && c2 < Map[0].Length);
            if (inBounds)
            {
                var curHeight = Map[r1][c1];
                var newHeight = Map[r2][c2];
                return (newHeight - curHeight) == 1;
            }
            return false;
        }

        private void Hike((int, int) position, bool allPaths)
        {
            var (r, c) = position;
            if (_visited[r,c]) return;
            if (Map[r][c] == 9)
            {
                if (allPaths)
                    _trail.Add((r, c));
                else if (!_trail.Contains((r, c)))
                    _trail.Add((r, c));
            }
            _visited[r, c] = true;
            if (ValidMove(position, (r - 1, c))) Hike((r - 1, c), allPaths);
            if (ValidMove(position, (r + 1, c))) Hike((r + 1, c), allPaths);
            if (ValidMove(position, (r, c + 1))) Hike((r, c + 1), allPaths);
            if (ValidMove(position, (r, c - 1))) Hike((r, c - 1), allPaths);
            _visited[r, c] = false;
        }
    }
}