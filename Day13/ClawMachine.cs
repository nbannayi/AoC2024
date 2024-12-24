namespace Day13
{
    public class ClawMachine
    {
        public (int, int) ButtonA { get; set; }
        public (int, int) ButtonB { get; set; }
        public (long, long) Prize { get; set; }

        /// <summary>
        /// Create a new claw machine.
        /// </summary>
        /// <param name="buttonA">Button A X and Y config.</param>
        /// <param name="buttonB">Button B X and Y config.</param>
        /// <param name="prize">Prize X and Y config.</param>        
        public ClawMachine((int,int) buttonA, (int, int) buttonB, (long, long) prize)
        {
            ButtonA = buttonA;
            ButtonB = buttonB;
            Prize = prize;         
        }

        /// <summary>
        /// Win prize (if possible.)
        /// </summary>
        /// <returns>A tuple of No A presses, No B presses and cost. -1's returns if no solution.</returns>
        public (double, double, double) WinPrize(long offset)
        {
            var (a, c) = (ButtonA.Item1, ButtonA.Item2);
            var (b, d) = (ButtonB.Item1, ButtonB.Item2);
            var (e, f) = (Prize.Item1 + offset, Prize.Item2 + offset);
            var det = (double) (a * d - b * c);
            double an = (e * d - b * f) / (double)det;
            double bn = (a * f - c * e) / (double)det;
            // Check all these condituons carefully!
            var validAnswer = (an - (long)an == 0) && (bn - (long)bn == 0) && an > 0 && bn > 0;
            return validAnswer ? (an, bn, 3 * an + bn) : (-1, -1, -1);            
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>Claw machine as a string.</returns>
        public override string ToString()
        {
            return $"A: {ButtonA}, B: {ButtonB}, Prize: {Prize}";
        }
    }
}