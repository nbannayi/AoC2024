using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 24: Crossed Wires.
namespace Day24
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse raw inputs.
            var fileName = "Day24Input.txt";
            var (inputsDict, gatesDict) = ParseInputs(fileName);

            // Process it all.
            var processedInputs = ProcessLogicGates(inputsDict, gatesDict);

            // Part 1.
            Console.WriteLine($"Part 1 answer: {ConvertResultBoolsToInt(processedInputs)}");

            // Part 2.
            Console.WriteLine($"Part 2 answer: TODO");
        }

        // Convert a sequence of boolean values representing a binary string to a number.
        static long ConvertResultBoolsToInt(Dictionary<string, bool> processedInputs)
        {
            var kvpZList = processedInputs.
                Where(kvp => kvp.Key.StartsWith('z')).
                OrderByDescending(kvp => kvp.Key).                
                ToList();
            var resultBinaryString = "";
            foreach (var kvpZ in kvpZList)            
                resultBinaryString += kvpZ.Value ? "1" : "0";
            return Convert.ToInt64(resultBinaryString, 2);
        }

        // Process all logic gates from inputs and gates dicts.
        static Dictionary<string, bool> ProcessLogicGates(
            Dictionary<string, bool> inputsDict,
            Dictionary<string, LogicGate> gatesDict)
        {
            var keysToRemove = new List<string>();            
            while (gatesDict.Count > 0)
            {
                keysToRemove.Clear();
                foreach (var kvp in gatesDict)
                {
                    var gate = kvp.Value;
                    if (gate.Evaluate(inputsDict, out bool result))
                    {
                        keysToRemove.Add(gate.ToString());
                        inputsDict.Add(gate.OutputLabel, result);
                    }
                }                
                foreach (var key in keysToRemove)
                    gatesDict.Remove(key);
            }
            return inputsDict;
        }
            
        // Parse everything.
        static (Dictionary<string, bool>, Dictionary<string, LogicGate>) ParseInputs(string inputFile)
        {
            var inputText = File.ReadAllText(inputFile);
            var tokens = inputText.Split("\n\n");
            inputText = tokens[0];
            var gatesText = tokens[1];
            var inputLines = inputText.Split("\n");
            var gateLines = gatesText.Split("\n");

            // Build input dictionary.
            var inputsDict = new Dictionary<string, bool>();
            foreach (var inputLine in inputLines)
            {
                tokens = inputLine.Split(": ");
                inputsDict.Add(tokens[0], tokens[1] == "1");
            }

            // Build gates dictionary.
            var gatesDict = new Dictionary<string, LogicGate>();
            foreach (var gateLine in gateLines)
            {
                tokens = gateLine.Split(" -> ");
                var gateTokens = tokens[0].Split(" ");
                var inputLabel1 = gateTokens[0];
                var inputLabel2 = gateTokens[2];
                var operation = gateTokens[1] switch
                {
                    "AND" => Operation.AND,
                    "OR" => Operation.OR,
                    "XOR" => Operation.XOR,
                    _ => throw new Exception("Operation not known.")
                };
                var outputLabel = tokens[1];
                var logicGate = new LogicGate(inputLabel1, inputLabel2, operation, outputLabel);
                gatesDict.Add(logicGate.ToString(), logicGate);
            }

            return (inputsDict, gatesDict);
        }
    }
}