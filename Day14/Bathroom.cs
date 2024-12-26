using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    public class Bathroom
    {
        public List<BathroomRobot> Robots { get; set; }
        public int SecondsElapsed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private int[,] _map;
        public int[,] Map { get { return _map; } }
        private List<(int, int)> _xmasTreeCoords;

        /// <summary>
        /// Represents an bathroom with a bunch of robots moving about in it.
        /// </summary>
        /// <param name="robotLines">Raw robot data.</param>
        /// <param name="height">Height of bathroom area.</param>
        /// <param name="width">Width of bathroom area.</param>
        public Bathroom(string[] robotLines, int height, int width)
        {
            SecondsElapsed = 0;
            Height = height;
            Width = width;
            Robots = new List<BathroomRobot>();

            // Parse robot details.
            foreach (var robotLine in robotLines)
            {
                var robotTokens = robotLine.Split(" ");
                var robotPosTokens = robotTokens[0].Split(",");
                var px = int.Parse(robotPosTokens[0].Replace("p=", ""));
                var py = int.Parse(robotPosTokens[1]);
                var robotVelTokens = robotTokens[1].Split(",");
                var vx = int.Parse(robotVelTokens[0].Replace("v=", ""));
                var vy = int.Parse(robotVelTokens[1]);
                Robots.Add(new BathroomRobot((px, py), (vx, vy), 'o'));
            }
            _map = new int[Height, Width];

            // Set up Easter egg - after a bot of exprimentation worked out these coords will identify it vv
            _xmasTreeCoords = new List<(int, int)>();
            _xmasTreeCoords.AddRange(new[] { (48, 29), (49, 29), (50, 29), (51, 29), (52, 29) });            
        }

        /// <summary>
        /// Work out safety factor.
        /// </summary>
        /// <returns>Safety factor index.</returns>
        public int GetSafetyFactor()
        {
            var (q1, q2, q3, q4) = (0, 0, 0, 0);
            var (midHeight, midWidth) = ((Height - 1) / 2, (Width-1)/2);            
            for (var row = 0; row < Height; row++)
            {
                for (var col = 0; col < Width; col++)
                {
                    var robotCount = _map[row, col];
                    if (row >= 0 && row < midHeight && col >= 0 && col < midWidth)
                        q1 += robotCount;
                    else if (row >= 0 && row < midHeight && col > midWidth && col < Width)
                        q2 += robotCount;
                    else if (row > midHeight && row < Height && col >= 0 && col < midWidth)
                        q3 += robotCount;
                    else if (row > midHeight && row < Height && col > midWidth && col < Width)
                        q4 += robotCount;
                }
            }
            return q1 * q2 * q3 * q4;
        }

        /// <summary>
        /// Move robots one cycle.
        /// </summary>
        public void MoveRobots()
        {            
            foreach (BathroomRobot robot in Robots)
            {
                var (dx, dy) = robot.Position;
                var (vx, vy) = robot.Velocity;
                var (newDx, newDy) = (dx + vx, dy + vy);

                if (newDx < 0) newDx = Width + newDx;
                if (newDx > Width-1) newDx -= Width;
                if (newDy < 0) newDy += Height;
                if (newDy > Height-1) newDy -= Height;
                robot.Position = (newDx, newDy);
            }
            UpdateMap();
        }

        /// <summary>
        /// Display the current robot map.
        /// </summary>
        public void Display()
        {
            for (var row = 0; row < Height; row++)
            {
                Console.Write($"{row}:");
                for (var col = 0; col < Width; col++)
                {
                    var botCount = _map[row, col];
                    if (botCount == 0)
                        Console.Write(' ');
                    else
                        Console.Write('o');
                }
                Console.WriteLine();
            }
        }

        // Update map with latest robots positions.
        private void UpdateMap()
        {
            // Re-init grid.
            for (var row = 0; row < Height; row++)
                for (var col = 0; col < Width; col++)
                    _map[row, col] = 0;

            // Draw robots on grid.
            foreach (BathroomRobot robot in Robots)
            {
                var (col, row) = (robot.Position.Item1, robot.Position.Item2);
                _map[row, col]++;
            }
        }

        /// <summary>
        /// In current configuration does Easter Egg exist?
        /// </summary>
        /// <returns>True if found, false otherwise.</returns>
        public bool FoundEasterEgg()
        {
            return (_xmasTreeCoords.All(x => Robots.Select(r => r.Position).ToList().Contains(x)));            
        }
    }
}