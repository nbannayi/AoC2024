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
            // Parse raw inputs and create adder
            var fileName = "Day24Input.txt";
            var (inputsDict, gatesDict) = ParseInputs(fileName);            
            var adder = new Adder(inputsDict, gatesDict);

            // Part 1.
            Console.WriteLine($"Part 1 answer: {adder.GetResult()}");

            // Part 2.  Had to get hints from Reddit for this one, settled on trying to work
            // out the invalid gates as brute force not feasible, turns out this is a 45-bit ripple carry adder. 
            Console.WriteLine($"Part 2 answer: {GetInvalidGates(gatesDict)}");
        }

        // Work out all invalid gates using rules of how a ripple adder should work.
        static string GetInvalidGates(Dictionary<string, LogicGate> gates)
        {
            var gatesList = gates.Select(kvp => kvp.Value).ToList();

            var invalidGates1 =
                gatesList.Where(g => g.OutputLabel.StartsWith('z') &&
                g.Operation != Operation.XOR &&
                g.OutputLabel != "z45").
                ToList();

            var invalidGates2 =
                gatesList.Where(g => !g.OutputLabel.StartsWith('z') &&
                !g.InputLabel1.StartsWith('x') && !g.InputLabel2.StartsWith('x') &&
                !g.InputLabel1.StartsWith('y') && !g.InputLabel2.StartsWith('y') &&
                g.Operation == Operation.XOR).
                ToList();

            var invalidGates3 =
                gatesList.Where(g => (g.InputLabel1.StartsWith('x') || g.InputLabel2.StartsWith('x') ||
                g.InputLabel1.StartsWith('y') || g.InputLabel2.StartsWith('y')) &&
                g.Operation == Operation.XOR &&
                !gatesList.Any(g2 => (g2.InputLabel1 == g.OutputLabel || g2.InputLabel2 == g.OutputLabel) && g2.Operation == Operation.XOR))
                .Where(g => !g.InputLabel1.Contains("00")).
                ToList();

            var invalidGates4 =
                gatesList.Where(g => (g.InputLabel1.StartsWith('x') || g.InputLabel2.StartsWith('x') ||
                g.InputLabel1.StartsWith('y') || g.InputLabel2.StartsWith('y')) &&
                g.Operation == Operation.AND &&
                !gatesList.Any(g2 => (g2.InputLabel1 == g.OutputLabel || g2.InputLabel2 == g.OutputLabel) && g2.Operation == Operation.OR))
                .Where(g => !g.InputLabel1.Contains("00")).
                ToList();

            var invalidGates = invalidGates1.
                Concat(invalidGates2).
                Concat(invalidGates3).
                Concat(invalidGates4).
                Distinct().
                OrderBy(ig => ig.OutputLabel);

            return string.Join(',', invalidGates.Select(g => g.OutputLabel));
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