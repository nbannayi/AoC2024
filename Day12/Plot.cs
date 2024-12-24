using System.Collections.Generic;

namespace Day12
{
    /// <summary>
    /// Represents a garden plot.
    /// </summary>
    public class Plot
    {
        public bool Visited { get; set; }
        public char Label { get; set; }
        public List<(int, int)> Neighbours { get; set; }
        public (int, int) Coords { get; set; }

        /// <summary>
        /// Create a new garden plot.
        /// </summary>
        /// <param name="label">Label of the plot.</param>
        public Plot(char label, (int,int) coord)
        {
            Label = label;
            Visited = false;
            Coords = coord;
            Neighbours = new List<(int,int)>();
        }

        /// <summary>
        /// Return number of neighbours.
        /// </summary>
        /// <returns>Number of neighbours.</returns>
        public int GetNoNeighbours()
        {
            return Neighbours.Count;
        }

        /// <summary>
        /// Represent as string.
        /// </summary>
        /// <returns>String representation of a plot.</returns>
        public override string ToString()
        {
            return $"{Coords}: {Label}, visited: {Visited}";
        }
    }
}