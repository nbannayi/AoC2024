namespace Day09
{
    public enum BlockType
    {
        File,
        Free
    }

    public class FileBlock
    {        
        public BlockType BlockType { get; set; }     
        public int Id { get; set; }        
        public int Size { get; set; }
        public int RemainingSize { get; set; }

        public FileBlock(BlockType blockType, int id, int size)
        {
            BlockType = blockType;
            Id = id;
            Size = size;
            RemainingSize = blockType == BlockType.File ? -1 : size;
        }

        public override string ToString()
        {
            return $"{BlockType}: Id:{Id}, Size:{Size}, Remaining:{RemainingSize}";
        }
    }
}