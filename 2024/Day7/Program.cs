using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var solution = new SolutionDay7();
solution.PrintSolutions();

public class SolutionDay7 : AbstractSolution
{
    private List<Test> rules;

    public SolutionDay7()
    {
        var input = File.ReadAllText("input.txt");
        rules = ParseRules(input);
    }

    public override long Part1()
    {
        return GetSum(['+', '*']);
    }

    public override long Part2()
    {
        return GetSum(['+', '*']);
    }


    long GetSum(List<char> chars)
    {
        var longestRule = rules.MaxBy(x => x.numbers.Count).numbers.Count;
        var permutationDictionary = new Dictionary<int, List<string>>();
        for (int i = 0; i < longestRule; i++)
        {
            permutationDictionary[i + 1] = GeneratePermutationsWithRepetition(chars.ToArray(), i + 1).ToList();
        }

        var sum = 0L;
        foreach (var rule in rules)
        {
            var expectedResult = rule.result;
            foreach (var r in permutationDictionary[rule.numbers.Count])
            {
                var result = rule.numbers[0];
                for (int i = 1; i < r.Length; i++)
                {
                    if (r[i] == '*')
                    {
                        result *= rule.numbers[i];
                    }
                    else if (r[i] == '|')
                    {
                        result = Concat(result, rule.numbers[i]);
                    }
                    else
                    {
                        result += rule.numbers[i];
                    }
                    if (result > expectedResult)
                    {
                        break;
                    }
                }
                if (result == expectedResult)
                {
                    sum += result;
                    break;
                }
                if (result > expectedResult)
                {
                    break;
                }
            }
        }
        return sum;
    }

    static long Concat(long left, long right)
    {
        int rightLength = 10;
        while (rightLength <= right)
        {
            rightLength *= 10;
        }
        return (left * rightLength) + right;
    }

    List<string> GeneratePermutationsWithRepetition(char[] characters, int length)
    {
        var results = new List<string>();
        GeneratePermutationsRecursive(characters, "", length, results);
        return results;
    }

    void GeneratePermutationsRecursive(char[] characters, string current, int length, List<string> results)
    {
        if (current.Length == length)
        {
            results.Add(current);
            return;
        }

        foreach (var character in characters)
        {
            GeneratePermutationsRecursive(characters, current + character, length, results);
        }
    }

    List<Test> ParseRules(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var testList = new List<Test>();
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            var result = long.Parse(parts[0]);
            var numbers = new List<long>();
            foreach (var number in parts[1].Split(' '))
            {
                if (number == string.Empty)
                    continue;

                numbers.Add(long.Parse(number));
            }
            testList.Add(new Test(result, numbers));
        }
        return testList;
    }



    public record Test(long result, List<long> numbers);
}