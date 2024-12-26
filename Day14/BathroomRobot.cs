namespace Day14
{
    public class BathroomRobot
    {
        // (X,Y) - right and down from top left (0,0).
        public (int,int) StartPosition { get; set; }
        public (int, int) Position { get; set; }
        public (int,int) Velocity { get; set; }
        public char Label { get; set; }

        /// <summary>
        /// Represents a bathroom robot.
        /// </summary>
        /// <param name="startPosition">Position before started off.</param>
        /// <param name="velocity">Movement trajectory.</param>
        /// <param name="label">Identifier for debugging.</param>
        public BathroomRobot((int,int) startPosition, (int,int) velocity, char label)
        {
            StartPosition = startPosition;
            Velocity = velocity;
            Position = startPosition;
            Label = label;
        }
    }
}