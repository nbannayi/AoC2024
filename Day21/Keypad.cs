using System.Collections.Generic;

namespace Day21
{
    public enum KeypadType
    {
        Numeric,
        Directional
    }

    public class Keypad
    {
        public KeypadType KeypadType { get; set; }

        /*
            +---+---+
            | ^ | A |
        +---+---+---+
        | < | v | > |
        +---+---+---+
        */
        private static char[,] _directionalKeypad =
        {
            { ' ', '^', 'A' },
            { '<', 'v', '>' }
        };

        /*
        +---+---+---+
        | 7 | 8 | 9 |
        +---+---+---+
        | 4 | 5 | 6 |
        +---+---+---+
        | 1 | 2 | 3 |
        +---+---+---+
            | 0 | A |
            +---+---+
        */
        private static char[,] _numericKeypad =
        {
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { ' ', '0', 'A' }
        };

        private Dictionary<(char, char), List<string>> _shortestPaths;
        private Dictionary<char, (int,int)> _keyPositions;

        /// <summary>
        /// Represents a robot operated keypad.
        /// </summary>
        public Keypad(KeypadType keypadType)
        {
            _shortestPaths = new Dictionary<(char, char), List<string>>();
            _keyPositions = new Dictionary<char, (int, int)>();
            KeypadType = keypadType;            
        }

        /// <summary>
        /// Get all sequences of directions for a key press, current key will be updated.
        /// </summary>
        /// <param name="code">Button to press.</param>
        /// <returns>Sortest lists of directions.</returns>
        public List<string> EnterCode(string code)
        {
            code = "A" + code;
            var allPaths = new List<List<string>>();
            for (var i = 0; i < code.Length - 1; i++)
            {
                var (char1, char2) = (code[i], code[i + 1]);                
                allPaths.Add(GetShortestPaths(char1, char2));
            }

            List<string> Combine(List<List<string>> lists, int index = 0, string current = "")
            {
                if (index == lists.Count)
                    return new List<string> { current };
                var result = new List<string>();
                foreach (var item in lists[index])
                {
                    var executedItem = item + "A";
                    result.AddRange(Combine(lists, index + 1, current + executedItem));
                }
                return result;
            }

            return Combine(allPaths);
        }

        /// <summary>
        /// Get all shortest paths between keys.
        /// </summary>
        /// <param name="startKey">Start key to move from.</param>
        /// <param name="endKey">End key.</param>
        /// <returns>List of all shortest path strings.</returns>
        public List<string> GetShortestPaths(char startKey, char endKey)
        {
            if (!_shortestPaths.ContainsKey((startKey, endKey)))
            {
                var keypad = KeypadType == KeypadType.Directional ? _directionalKeypad : _numericKeypad;

                // Reverse as reconstruction reverses again. v
                var startPos = GetKeypadPosition(endKey);
                var endPos = GetKeypadPosition(startKey);

                var directions = new (int, int)[] { (1, 0), (-1, 0), (0, -1), (0, 1) };
                var queue = new Queue<(int, int)>();
                var distance = new Dictionary<(int, int), int>();
                var parents = new Dictionary<(int, int), List<(int, int)>>();

                queue.Enqueue(startPos);
                distance[startPos] = 0;
                parents[startPos] = new List<(int, int)>();

                // BFS to find all shortest paths from start to end key.
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    int currentDistance = distance[current];
                    foreach (var dir in directions)
                    {
                        var (row, col) = (current.Item1 + dir.Item1, current.Item2 + dir.Item2);
                        if (row >= 0 && row < keypad.GetLength(0) && col >= 0 && col < keypad.GetLength(1) && keypad[row, col] != ' ')
                        {
                            if (!distance.ContainsKey((row, col)))
                            {
                                distance[(row, col)] = currentDistance + 1;
                                parents[(row, col)] = new List<(int, int)> { current };
                                queue.Enqueue((row, col));
                            }
                            else if (distance[(row, col)] == currentDistance + 1)
                                parents[(row, col)].Add(current);
                        }
                    }
                }
                if (!parents.ContainsKey(endPos))
                    return new List<string>(); // No paths.

                // Reconstruct paths.
                var paths = new List<List<(int, int)>>();
                var path = new List<(int, int)>();
                ReconstructPaths(endPos, startPos, parents, path, paths);

                // Finally turn all paths to strings.
                var pathDirs = new List<string>();
                foreach (var path2 in paths)
                {
                    var dirs = "";
                    for (var i = 0; i < path2.Count - 1; i++)
                    {
                        var (row1, col1) = path2[i];
                        var (row2, col2) = path2[i + 1];
                        var diff = (row2 - row1, col2 - col1);
                        switch (diff)
                        {
                            case (-1, 0):
                                dirs += "^";
                                break;
                            case (1, 0):
                                dirs += "v";
                                break;
                            case (0, -1):
                                dirs += "<";
                                break;
                            case (0, 1):
                                dirs += ">";
                                break;
                        }
                    }
                    pathDirs.Add(dirs);
                }
                _shortestPaths[(startKey, endKey)] = pathDirs;
            }
            return _shortestPaths[(startKey, endKey)];

            // Inner function to create paths.
            void ReconstructPaths((int,int) current, (int,int) start, Dictionary<(int,int), List<(int,int)>> parents,
                List<(int,int)> path, List<List<(int,int)>> paths)
            {
                path.Add(current);
                if (current == start)
                    paths.Add(new List<(int, int)>(path));
                else if (parents.ContainsKey(current))
                {
                    foreach (var parent in parents[current])                    
                        ReconstructPaths(parent, start, parents, path, paths);                    
                }
                path.RemoveAt(path.Count - 1); // Backtrack.
            }
        }

        // Get position of a specific key on keypad.
        private (int,int) GetKeypadPosition(char key)
        {
            (int, int) keyPos = (-1, -1);
            bool found = false;
            var keypad = KeypadType == KeypadType.Directional ? _directionalKeypad : _numericKeypad;
            // Search for and cache if not already there.
            if (!_keyPositions.ContainsKey(key))
            {
                for (int row = 0; row < keypad.GetLength(0); row++)
                {
                    for (int col = 0; col < keypad.GetLength(1); col++)
                    {
                        if (keypad[row, col] == key)
                        {
                            found = true;
                            keyPos = (row, col);
                            break;
                        }
                    }
                    if (found) break;
                }
                _keyPositions[key] = keyPos;
            }
            return _keyPositions[key];
        }
    }
}