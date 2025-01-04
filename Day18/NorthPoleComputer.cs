using System;
using System.Collections.Generic;

namespace Day18
{
    public class NorthPoleComputer
    {
        public char[,] Memory { get; set; }

        private int _noRows, _noCols;
        (int, int) _startPos, _endPos;

        /// <summary>
        /// Represent a North Pole computer.
        /// </summary>
        /// <param name="noRows">Number of rows in memory.</param>
        /// <param name="noCols">Number of cols in memory.</param>
        public NorthPoleComputer(int noRows, int noCols)
        {
            _noRows = noRows;
            _noCols = noCols;
            Memory = new char[_noRows, _noCols];
            _startPos = (0, 0);
            _endPos = (noCols - 1, noCols - 1);
            for (int i = 0; i < _noRows; i++)
                for (int j = 0; j < _noCols; j++)
                    Memory[i, j] = '.';
        }

        /// <summary>
        /// Drop a byte down to (x,y) coord in Memory.
        /// </summary>
        /// <param name="pos">Memory location to drop courrption to.</param>
        public void DropByte((int x,int y) pos)
        {
            Memory[pos.y, pos.x] = '#';
        }

        /// <summary>
        /// Drop first n bytes from given list.
        /// </summary>
        /// <param name="bytes">Bytes to drop.</param>
        /// <param name="n">No bytes to drop from list.</param>
        public void DropNBytes(List<(int,int)> bytes, int n)
        {
            // Drop first n bytes.
            for (int i = 0; i < n; i++)
                DropByte(bytes[i]);
        }

        /// <summary>
        /// Display the memory.
        /// </summary>
        public void MemoryDump()
        {
            for (int i = 0; i < _noRows; i++)
            {
                for (int j = 0; j < _noCols; j++)
                    Console.Write(Memory[i, j]);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Find shortest number of steps through the maze.
        /// </summary>
        /// <returns>A number of steps or -1 if none exists.</returns>
        public int FindShortestPath()
        {
            var queue = new Queue<((int, int), int)>();
            var visited = new List<(int, int)>();
            queue.Enqueue((_startPos, 0));
            
            while (queue.Count > 0)
            {
                var (curPos, curSteps) = queue.Dequeue();                
                if (curPos == _endPos)
                    return curSteps;
                var (x, y) = curPos;
                if (x < 0 || y < 0 || x > _noCols - 1 || y > _noRows - 1 || Memory[y, x] == '#' || visited.Contains(curPos))
                    continue;
                visited.Add(curPos);
                queue.Enqueue(((x - 1, y), curSteps + 1));
                queue.Enqueue(((x + 1, y), curSteps + 1));
                queue.Enqueue(((x, y - 1), curSteps + 1));
                queue.Enqueue(((x, y + 1), curSteps + 1));
            }

            return -1;
        }

        /// <summary>
        /// Find byte drop that blocks the path ahead.
        /// </summary>
        /// <param name="bytes">List of bytes to check.</param>
        /// <param name="startByteNo">Point to start from in list of bytes.</param>
        /// <returns></returns>
        public (int,int) FindBlockingByte(List<(int x,int y)> bytes, int startByteNo)
        {
            // Drop bytes to this start point (just in case not already done.)
            var byteNo = startByteNo-1;
            DropNBytes(bytes, byteNo);
            
            // Now from startByteNo to end successively drop till blocked.
            var noSteps = 0;            
            while (noSteps >= 0 && byteNo < bytes.Count)
            {
                byteNo++;
                DropByte(bytes[byteNo]);
                noSteps = FindShortestPath();                
            }
            return bytes[byteNo];
        }
    }
}