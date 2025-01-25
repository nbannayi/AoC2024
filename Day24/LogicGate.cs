using System;
using System.Collections.Generic;

namespace Day24
{
    public enum Operation
    {
        AND,
        OR,
        XOR
    }   

    public class LogicGate
    {
        public Operation Operation { get; set; }
        public string InputLabel1 { get; set; }
        public string InputLabel2 { get; set; }
        public string OutputLabel { get; set; }

        /// <summary>
        /// Represents a logic gate.
        /// </summary>
        /// <param name="inputLabel1">Input label 1.</param>
        /// <param name="inputLabel2">Input label 2.</param>
        /// <param name="operation">Type (AND, OR, XOR.)</param>
        /// <param name="outputLabel">Input label 2.</param>
        public LogicGate(string inputLabel1, string inputLabel2, Operation operation, string outputLabel)
        {
            InputLabel1 = inputLabel1;
            InputLabel2 = inputLabel2;
            Operation = operation;
            OutputLabel = outputLabel;
        }

        /// <summary>
        /// Create an exact clone of this object.
        /// </summary>
        /// <returns>Clone of object.</returns>
        public LogicGate Clone()
        {
            return new LogicGate(InputLabel1, InputLabel2, Operation, OutputLabel);
        }

        /// <summary>
        /// Evaluate the logic gate based on passed dictionary of inputs.
        /// </summary>
        /// <param name="inputs">Set of avaulabke inputs.</param>
        /// <param name="result">Result if it can be evaluated.</param>
        /// <returns>True if can be evluated based on inputs, false otherwise.</returns>
        public bool Evaluate(Dictionary<string, bool> inputs, out bool result)
        {
            // Exit if can't evaluate yet.
            if (!inputs.ContainsKey(InputLabel1) || !inputs.ContainsKey(InputLabel2))
            {
                result = false;
                return false;
            }
            else
            {
                var (input1, input2) = (inputs[InputLabel1], inputs[InputLabel2]);
                result = Operation switch
                {
                    Operation.AND => input1 && input2,
                    Operation.OR => input1 || input2,
                    Operation.XOR => input1 ^ input2,
                    _ => false
                };
            }
            return true;
        }

        /// <summary>
        /// Represent for keying.
        /// </summary>
        /// <returns>Unique strign representation.</returns>
        public override string ToString()
        {
            return $"{InputLabel1} {Operation} {InputLabel2} -> {OutputLabel}";
        }        
    }
}