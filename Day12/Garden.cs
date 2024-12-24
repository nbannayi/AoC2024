using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    /// <summary>
    /// Represents a graden containing a grid of plots.
    /// </summary>
    public class Garden
    {
        public Plot[,] Map { get; set; }
        public List<Region> Regions { get; set; }
        public int NoRows { get; set; }
        public int NoCols { get; set; }

        /// <summary>
        /// Create a new garden.
        /// </summary>
        /// <param name="mapLines">Array grid of plots.</param>
        public Garden(string[] mapLines)
        {
            Map = new Plot[mapLines.Length, mapLines[0].Length];
            (NoRows, NoCols) = (mapLines.Length, mapLines[0].Length);
            for (var row = 0; row < mapLines.Length; row++)
            {
                for (var col = 0; col < mapLines[0].Length; col++)
                {
                    var label = mapLines[row][col];
                    Map[row, col] = new Plot(label, (row, col));
                }
            }
            Regions = GetRegions();
        }

        /// <summary>
        /// Get total cost of all regions fences (part 1.)
        /// </summary>
        /// <returns>Total cost.</returns>
        public int GetTotalCost1()
        {
            return Regions.Sum(r => r.GetCost1());
        }

        /// <summary>
        /// Get total cost of all regions fences (part 2.)
        /// </summary>
        /// <returns>Total cost.</returns>
        public int GetTotalCost2()
        {
            return Regions.Sum(r => r.GetCost2());
        }

        private List<Region> GetRegions()
        {
            var outputRegions = new List<Region>();
            var coord = GetNextUnvisitedRegion();
            while (coord.Item1 >= 0)
            {
                var region = new Region();
                ExploreRegion(coord, region);
                outputRegions.Add(region);
                coord = GetNextUnvisitedRegion();
            }
            return outputRegions;
        }

        private (int,int) GetNextUnvisitedRegion()
        {
            // Get next unvisited region.
            for (int r = 0; r < NoRows; r++)            
                for (int c = 0; c < NoCols; c++)                
                    if (!Map[r, c].Visited) return (r, c);
            // If none found.
            return (-1, -1);
        }

        private bool IsInRegion((int,int) coord, char label, bool ignoreVisited)
        {
            var (row, col) = coord;
            if (row < 0 || col < 0 || row > NoRows - 1 || col > NoCols - 1)
                return false;
            else
            {
                var plot = Map[row, col];
                return plot.Label == label && (!plot.Visited || ignoreVisited);
            }
        }

        private void AddNeighbours((int,int) coord, Plot plot)
        {
            var (row, col) = coord;
            if (IsInRegion((row + 1, col), plot.Label, true)) plot.Neighbours.Add((row + 1, col));
            if (IsInRegion((row - 1, col), plot.Label, true)) plot.Neighbours.Add((row - 1, col));
            if (IsInRegion((row, col + 1), plot.Label, true)) plot.Neighbours.Add((row, col + 1));
            if (IsInRegion((row, col - 1), plot.Label, true)) plot.Neighbours.Add((row, col - 1));
        }

        private void ExploreRegion((int,int) coord, Region region)
        {
            var (row, col) = coord;
            var plot = Map[row, col];
            plot.Visited = true;
            AddNeighbours(coord, plot);
            if (region.Label == ' ') region.Label = plot.Label;
            region.Plots.Add(plot);
            if (IsInRegion((row + 1, col), plot.Label, false)) ExploreRegion((row + 1, col), region);
            if (IsInRegion((row - 1, col), plot.Label, false)) ExploreRegion((row - 1, col), region);
            if (IsInRegion((row, col + 1), plot.Label, false)) ExploreRegion((row, col + 1), region);
            if (IsInRegion((row, col - 1), plot.Label, false)) ExploreRegion((row, col - 1), region);
        }
    }
}