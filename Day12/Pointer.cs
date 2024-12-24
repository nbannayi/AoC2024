namespace Day12
{
    public class Pointer
    {
        public Direction Direction { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        /// <summary>
        /// Create a new pointer to walk round the shape.
        /// </summary>
        /// <param name="row">Current row</param>
        /// <param name="col">Current col.</param>
        /// <param name="direction">Current direction.</param>
        public Pointer(int row, int col, Direction direction)
        {
            Row = row;
            Col = col;
            Direction = direction;
        }

        /// <summary>
        /// Turn 90 degrees anit-clockwise.
        /// </summary>
        public void TurnClockwise()
        {
            switch (Direction)
            {
                case Direction.East:
                    Direction = Direction.South;
                    break;
                case Direction.South:
                    Direction = Direction.West;
                    break;
                case Direction.North:
                    Direction = Direction.East;
                    break;
                case Direction.West:
                    Direction = Direction.North;
                    break;
            }
        }

        /// <summary>
        /// Turn 90 degrees clockwise.
        /// </summary>
        public void TurnAntiClockwise()
        {
            switch (Direction)
            {
                case Direction.East:
                    Direction = Direction.North;
                    break;
                case Direction.South:
                    Direction = Direction.East;
                    break;
                case Direction.North:
                    Direction = Direction.West;
                    break;
                case Direction.West:
                    Direction = Direction.South;
                    break;
            }
        }

        /// <summary>
        /// Peek what's ahead.
        /// </summary>
        /// <returns>What's ahead - a wall?</returns>
        public (int,int) PeekAdvance()
        {
            switch (Direction)
            {
                case Direction.East:
                    return (Row, Col + 1);
                case Direction.South:
                    return (Row + 1, Col);
                case Direction.North:
                    return (Row - 1, Col);
                case Direction.West:
                    return (Row, Col - 1);
            }
            return (Row, Col);
        }

        /// <summary>
        /// Peek to the right.
        /// </summary>
        /// <returns>What's on the right?</returns>
        public (int, int) PeekRight()
        {
            switch (Direction)
            {
                case Direction.East:
                    return (Row + 1, Col);
                case Direction.South:
                    return (Row, Col - 1);
                case Direction.North:
                    return (Row, Col + 1);
                case Direction.West:
                    return (Row - 1, Col);
            }
            return (Row, Col);
        }

        /// <summary>
        /// Go forward one step in current direction.
        /// </summary>
        public void Advance()
        {
            switch (Direction)
            {
                case Direction.East:
                    Col++;
                    break;
                case Direction.South:
                    Row++;
                    break;
                case Direction.North:
                    Row--;
                    break;
                case Direction.West:
                    Col--;
                    break;
            }
        }
    }
}