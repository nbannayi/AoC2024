using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 7: Bridge Repair.
namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse all input.
            var equationLines = File.ReadAllLines("Day07Input.txt");
            var equations = new List<(long, long[])>();
            foreach (var el in equationLines)
            {
                var tokens = el.Split(": ");
                var target = long.Parse(tokens[0]);
                var operands = tokens[1].Split(" ").Select(o => long.Parse(o)).ToArray();
                equations.Add((target, operands));
            }
            
            long totalSolutions = 0;
            long totalSolutionsIncConcat = 0;
            foreach (var equation in equations)
            {
                long target = equation.Item1;
                long[] operands = equation.Item2;
                if (solutionExists(target, operands, false))
                    totalSolutions += target;
                if (solutionExists(target, operands, true))
                    totalSolutionsIncConcat += target;
            }

            Console.WriteLine($"Part 1 answer: {totalSolutions}");
            Console.WriteLine($"Part 2 answer: {totalSolutionsIncConcat}");
        }

        // Get all possible operators to use in a calc.
        static List<char[]> getOperatorCombinations(long[] operands, bool includeConcat)
        {
            var index = includeConcat ? 4 : 3;
            var n = operands.Length - 1;
            var operatorCombinations = new List<char[]>();            
            var queue = new Queue<string>();
            for (int i = 1; i < index; i++)
                queue.Enqueue(i.ToString());            
            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (current.Length == n)
                    operatorCombinations.Add(current.
                        Select(d => d == '1' ? '+' : (d == '2' ? '*' : '|')).ToArray());
                else
                {
                    for (int i = 1; i < index; i++)
                        queue.Enqueue(current + i);
                }
            }
            return operatorCombinations;
        }

        // Given a list of operators and operands interleave them to calculate a result.
        static long calcEquation(long[] operands, char[] operators)
        {
            long result = operands[0];
            var currentOperandIndex = 0;
            for (var i = 1; i < operands.Length; i++)
            {
                if (operators[currentOperandIndex] == '+')
                    result += operands[i];
                else if (operators[currentOperandIndex] == '*')
                    result *= operands[i];
                else // This will be '|' concat operator.
                    result = long.Parse(result.ToString() + operands[i].ToString());
                currentOperandIndex++;
            }
            return result;
        }

        // Returns true if a solution exists for some combo of operators.
        static bool solutionExists(long target, long[] operands, bool includeConcat)
        {
            var operatorCombinations = getOperatorCombinations(operands, includeConcat);
            foreach (var operators in operatorCombinations)
            {
                var result = calcEquation(operands, operators);
                if (result == target) return true;
            }
            return false;
        }
    }
}