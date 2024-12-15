using System.Collections.Immutable;
using System.ComponentModel;
using System.Numerics;
using AdventOfCode.Helpers;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

var solution = new SolutionDay15("input.txt");
solution.PrintSolutions();

public class SolutionDay15 : AbstractSolution
{
    static Complex Up = -Complex.ImaginaryOne;
    static Complex Down = Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;

    public SolutionDay15() : base() { }
    public SolutionDay15(string filepath) : base(filepath) { }

    public override long Part1()
    {
        return (long)Solve(_input);
    }
    public override long Part2()
    {
        return (long)Solve(ScaleUp(_input));
    }

    public double Solve(string input)
    {
        var (map, steps) = Parse(input);

        var robot = map.Keys.Single(k => map[k] == '@');
        foreach (var dir in steps)
        {
            if (TryStep(ref map, robot, dir))
            {
                robot += dir;
            }
        }

        return map.Keys
            .Where(k => map[k] == '[' || map[k] == 'O')
            .Sum(box => box.Real + 100 * box.Imaginary);
    }

    string ScaleUp(string input) =>
      input.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.");


    public bool TryStep(ref Map map, Complex position, Complex step)
    {
        var mapOrig = map;

        if (map[position] == '.')
        {
            return true;
        }
        else if (map[position] == 'O' || map[position] == '@')
        {
            if (TryStep(ref map, position + step, step))
            {
                map[position + step] = map[position];
                map[position] = '.';
                return true;
            }
        }
        else if (map[position] == ']')
        {
            return TryStep(ref map, position + Left, step);
        }
        else if (map[position] == '[')
        {
            if (step == Left)
            {
                if (TryStep(ref map, position + Left, step))
                {
                    map[position + Left] = '[';
                    map[position] = ']';
                    map[position + Right] = '.';
                    return true;
                }
            }
            else if (step == Right)
            {
                if (TryStep(ref map, position + 2 * Right, step))
                {
                    map[position + 2 * Right] = ']';
                    map[position + Right] = '[';
                    map[position] = '.';
                    return true;
                }
            }
            else
            {
                if (TryStep(ref map, position + step, step) && TryStep(ref map, position + Right + step, step))
                {
                    map[position + step] = '[';
                    map[position + step + Right] = ']';
                    map[position + Right] = '.';
                    map[position] = '.';
                    return true;
                }
            }
        }
        map = mapOrig;
        return false;
    }

    (Map, Complex[]) Parse(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);
        var lines = blocks[0].Split(Environment.NewLine);
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToDictionary();

        var steps = blocks[1].ReplaceLineEndings("").Select(ch =>
            ch switch
            {
                '^' => Up,
                '<' => Left,
                '>' => Right,
                'v' => Down,
                _ => throw new Exception()
            });

        return (map, steps.ToArray());
    }
}

