using System.Collections.Generic;
using System.Linq;

namespace Day09
{
    public class Disk
    {
        List<FileBlock> _fileBlocks;
        public List<FileBlock> FileBlocks { get { return _fileBlocks; } }

        public Disk(string diskContents)
        {
            var diskMap = diskContents
                .ToCharArray()
                .Select(d => int.Parse(d.ToString()))
                .ToList();

            _fileBlocks = new List<FileBlock>();

            for (int i = 0; i < diskMap.Count; i++)
            {
                // File occurs every other char.
                if (i % 2 == 0)
                    _fileBlocks.Add(new FileBlock(BlockType.File, i / 2, diskMap[i]));
                else
                    _fileBlocks.Add(new FileBlock(BlockType.Free, -1, diskMap[i]));
            }
        }

        private int GetNextFreeBlockIndex()
        {
            for (int i = 0; i < FileBlocks.Count; i++)
            {
                var fileBlock = _fileBlocks[i];
                if (fileBlock.BlockType == BlockType.Free && fileBlock.RemainingSize > 0)
                    return i;
            }
            return 0;
        }

        private int GetNextBlockToAssignIndex()
        {
            var indexesToDelete = new List<int>();
            var assignIndex = 0;
            for (int i = _fileBlocks.Count-1; i >= 0; i--)
            {
                var fileBlock = _fileBlocks[i];
                if ((fileBlock.BlockType == BlockType.Free && fileBlock.RemainingSize == 0) || fileBlock.Size == 0)
                    indexesToDelete.Add(i);
                else if (fileBlock.BlockType == BlockType.File)
                {
                    assignIndex = i;
                    i = -1;
                }
            }
            foreach (var idx in indexesToDelete) _fileBlocks.RemoveAt(idx);
            return assignIndex;
        }

        /// <summary>
        /// Compact disk using first (harder) alogirithm a block at time.
        /// </summary>
        public void Compact1()
        {
            // Get next free index.
            var nextFreeBlockIndex = GetNextFreeBlockIndex();
            while (nextFreeBlockIndex > 0)
            {
                // Get next block to assign index.
                var nextBlockToAssignIndex = GetNextBlockToAssignIndex();
                if (nextBlockToAssignIndex < 0)
                    break;

                // Process.
                var freeBlock = _fileBlocks[nextFreeBlockIndex];
                var assignBlock = _fileBlocks[nextBlockToAssignIndex];

                // Case where assignBlock is larger or equal to than free space.
                if (assignBlock.Size >= freeBlock.RemainingSize)
                {
                    freeBlock.BlockType = BlockType.File;
                    freeBlock.Id = assignBlock.Id;
                    freeBlock.RemainingSize = -1;
                    assignBlock.Size -= freeBlock.Size;                    
                }
                // Otherwise handle assignBlock is less than free space.
                else
                {
                    var newFreeBlock = new FileBlock(BlockType.Free, -1, freeBlock.RemainingSize - assignBlock.Size);
                    freeBlock.BlockType = BlockType.File;
                    freeBlock.Id = assignBlock.Id;
                    freeBlock.RemainingSize = -1;                    
                    freeBlock.Size = assignBlock.Size;
                    assignBlock.Size = 0;
                    _fileBlocks.Insert(nextFreeBlockIndex+1, newFreeBlock);
                }

                nextFreeBlockIndex = GetNextFreeBlockIndex();
            }
        }

        /// <summary>
        /// Compact disk using a whole file at a time.
        /// </summary>
        public void Compact2()
        {
            for (int i = _fileBlocks.Count - 1; i >= 0; i--)
            {
                var fileBlock1 = _fileBlocks[i];                
                if (fileBlock1.BlockType == BlockType.File)
                {
                    for (var j = 0; j < i; j++)
                    {
                        var fileBlock2 = _fileBlocks[j];
                        if (fileBlock2.BlockType == BlockType.Free && fileBlock2.RemainingSize >= fileBlock1.Size)
                        {
                            var newFreeBlock = new FileBlock(BlockType.Free, -1, fileBlock2.RemainingSize - fileBlock1.Size);
                            fileBlock2.BlockType = BlockType.File;
                            fileBlock2.Id = fileBlock1.Id;
                            fileBlock2.RemainingSize = -1;
                            fileBlock2.Size = fileBlock1.Size;
                            fileBlock1.BlockType = BlockType.Free;
                            _fileBlocks.Insert(j + 1, newFreeBlock);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get checksum of the disk after compression.
        /// </summary>
        /// <returns>Total checksum.</returns>
        public long GetChecksum()
        {
            var total = 0L;
            var position = 0;
            foreach (FileBlock fb in _fileBlocks)
            {
                var size = fb.Size;
                for (int i = 0; i < size; i++)
                {
                    // Don't include free space.
                    if (fb.BlockType != BlockType.Free)
                        total += fb.Id * position;
                    position++;
                }
            }
            return total;
        }
    }
}