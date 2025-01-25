using System;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    public class Adder
    {
        public Dictionary<string, bool> Inputs { get; set; }
        public Dictionary<string, LogicGate> Gates { get; set; }

        private long _x;        
        public long X { get { return _x; } }

        private long _y;
        public long Y { get { return _y; } }

        private long _targetResult;
        public long TargetResult { get { return _targetResult; } }

        /// <summary>
        /// Represents a simulation of an adder circuit.
        /// </summary>
        /// <param name="inputs">All inputs.</param>
        /// <param name="gates">Logic gates.</param>
        public Adder(Dictionary<string, bool> inputs, Dictionary<string, LogicGate> gates)
        {
            Inputs = inputs;
            Gates = gates;
            _x = GetResult('x');
            _y = GetResult('y');
            _targetResult = _x + _y;
        }

        // Evaluate logic gates (+2 overloads below.)
        private Dictionary<string, bool> Evaluate(
            Dictionary<string, bool> inputs,
            Dictionary<string, LogicGate> gates)
        {
            var keysToRemove = new List<string>();
            while (gates.Count > 0)
            {
                keysToRemove.Clear();
                foreach (var kvp in gates)
                {
                    var gate = kvp.Value;
                    if (gate.Evaluate(inputs, out bool result))
                    {
                        keysToRemove.Add(gate.ToString());
                        inputs.Add(gate.OutputLabel, result);
                    }
                }
                foreach (var key in keysToRemove)
                    gates.Remove(key);
            }
            return inputs;
        }

        /// <summary>
        /// Calculate result of processing adder.
        /// </summary>
        /// <returns>Dictionary of all outputs.</returns>
        public Dictionary<string, bool> Evaluate()
        {
            var inputs = Inputs.ToDictionary(entry => entry.Key, entry => entry.Value);
            var gates = Gates.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());
            return Evaluate(inputs, gates);
        }

        // Get result (+2 overloads below.)
        private long GetResult(char startChar, Dictionary<string, bool> inputs, Dictionary<string, LogicGate> gates)
        {
            var processedInputs = Evaluate(inputs, gates);
            var kvpZList = 
                processedInputs.
                Where(kvp => kvp.Key.StartsWith(startChar)).
                OrderByDescending(kvp => kvp.Key).
                ToList();
            var resultBinaryString = "";
            foreach (var kvpZ in kvpZList)
                resultBinaryString += kvpZ.Value ? "1" : "0";
            return Convert.ToInt64(resultBinaryString, 2);
        }

        /// <summary>
        /// Get result of evaluating adder.
        /// </summary>
        /// <param name="startChar">Start char (e.g. 'x')</param>
        /// <returns>Long result.</returns>
        public long GetResult(char startChar)
        {
            var inputs = Inputs.ToDictionary(entry => entry.Key, entry => entry.Value);
            var gates = Gates.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());
            return GetResult(startChar, inputs, gates);
        }

        /// <summary>
        /// Get result of evaluating adder.
        /// </summary>
        /// <returns>Long result.</returns>
        public long GetResult()
        {
            var inputs = Inputs.ToDictionary(entry => entry.Key, entry => entry.Value);
            var gates = Gates.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());
            return GetResult('z', inputs, gates);
        }

        /// <summary>
        /// Get result of evaluating adder with swaps.
        /// </summary>
        /// <param name="swaps">List of swaps.</param>
        /// <returns>Long swaps.</returns>
        public long GetResult(List<(string, string)> swaps)
        {
            var inputs = Inputs.ToDictionary(entry => entry.Key, entry => entry.Value);
            var gates = Gates.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());
            foreach (var swap in swaps)
            {
                var (id1, id2) = swap;
                var gate1 = gates[id1];
                var gate2 = gates[id2];
                var outputLabel1 = gate1.OutputLabel;
                var outputLabel2 = gate2.OutputLabel;
                gates.Remove(gate1.ToString());
                gates.Remove(gate2.ToString());
                gate1.OutputLabel = outputLabel2;
                gate2.OutputLabel = outputLabel1;
                gates.Add(gate1.ToString(), gate1);
                gates.Add(gate2.ToString(), gate2);
            }
            return GetResult('z', inputs, gates);
        }
    }
}