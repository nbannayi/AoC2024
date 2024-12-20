using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08
{
    public class AntennaMap
    {
        private char[][] _grid;
        public char[][] Grid { get { return _grid; } }

        private List<(int,int)> _antinodes;
        public List<(int, int)> Antinodes { get { return _antinodes; } }

        public AntennaMap(string[] mapLines)
        {
            _grid = mapLines.Select(m => m.ToCharArray()).ToArray();
            _antinodes = new List<(int, int)>();
        }

        /// <summary>
        /// Display the current map.
        /// </summary>
        public void Display()
        {
            for (var r = 0; r < _grid.Length; r++)
            {
                for (var c = 0; c < _grid[0].Length; c++)                
                    Console.Write(_grid[r][c]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Gets positions of antinodes based on passed antennas.
        /// </summary>
        /// <param name="antenna1">First antenna coord.</param>
        /// <param name="antenna2">Second antenna coord.</param>
        /// <returns>A tuple containing both antinode coords.</returns>
        public ((int,int),(int,int)) GetAntinodes((int,int) antenna1, (int,int) antenna2)
        {
            var x1 = antenna1.Item1;
            var x2 = antenna2.Item1;
            var y1 = antenna1.Item2;
            var y2 = antenna2.Item2;
            var dx = x2 - x1;
            var dy = y2 - y1;            
            (int, int) a1;
            (int, int) a2;
            if (dx > 0 && dy > 0)
            {
                a1 = (x1 - dx, y1 - dy);
                a2 = (x2 + dx, y2 + dy);
            }
            else if (dx < 0 && dy > 0)
            {
                a1 = (x1 - dx, y1 - dy);
                a2 = (x2 + dx, y2 + dy);
            }
            else if (dx > 0 && dy < 0)
            {
                a1 = (x1 - dx, y1 - dy);
                a2 = (x2 + dx, y2 + dy);
            }
            else if (dx < 0 && dy < 0)
            {
                a1 = (x2 + dx, y2 + dy);
                a2 = (x1 - dx, y1 - dy);
            }
            else if (dx == 0)
            {
                a1 = (dy > 0) ? (x1, y1 - dy) : (x2, y2 + dy);
                a2 = (dy > 0) ? (x2, y2 + dy) : (x2, y2 - dy);
            }
            else
            {
                a1 = (dx > 0) ? (x1 - dx, y1) : (x2 + dx, y2);
                a2 = (dx > 0) ? (x2 + dx, y2) : (x2 - dx, y2);
            }
            return (a1, a2);
        }

        /// <summary>
        /// Given two antennas get all TNodes in line with them.
        /// </summary>
        /// <param name="antenna1">First antenna coord.</param>
        /// <param name="antenna2">Second antenna coord.</param>
        /// <returns>List containing all TNode coords.</returns>
        public List<(int,int)> GetTNodes((int, int) antenna1, (int, int) antenna2)
        {
            var x1 = antenna1.Item1;
            var x2 = antenna2.Item1;
            var y1 = antenna1.Item2;
            var y2 = antenna2.Item2;

            var dx = x2 - x1;
            var dy = y2 - y1;
            var tnodes = new List<(int, int)>();
            int tnx, tny;
            if ((dx > 0 && dy > 0) || (dx < 0 && dy < 0))
            {
                while (xInBounds(x1) && yInBounds(y1))
                {
                    x1 -= Math.Abs(dx);
                    y1 -= Math.Abs(dy);
                }
                tnx = x1 + Math.Abs(dx); tny = y1 + Math.Abs(dy);
                while (xInBounds(tnx) && yInBounds(tny))
                {
                    tnodes.Add((tnx, tny));
                    tnx += Math.Abs(dx);
                    tny += Math.Abs(dy);                    
                }                
            }
            else if ((dx > 0 && dy < 0) || (dx < 0 && dy > 0))
            {
                while (xInBounds(x1) && yInBounds(y1))
                {
                    x1 += Math.Abs(dx);
                    y1 -= Math.Abs(dy);
                }
                tnx = x1 - Math.Abs(dx); tny = y1 + Math.Abs(dy);
                while (xInBounds(tnx) && yInBounds(tny))
                {
                    tnodes.Add((tnx, tny));
                    tnx -= Math.Abs(dx);
                    tny += Math.Abs(dy);                    
                }
            }
            else if (dx == 0)
            {                
                while (xInBounds(x1) && yInBounds(y1))
                {
                    y1 -= Math.Abs(dy);                    
                }
                tnx = x1; tny = y1 + Math.Abs(dy);
                while (xInBounds(tnx) && yInBounds(tny))
                {                                        
                    tnodes.Add((tnx, tny));
                    tny += Math.Abs(dy);
                }
            }
            else
            {                
                while (xInBounds(x1) && yInBounds(y1))
                {
                    x1 -= Math.Abs(dx);
                }
                tnx = x1 + Math.Abs(dx); tny = y1;
                while (xInBounds(tnx) && yInBounds(tny))
                {
                    tnodes.Add((tnx, tny));
                    tnx += Math.Abs(dx);             
                }
            }
            return tnodes;
        }

        private bool xInBounds(int x)
        {
            return (x >= 0 && x < _grid[0].Length);
        }

        private bool yInBounds(int y)
        {
            return (y >= 0 && y < _grid.Length);
        }

        /// <summary>
        /// Given two antinodes, place them on the grid.
        /// </summary>
        /// <param name="antinode1">1st antinode.</param>
        /// <param name="antinode2"><2nd equidistant antinode./param>
        public void PlaceAntinodes((int, int) antinode1, (int, int) antinode2)
        {
            var x1 = antinode1.Item1;
            var x2 = antinode2.Item1;
            var y1 = antinode1.Item2;
            var y2 = antinode2.Item2;
            var yb = _grid.Length;
            var xb = _grid[0].Length;
            // If out of bounds don't attempt to create antinodes.            
            if (!(x1 >= xb || x1 < 0 || y1 >= yb || y1 < 0))
            {
                if (!_antinodes.Contains(antinode1))
                {
                    if (_grid[y1][x1] == '.') _grid[y1][x1] = '#';
                    _antinodes.Add(antinode1);
                }
            }
            // Dont place same antinode twice.
            if (antinode1 == antinode2)
                return;
            if (!(x2 >= xb || x2 < 0 || y2 >= yb || y2 < 0))
            {
                if (!_antinodes.Contains(antinode2))
                {
                    if (_grid[y2][x2] == '.') _grid[y2][x2] = '#';
                    _antinodes.Add(antinode2);
                }                                
            }
        }

        /// <summary>
        /// Given a source antenna get list of all antenna pairs.
        /// </summary>
        /// <param name="sourceAntenna"></param>
        /// <returns>All antenna pairs for this frequency.</returns>
        public List<((int,int),(int,int))> GetAntennaPairs((int,int) sourceAntenna)
        {
            var x = sourceAntenna.Item1;
            var y = sourceAntenna.Item2;
            var symbol = _grid[y][x];
            var antennaPairs = new List<((int, int), (int, int))>();
            for (int r = 0; r < _grid.Length; r++)
                for (var c = 0; c < _grid[0].Length; c++)
                    if (_grid[r][c] == symbol && (c, r) != sourceAntenna)
                        antennaPairs.Add((sourceAntenna,(c,r)));                            
            return antennaPairs;
        }
    }
}