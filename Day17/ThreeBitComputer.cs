using System;
using System.Collections.Generic;
using System.Linq;

namespace Day17
{
    public class ThreeBitComputer
    {
        public enum Instruction
        {
            ADV,
            BXL,
            BST,
            JNZ,
            BXC,
            OUT,
            BDV,
            CDV,
            NUL
        }

        // Points to next instruction to run.
        private int _instructionPointer;
        public int InstructionPointer { get { return _instructionPointer; } }

        // Registers.
        public long A { get; set; }
        public long B { get; set; }
        public long C { get; set; }

        // Program stored on the computer.
        public List<int> Program { get; set; }

        // Program output.
        public List<long> Output { get; set; }

        /// <summary>
        /// Represents a 3 Bit computer (AoC style.)
        /// </summary>
        public ThreeBitComputer(long a, long b, long c)
        {
            // Empty to begin with.
            Program = new List<int>();
            Output = new List<long>();

            _instructionPointer = 0;
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Load a provided program into the computer.
        /// </summary>
        /// <param name="program">Program is supplied as a comma delimited string of instructions e.g. "5,0,5,1,5,4"</param>
        public void LoadProgram(string program)
        {
            Program =
                program.Split(',').
                Select(pc => int.Parse(pc)).
                ToList();
            Output.Clear();
        }

        /// <summary>
        /// Reset the computer but keep the program in memory.
        /// </summary>
        public void Reset()
        {
            A = 0;
            B = 0;
            C = 0;
            _instructionPointer = 0;
            Output.Clear();
        }

        /// <summary>
        /// Display contents of all registers.
        /// </summary>
        public void DisplayRegisters()
        {
            Console.WriteLine($"Register A: {A}, ({Convert.ToString(A, 2)})");
            Console.WriteLine($"Register B: {B}, ({Convert.ToString(B, 2)})");
            Console.WriteLine($"Register C: {C}, ({Convert.ToString(C, 2)})");
            Console.WriteLine($"Instruction Pointer: {_instructionPointer}");
        }

        /// <summary>
        /// Display program.
        /// </summary>
        /// <param name="convertToMnemonics">If true converts program to mnemonics.</param>
        public void DisplayProgram(bool convertToMnemonics)
        {
            var programStringList = new List<string>();
            for (var i = 0; i <= Program.Count - 2; i += 2)
            {
                var programOpCode = Program[i];
                var programOpCodeString = convertToMnemonics ?
                    ((Instruction)Enum.ToObject(typeof(Instruction), programOpCode)).ToString() :
                    programOpCode.ToString();
                var programOperandString = Program[i + 1].ToString();
                programStringList.Add(programOpCodeString);
                programStringList.Add(programOperandString);
            }
            var programString = string.Join(",", programStringList);
            Console.WriteLine($"Program: {programString}");
        }

        /// <summary>
        /// Display dump of entire memory.
        /// </summary>
        /// <param name="convertToMnemonics">If true converts program to mnemonics.</param>
        public void MemoryDump(bool convertToMnemonics)
        {
            DisplayRegisters();
            Console.WriteLine();
            DisplayProgram(convertToMnemonics);
            Console.WriteLine($"Output : {GetOutput()}");
        }

        /// <summary>
        /// Return output as a comma delimited string.
        /// </summary>
        /// <returns>Output ints separated by commas.</returns>
        public string GetOutput()
        {
            return Output.Count > 0 ?
                string.Join(",", Output) :
                "";
        }

        /// <summary>
        /// Run the currently loaded program.
        /// </summary>
        /// <param name="debug">Pass debug flag as true to see memory dump.</param>
        public void Run(bool debug)
        {
            if (debug) MemoryDump(false);
            while (_instructionPointer < Program.Count)
            {
                var instruction = (Instruction)Enum.
                    ToObject(typeof(Instruction), Program[_instructionPointer]);
                var operand = Program[_instructionPointer + 1];

                switch (instruction)
                {
                    case Instruction.ADV:
                        var comboOperand = GetComboOperand(operand);
                        A /= (long)Math.Pow(2.0, comboOperand);
                        _instructionPointer += 2;
                        break;
                    case Instruction.BXL:
                        B ^= operand;
                        _instructionPointer += 2;
                        break;
                    case Instruction.BST:
                        comboOperand = GetComboOperand(operand);
                        B = comboOperand % 8;
                        _instructionPointer += 2;
                        break;
                    case Instruction.JNZ:
                        if (A != 0)
                            _instructionPointer = operand;
                        else
                            _instructionPointer += 2;
                        break;
                    case Instruction.BXC:
                        B ^= C;
                        _instructionPointer += 2;
                        break;
                    case Instruction.OUT:
                        comboOperand = GetComboOperand(operand);
                        var result = comboOperand % 8;
                        Output.Add(result);
                        _instructionPointer += 2;
                        break;
                    case Instruction.BDV:
                        comboOperand = GetComboOperand(operand);
                        B = A / (long)Math.Pow(2.0, comboOperand);
                        _instructionPointer += 2;
                        break;
                    case Instruction.CDV:
                        comboOperand = GetComboOperand(operand);
                        C = A / (long)Math.Pow(2.0, comboOperand);
                        _instructionPointer += 2;
                        break;
                }
                if (debug)
                {
                    Console.Clear();
                    Console.WriteLine();
                    MemoryDump(true);
                    Console.WriteLine($"Output : {GetOutput()}");
                }
            }

            // Get operand for combo instruction mode.
            long GetComboOperand(long operand)
            {
                long comboOperand = operand;
                switch (operand)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        comboOperand = operand;
                        break;
                    case 4:
                        comboOperand = A;
                        break;
                    case 5:
                        comboOperand = B;
                        break;
                    case 6:
                        comboOperand = C;
                        break;
                    case 7:
                        throw new Exception("Combo operand 7 is reserved and should not appear in valid programs.");
                }
                return comboOperand;
            }
        }

        /// <summary>
        /// Get quine for part 2.
        /// </summary>
        /// <returns>Long value of A register that produces quine.</returns>
        public long GetQuineARegister()
        {
            // Return the minimum solution for a quine.
            var allSolutions = GetQuine(0, 0);
            return allSolutions.Min();

            // Perform recursive DFS to search for quine.
            List<long> GetQuine(long current, int depth)
            {
                var allSolutions = new List<long>();
                if (depth > Program.Count - 1) return allSolutions;
                var a = current * 8;
                for (int i = 0; i < 8; i++)
                {
                    Reset();
                    A = a + i;
                    Run(false);
                    if (CheckOutput(depth + 1))
                    {
                        if (depth + 1 == Program.Count) allSolutions.Add(a + i);
                        allSolutions.AddRange(GetQuine(a + i, depth + 1));
                    }
                }
                return allSolutions;
            }

            // Check last 'n' values of program match output.
            bool CheckOutput(int n)
            {
                var programLen = Program.Count;
                var outputLen = Output.Count;
                var matched = true;
                for (var i = 0; i < n; i++)
                {
                    if (Output[outputLen - i - 1] != Program[programLen - i - 1])
                    {
                        matched = false;
                        break;
                    }
                }
                return matched;
            }
        }
    }
}