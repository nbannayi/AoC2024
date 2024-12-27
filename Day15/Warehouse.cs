using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    public class Warehouse
    {
        public (int, int) RobotPosition { get; set; }

        private char[,] _map;
        public char[,] Map { get { return _map; } }

        private int _height, _width;
        public int Height { get { return _height; } }
        public int Width { get { return _width; } }

        private bool _isWide;
        public object IsWide { get { return _isWide; } }

        /// <summary>
        /// Create a representation of a warehouse.
        /// </summary>
        /// <param name="mapLines">Lines to initialise map from.</param>
        /// <param name="isWide">If true create wide version..</param>
        public Warehouse(string[] mapLines, bool isWide)
        {
            _height = mapLines.Length;
            _isWide = isWide;
            _width = _isWide ? mapLines[0].Length * 2 : mapLines[0].Length;
            _map = new char[_height, _width];
            var offset = isWide ? 2 : 1;
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col += offset)
                {
                    if (_isWide)
                    {
                        var currentChar = mapLines[row][col / 2];
                        if (currentChar == 'O')
                        {
                            _map[row, col] = '[';
                            _map[row, col + 1] = ']';
                        }
                        else if (currentChar == '#')
                        {
                            _map[row, col] = '#';
                            _map[row, col + 1] = '#';
                        }
                        else if (currentChar == '.')
                        {
                            _map[row, col] = '.';
                            _map[row, col + 1] = '.';
                        }
                        else if (currentChar == '@')
                        {
                            _map[row, col] = '@';
                            _map[row, col + 1] = '.';
                            RobotPosition = (row, col);
                        }
                    }
                    else
                    {
                        _map[row, col] = mapLines[row][col];
                        if (_map[row, col] == '@')
                            RobotPosition = (row, col);
                    }
                }
            }
        }

        /// <summary>
        /// Display current map.
        /// </summary>
        public void Display()
        {
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                    Console.Write(_map[row, col]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Get total GPS of all boxes in current state.
        /// </summary>
        /// <returns>Total GPS of boxes.</returns>
        public int GetTotalGPS()
        {
            var totalGPS = 0;
            for (var row = 0; row < _height; row++)
            {
                for (var col = 0; col < _width; col++)
                    if (_map[row, col] == 'O' || _map[row, col] == '[')
                        totalGPS += (row * 100 + col);
            }
            return totalGPS;
        }

        /// <summary>
        /// Move the robot in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to move.</param>        
        public void MoveRobot(Direction direction)
        {
            var (row, col) = RobotPosition;
            var (newRow, newCol) = direction switch
            {
                Direction.Up => (row - 1, col),
                Direction.Down => (row + 1, col),
                Direction.Left => (row, col - 1),
                Direction.Right => (row, col + 1),
                _ => (row, col)
            };

            // Hit outer boundary or a wall.
            var newChar = _map[newRow, newCol];
            if (newChar == '#')
                return;
            // No boxes.
            else if (newChar == '.')
            {
                RobotPosition = (newRow, newCol);
                _map[newRow, newCol] = '@';
                _map[row, col] = '.';
                return;
            }
            // If foumd a box, need to push entire column.
            else if (newChar == 'O' || newChar == '[' || newChar == ']')
            {
                pushBoxes((newRow, newCol), direction);
                return;
            }
        }

        // Handle box column pushing logic.
        private void pushBoxes((int, int) newRobotPosition, Direction direction)
        {
            // Push a column of boxes in specified direction.
            var (boxRow, boxCol) = newRobotPosition;
            switch (direction)
            {
                case Direction.Left:
                    if (_map[boxRow, boxCol - 1] == '#') break;
                    bool canMove = false; int l;
                    for (l = boxCol; l > 0; l--) if (_map[boxRow, l] == '#') break;
                    if (l == 0) l++;
                    for (var col = boxCol; col > l; col--)
                    {
                        if (_map[boxRow, col - 1] == '.')
                        {
                            for (var col2 = col - 1; col2 < boxCol; col2++)
                                _map[boxRow, col2] = _map[boxRow, col2 + 1];
                            canMove = true;
                            break;
                        }
                    }
                    if (canMove)
                    {
                        RobotPosition = newRobotPosition;
                        _map[boxRow, boxCol] = '@';
                        _map[boxRow, boxCol + 1] = '.';
                    }
                    break;
                case Direction.Right:
                    if (_map[boxRow, boxCol + 1] == '#') break;
                    canMove = false;
                    for (l = boxCol; l < _width; l++) if (_map[boxRow, l] == '#') break;
                    if (l == _width - 1) l--;
                    for (var col = boxCol; col < l; col++)
                    {
                        if (_map[boxRow, col + 1] == '.')
                        {
                            for (var col2 = col + 1; col2 > boxCol; col2--)
                                _map[boxRow, col2] = _map[boxRow, col2 - 1];
                            canMove = true;
                            break;
                        }
                    }
                    if (canMove)
                    {
                        RobotPosition = newRobotPosition;
                        _map[boxRow, boxCol] = '@';
                        _map[boxRow, boxCol - 1] = '.';
                    }
                    break;
                case Direction.Up:
                    if (!_isWide)
                        PushBoxesUpStandard(boxRow, boxCol);
                    else
                        PushBoxesWide(boxRow, boxCol, Direction.Up);
                    break;
                case Direction.Down:
                    if (!_isWide)
                        PushBoxesDownStandard(boxRow, boxCol);
                    else
                        PushBoxesWide(boxRow, boxCol, Direction.Down);
                    break;
                default:
                    break;
            }
        }

        private void PushBoxesUpStandard(int boxRow, int boxCol)
        {
            if (_map[boxRow - 1, boxCol] == '#') return;
            bool canMove = false; int l;
            for (l = boxRow; l > 0; l--) if (_map[l, boxCol] == '#') break;
            if (l == 1) l++;
            for (var row = boxRow; row > l; row--)
            {
                if (_map[row - 1, boxCol] == '.')
                {
                    for (var row2 = row - 1; row2 < boxRow; row2++)
                        _map[row2, boxCol] = _map[row2 + 1, boxCol];
                    canMove = true;
                    break;
                }
            }
            if (canMove)
            {
                RobotPosition = (boxRow, boxCol);
                _map[boxRow, boxCol] = '@';
                _map[boxRow + 1, boxCol] = '.';
            }
        }

        private void PushBoxesDownStandard(int boxRow, int boxCol)
        {
            if (_map[boxRow + 1, boxCol] == '#') return;
            bool canMove = false; int l;
            for (l = boxRow; l < _height - 1; l++) if (_map[l, boxCol] == '#') break;
            if (l == _height - 1) l--;
            for (var row = boxRow; row < l; row++)
            {
                if (_map[row + 1, boxCol] == '.')
                {
                    for (var row2 = row + 1; row2 > boxRow; row2--)
                        _map[row2, boxCol] = _map[row2 - 1, boxCol];
                    canMove = true;
                    break;
                }
            }
            if (canMove)
            {
                RobotPosition = (boxRow, boxCol);
                _map[boxRow, boxCol] = '@';
                _map[boxRow - 1, boxCol] = '.';
            }
        }

        // Part 2 logic vv.

        // Push boxes in wide mode.
        private void PushBoxesWide(int boxRow, int boxCol, Direction direction)
        {
            // Get connected boxes.
            var connectedBoxes = GetConnectedBoxesWide(boxRow, boxCol, new List<(int, int)>(), direction);

            // Now check all boxes can translate in given direction.
            if (CanMoveWide(connectedBoxes, direction))
            {
                // Finally if they can move, move them.
                var vertOffset = direction == Direction.Up ? 1 : -1;
                var tempBoxes = new Dictionary<(int,int),char> ();

                // Create an empty space.
                foreach (var box in connectedBoxes)
                {
                    // Store box symbol.
                    tempBoxes[box] = _map[box.Item1, box.Item2];
                    // Replace the box with a space.
                    _map[box.Item1, box.Item2] = '.';
                }

                // Translate boxes down or up.
                foreach (var box in connectedBoxes)
                    _map[box.Item1 - vertOffset, box.Item2] = tempBoxes[box];

                // Finally move the robot.
                RobotPosition = (boxRow, boxCol);
                _map[boxRow, boxCol] = '@';
                _map[boxRow + vertOffset, boxCol] = '.';
            }
        }

        // Determine if a set of connected blocks can move.
        private bool CanMoveWide(List<(int,int)> connectedBoxes, Direction direction)
        {
            bool canMove = true;
            var vertOffset = direction == Direction.Up ? 1 : -1;
            foreach (var box in connectedBoxes)
            {
                if (_map[box.Item1 - vertOffset, box.Item2] == '#')
                {
                    canMove = false;
                    break;
                }
            }
            return canMove;
        }

        // Get all connected boxes to attemot tp push.
        private List<(int,int)> GetConnectedBoxesWide(int boxRow, int boxCol, List<(int,int)> connectedBoxes, Direction direction)
        {
            // Get current symbol and determine which bxoes to check.
            var currentSymbol = _map[boxRow, boxCol];
            var vertOffset = direction == Direction.Up ? 1 : -1;
            if (currentSymbol == '[')
            {
                var (nextPos1, nextPos2) = ((boxRow - vertOffset, boxCol), (boxRow - vertOffset, boxCol + 1));
                connectedBoxes.Add((boxRow, boxCol));
                connectedBoxes.Add((boxRow, boxCol + 1));
                GetConnectedBoxesWide(nextPos1.Item1, nextPos1.Item2, connectedBoxes, direction);
                GetConnectedBoxesWide(nextPos2.Item1, nextPos2.Item2, connectedBoxes, direction);
            }
            else if (currentSymbol == ']')
            {
                var (nextPos1, nextPos2) = ((boxRow - vertOffset, boxCol), (boxRow - vertOffset, boxCol - 1));
                connectedBoxes.Add((boxRow, boxCol));
                connectedBoxes.Add((boxRow, boxCol - 1));
                GetConnectedBoxesWide(nextPos1.Item1, nextPos1.Item2, connectedBoxes, direction);
                GetConnectedBoxesWide(nextPos2.Item1, nextPos2.Item2, connectedBoxes, direction);
            }
            return connectedBoxes.Distinct().ToList();
        }
    }
}