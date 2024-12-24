using System;
using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    /// <summary>
    /// Represents a region within a garden map.
    /// </summary>
    public class Region
    {
        public List<Plot> Plots { get; set; }
        public char Label { get; set; }

        /// <summary>
        /// Create a new region.
        /// </summary>
        public Region()
        {
            Label = ' ';
            Plots = new List<Plot>();
        }

        /// <summary>
        /// Get area of region.
        /// </summary>
        /// <returns>Area of region.</returns>
        public int GetArea()
        {
            return Plots.Count;
        }

        /// <summary>
        /// Get perimeter of region.
        /// </summary>
        /// <returns>Perimeter of region.</returns>
        public int GetPerimeter()
        {
            var totalNoNeighbours = Plots.Sum(p => p.GetNoNeighbours());
            return GetArea() * 4 - totalNoNeighbours; 
        }

        /// <summary>
        /// Get total cost of region (part 1.)
        /// </summary>
        /// <returns>Cost of region.</returns>
        public int GetCost1()
        {
            return GetArea() * GetPerimeter();
        }

        /// <summary>
        /// Get total cost of region (part 2.)
        /// </summary>
        /// <returns>Cost of region.</returns>
        public int GetCost2()
        {
            return GetArea() * GetNoSides();
        }

        /// <summary>
        /// Get all plots (including any inner ones.
        /// </summary>
        /// <returns>All plots (including inners ones.</returns>
        public List<Region> GetAllRegions()
        {
            // Get dimensions of enclosing rectangle.
            var minRow = Plots.Min(p => p.Coords.Item1);
            var maxRow = Plots.Max(p => p.Coords.Item1);
            var minCol = Plots.Min(p => p.Coords.Item2);
            var maxCol = Plots.Max(p => p.Coords.Item2);
            var rows = maxRow - minRow + 3;
            var cols = maxCol - minCol + 3;

            // Create 2D array containing region enclosed in rectangle.
            char[,] regionMap = new char[rows, cols];
            for (var r = 0; r < rows; r++)            
                for (var c = 0; c < cols; c++)
                    regionMap[r,c] = '.';                
            foreach (var plot in Plots)
            {
                var r = plot.Coords.Item1 - minRow + 1;
                var c = plot.Coords.Item2 - minCol + 1;
                regionMap[r, c] = 'x';
            }

            // Put into a garden.
            List<string> mapLines = new List<string>();
            for (var r = 0; r < rows; r++)
            {
                string mapLine = "";
                for (var c = 0; c < cols; c++)
                    mapLine += regionMap[r, c].ToString();
                mapLines.Add(mapLine);
            }
            var tempGarden = new Garden(mapLines.ToArray());

            // Finally remove outer region and assign to contained plots.
            var outputRegions = new List<Region>();
            foreach (Region r in tempGarden.Regions)
            {
                if (!r.Plots.Any(p => p.Coords == (0, 0)))
                    outputRegions.Add(new Region { Plots = r.Plots, Label = 'x' });
            }

            return outputRegions;
        }

        /// <summary>
        /// Display the region (for debugging purposes.)
        /// </summary>
        public void Display()
        {
            // Get dimensions of enclosing rectangle.
            var minRow = Plots.Min(p => p.Coords.Item1);
            var maxRow = Plots.Max(p => p.Coords.Item1);
            var minCol = Plots.Min(p => p.Coords.Item2);
            var maxCol = Plots.Max(p => p.Coords.Item2);
            var rows = maxRow - minRow + 1;
            var cols = maxCol - minCol + 1;

            // Create 2D array containing region enclosed in rectangle.
            char[,] regionMap = new char[rows, cols];
            for (var r = 0; r < rows; r++)
                for (var c = 0; c < cols; c++)
                    regionMap[r, c] = '.';
            foreach (var plot in Plots)
            {
                var r = plot.Coords.Item1 - minRow;
                var c = plot.Coords.Item2 - minCol;
                regionMap[r, c] = plot.Label;
            }

            // Finally, display it.
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                    Console.Write(regionMap[r, c]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Get Number of sides (innr and outer) or a region.
        /// </summary>
        /// <returns>Total no. of sides.</returns>
        public int GetNoSides()
        {
            var regions = GetAllRegions();
            var totalSides = 0;
            foreach (Region region in regions)            
                totalSides += region.WalkAroundRegion();            
            return totalSides;
        }

        private int WalkAroundRegion()
        {
            // Get dimensions of enclosing rectangle.
            var minRow = Plots.Min(p => p.Coords.Item1);
            var maxRow = Plots.Max(p => p.Coords.Item1);
            var minCol = Plots.Min(p => p.Coords.Item2);
            var maxCol = Plots.Max(p => p.Coords.Item2);
            var rows = maxRow - minRow + 3;
            var cols = maxCol - minCol + 3;

            // Create 2D array containing region enclosed in rectangle.
            char[,] regionMap = new char[rows, cols];
            for (var r = 0; r < rows; r++)
                for (var c = 0; c < cols; c++)
                    regionMap[r, c] = '.';
            foreach (var plot in Plots)
            {
                var r = plot.Coords.Item1 - minRow + 1;
                var c = plot.Coords.Item2 - minCol + 1;
                regionMap[r, c] = plot.Label != '.' ? plot.Label : 'x';
            }

            // Find start.
            var rowPointer = 0;
            var colPointer = 0;            
            for (var c = 0; c <= maxCol; c++)
            {
                if (regionMap[1,c] != '.')
                {
                    colPointer = c;
                    break;
                }
            }
            var pointer = new Pointer(rowPointer, colPointer, Direction.East);
            var attemptedAdvances = new List<(int, int)>();
            var turns = new List<(int, int, Direction)>();
            turns.Add((rowPointer, colPointer, Direction.East));

            // Walk around till we've done a complete circuit.
            regionMap[rowPointer, colPointer] = 'S';
            while ((pointer.Row, pointer.Col) != (rowPointer, colPointer-1))
            {
                var (row, col) = pointer.PeekRight();
                if (regionMap[row, col] != '.')
                {
                    var (row2, col2) = pointer.PeekAdvance();                    
                    if (regionMap[row2, col2] == '.' || regionMap[row2, col2] == '*')
                        pointer.Advance();
                    else
                    {
                        // Turning out of a dead-end! v
                        if (regionMap[row, col] == '*')
                            pointer.TurnClockwise();
                        // This caught me out mightily :/
                        else
                            pointer.TurnAntiClockwise();
                        turns.Add((pointer.Row, pointer.Col, pointer.Direction));                        
                    }
                }
                else
                {
                    pointer.TurnClockwise();
                    turns.Add((pointer.Row, pointer.Col, pointer.Direction));
                }
                regionMap[pointer.Row, pointer.Col] = '*';
            }

            // Display it (debug).
            //DisplayRegionMap(regionMap, rows, cols);

            // Finally number of turns will be equal to number of edges.
            var noTurns = turns.Distinct().Count();            
                       
            return noTurns;            
        }

        private void DisplayRegionMap(char[,] map, int rows, int cols)
        {
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                    Console.Write(map[r, c]);
                Console.WriteLine();
            }
        }
    }
}