using System.Collections.Generic;

namespace Day16
{
    public struct State
    {
        public (int, int) Position;
        public Direction Direction;
        public int Score;
        public List<(int, int)> Path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="score"></param>
        /// <param name="path"></param>
        public State((int, int) position, Direction direction, int score, List<(int, int)> path)
        {
            Position = position;
            Direction = direction;
            Score = score;
            Path = path;
        }

        /// <summary>
        /// Progress one step.
        /// </summary>
        /// <returns><New state advanced 1 step./returns>
        public State Step()
        {
            var newPos = (Position.Item1, Position.Item2);
            switch (Direction)
            {
                case Direction.North:
                    newPos.Item1--;
                    break;
                case Direction.South:
                    newPos.Item1++;
                    break;
                case Direction.East:
                    newPos.Item2++;
                    break;
                case Direction.West:
                    newPos.Item2--;
                    break;
            }
            var newPath = new List<(int, int)>(Path);
            newPath.Add(newPos);
            return new State(newPos, Direction, Score + 1, newPath);
        }

        /// <summary>
        /// Return a new state rotated accordingly.
        /// </summary>
        /// <param name="clockwise">True for clockwise, false for anti.</param>
        /// <returns>New rotated struct.</returns>
        public State Rotate(bool clockwise)
        {
            Direction newDir = Direction.North;
            switch (Direction)
            {
                case Direction.North:
                    newDir = clockwise ? Direction.East : Direction.West;
                    break;
                case Direction.South:
                    newDir = clockwise ? Direction.West : Direction.East;
                    break;
                case Direction.East:
                    newDir = clockwise ? Direction.South : Direction.North;
                    break;
                case Direction.West:
                    newDir = clockwise ? Direction.North : Direction.South;
                    break;
            }
            var newPath = new List<(int, int)>(Path);
            return new State(Position, newDir, Score + 1000, newPath);
        }
    }
}