using System;
using System.Collections.Generic;
using System.Linq;

namespace Day06
{
    public class Map
    {
        private char[][] _grid;
        public char[][] Grid { get { return _grid; } }

        private Guard _guard;
        public EndState GuardEndState { get { return _guard.EndState; } }
        private List<(int, int, Orientation)> _obstaclePositionAndOrientations = new List<(int, int, Orientation)>();
        private string[] _mapLines;

        /// <summary>
        /// Create a new Map object.
        /// </summary>
        /// <param name="mapLines">Initialise with Map layout.</param>
        public Map(string[] mapLines)
        {
            _mapLines = mapLines;
            Init();
        }

        /// <summary>
        /// Initialise a new Map object.
        /// </summary>
        /// <param name="mapLines">Initialise with Map layout.</param>
        public void Init()
        {
            // Turn array of strings into 2D array of char.
            _grid = _mapLines.Select(ml => ml.ToCharArray()).ToArray();
            var startPosition = (0, 0);
            // Set start position of guard.
            var found = false;
            for (var r = 0; r < _grid.Length; r++)
            {
                for (var c = 0; c < _grid[0].Length; c++)
                {
                    if (_grid[r][c] == '^')
                    {
                        startPosition = (c, r);
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            _guard = new Guard(startPosition, Orientation.Up);
        }

        /// <summary>
        /// Moves guard and updates position, orientation, trail etc.
        /// </summary>        
        public void MoveGuardOnce()
        {
            var offset = (0, 0);
            switch (_guard.Orientation)
            {
                case Orientation.Up:
                    offset = (0,-1);                    
                    break;
                case Orientation.Right:
                    offset = (1, 0);
                    break;
                case Orientation.Down:
                    offset = (0, 1);
                    break;
                case Orientation.Left:
                    offset = (-1, 0);
                    break;
            }
            var newX = _guard.Position.Item1 + offset.Item1;
            var newY = _guard.Position.Item2 + offset.Item2;
            if (newX == _grid.Length || newY == _grid[0].Length || newX < 0 || newY < 0)
            {
                _guard.EndState = EndState.Exited;
                return;
            }
            else if (_grid[newY][newX] == '#')
            {
                var newObstaclePositionAndOrientation = (newX, newY, _guard.Orientation);
                // We have found a loop exit.
                if (_obstaclePositionAndOrientations.Contains(newObstaclePositionAndOrientation))
                {
                    _guard.EndState = EndState.Stuck;
                    return;
                }
                _obstaclePositionAndOrientations.Add((newX, newY, _guard.Orientation));
                _guard.Turn();
            }
            else
            {
                _grid[_guard.Position.Item2][_guard.Position.Item1] = 'X';
                _guard.Position = (newX, newY);
                _grid[newY][newX] = _guard.GetOrientationSymbol();
            }            
        }

        /// <summary>
        /// Display the current map.
        /// </summary>
        public void Display()
        {
            foreach (var gridLine in _grid)
            {
                Console.WriteLine(gridLine);
            }
        }

        /// <summary>
        /// Examines grid and finds number of distinct squares visited.
        /// </summary>
        /// <returns>No. of distinct sqaures visited.</returns>
        public int GetNoVisited()
        {
            int totalVisited  = 0;
            foreach (var gridRow in _grid)            
                totalVisited += gridRow.Where(c => c != '#' && c != '.').Count();            
            return totalVisited;
        }

        /// <summary>
        /// Move guard around till she leaves map or gets stuck.
        /// </summary>
        /// <returns>Final state of the guard.</returns>
        public EndState MoveGuard()
        {
            _obstaclePositionAndOrientations.Clear();
            while (_guard.EndState == EndState.Unknown)
                MoveGuardOnce();
            return _guard.EndState;
        }

        /// <summary>
        /// Place an obstacle on the map at given grid coord.
        /// </summary>
        /// <param name="x">x coord of obstacle.</param>
        /// <param name="y">y coord of obstacle.</param>
        public void PlaceObstacle(int x, int y)
        {
            _grid[y][x] = '#';
        }
    }
}