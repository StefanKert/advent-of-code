using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Helpers;

using Position = (long X, long Y);


var solution = new SolutionDay14("input.txt");
solution.PrintSolutions();

public class SolutionDay14 : AbstractSolution
{
    const int width = 101;
    const int height = 103;


    public SolutionDay14() : base() { }
    public SolutionDay14(string filepath) : base(filepath) { }

    public override long Part1()
    {
        var lastElement = Simulate(_input).ElementAt(100);
        var quadrants = new List<long>();
        var sum = 1;
        foreach (var robot in lastElement.GroupBy(GetQuadrant))
        {
            if (robot.Key.X == 0 || robot.Key.Y == 0)
                continue;
            sum *= robot.Count();
        }
        return sum;
    }

    public override long Part2()
    {
       return  Simulate(_input)
          .TakeWhile(robots => !Plot(robots).Contains("#################"))
          .Count();
    }

    IEnumerable<Robot[]> Simulate(string input)
    {
        var robots = Parse(input).ToArray();
        while (true)
        {
            yield return robots;
            robots = robots.Select(Step).ToArray();
        }
    }

    Robot Step(Robot robot) => robot with { Position = AddWithWrapAround(robot.Position, robot.Velocity) };

    Position GetQuadrant(Robot robot) =>
        new Position(Math.Sign(robot.Position.X - width / 2), Math.Sign(robot.Position.Y - height / 2));

    Position AddWithWrapAround(Position a, Position b) => new Position((a.X + b.X + width) % width, (a.Y + b.Y + height) % height);

    IEnumerable<Robot> Parse(string input) =>
           from line in input.Split("\n")
           let nums = Regex.Matches(line, @"-?\d+").Select(m => int.Parse(m.Value)).ToArray()
           select new Robot
           {
               Position = new Position(nums[0], nums[1]),
               Velocity = new Position(nums[2], nums[3])
           };

    string Plot(IEnumerable<Robot> robots)
    {
        var res = new int[height, width];
        foreach (var robot in robots)
        {
            res[robot.Position.Y, robot.Position.X]++;
        }
        var sb = new StringBuilder();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                sb.Append(res[y, x] == 0 ? "." : "#");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

public record struct Robot
{
    public Position Position { get; set; }
    public Position Velocity { get; set; }
}

