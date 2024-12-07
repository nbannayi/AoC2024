namespace Day06
{
    public enum Orientation
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum EndState
    {
        Unknown,
        Exited,
        Stuck
    }

    public class Guard
    {
        public (int, int) Position { get; set; }        
        public Orientation Orientation { get; set; }
        public EndState EndState { get; set; }

        /// <summary>
        /// Create a guard object.
        /// </summary>
        /// <param name="position">Current position.</param>
        /// <param name="orientation">Current orientation.</param>
        public Guard((int, int) position, Orientation orientation)
        {
            Position = position;
            Orientation = orientation;
            EndState = EndState.Unknown;
        }

        /// <summary>
        /// Get the symbol for the current orientation.
        /// </summary>
        /// <returns>Return orientation symbol.</returns>
        public char GetOrientationSymbol()
        {
            switch (Orientation)
            {
                case Orientation.Up:
                    return '^';                    
                case Orientation.Right:
                    return '>';
                case Orientation.Down:
                    return 'v';
                case Orientation.Left:
                    return '<';
            }
            return ' ';
        }

        /// <summary>
        /// Turn guard 90 degress clockwise.
        /// </summary>
        public void Turn()
        {
            switch (Orientation)
            {
                case Orientation.Up:
                    Orientation = Orientation.Right;                    
                    break;
                case Orientation.Right:
                    Orientation = Orientation.Down;                    
                    break;
                case Orientation.Down:
                    Orientation = Orientation.Left;                    
                    break;
                case Orientation.Left:
                    Orientation = Orientation.Up;                    
                    break;
            }
        }
    }
}