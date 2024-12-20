using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Day 5: Print Queue.
namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse everything into lists of rules and updates.
            var rulesInput = File.ReadAllText("Day05Input.txt");            
            var rulesInputSections = rulesInput.Split("\n\n");

            var rules = rulesInputSections[0].Split("\n").Select(r => (r.Split("|")[0], r.Split("|")[1])).ToList();
            var updates = rulesInputSections[1].Split("\n").Select(u => u.Split(",")).ToList();

            // Now process the whole thing.
            var centralTotal = 0;
            var centralCorrectedTotal = 0;
            var incorrectlyOrderedUpdates = new List<string[]>();

            foreach (var update in updates)
            {
                if (IsUpdateValid(update, rules))
                    centralTotal += int.Parse(GetCentreUpdate(update));
                else
                    incorrectlyOrderedUpdates.Add(update);
            }

            foreach (var update in incorrectlyOrderedUpdates)
            {
                var correctedUpdate = correctUpdate(update, rules);
                centralCorrectedTotal += int.Parse(GetCentreUpdate(correctedUpdate));
            }

            Console.WriteLine($"Part 1 answer: {centralTotal}");
            Console.WriteLine($"Part 2 answer: {centralCorrectedTotal}");
        }

        // For a given update set determine all rules that would make it illegal.
        static List<(string, string)> GetDisablingRules(string[] updates)
        {
            var illegalPairs = new List<(string, string)>();
            for (int i = 0; i < updates.Length; i++)
            {
                for (int j = 0; j < updates.Length; j++)
                {
                    if (i == j)
                        continue;
                    else if (i < j)
                        illegalPairs.Add((updates[j], updates[i]));
                    else
                        illegalPairs.Add((updates[i], updates[j]));
                }
            }
            return illegalPairs;
        }

        // Returns true if any of the passed search rules are in rules.
        static bool ContainsRules(List<(string,string)> rules, List<(string, string)> searchRules)
        {
            return rules.Intersect(searchRules).Count() != 0;
        }

        // Returns true if all given updates are valid based on passed rules.
        static bool IsUpdateValid(string[] updates, List<(string, string)> rules)
        {
            var disablingRules = GetDisablingRules(updates);
            return !ContainsRules(rules, disablingRules);
        }

        // Get update in the centre of array.
        static string GetCentreUpdate(string[] updates)
        {
            var pos = (updates.Length - 1) / 2;
            return updates[pos];
        }

        // Takes an incorrect update and applies the rules to correctly order it, then returns this.
        static string[] correctUpdate(string[] update, List<(string,string)> rules)
        {            
            while (!IsUpdateValid(update, rules))
            {
                foreach (var rule in rules)
                {
                    var i = Array.FindIndex(update, u => u == rule.Item1);
                    var j = Array.FindIndex(update, u => u == rule.Item2);
                    if (i > j && i >= 0 && j >= 0)
                    {
                        var page = update[j];
                        var updateList = update.ToList();
                        updateList.Insert(i+1, page);
                        updateList.RemoveAt(j);
                        update = updateList.ToArray();
                    }
                }
            }
            return update;
        }
    }
}