using Rules = System.Collections.Generic.Dictionary<string, string>;
using Cube = System.Collections.Immutable.ImmutableArray<Range>;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Collections.Immutable;



var input = File.ReadAllText("demo.txt");
var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
var rules = ParseRules(parts[0]);
Console.WriteLine((
    from cube in ParseUnitCube(parts[1])
    where AcceptedVolume(rules, cube) == 1
    select cube.Select(r => r.begin).Sum()
).Sum());

static BigInteger AcceptedVolume(Rules rules, Cube cube)
{
    var q = new Queue<(Cube cube, string state)>();
    q.Enqueue((cube, "in"));

    BigInteger res = 0;
    while (q.Any())
    {
        (cube, var state) = q.Dequeue();
        if (cube.Any(coord => coord.end < coord.begin))
        {
            continue; // cube is empty
        }
        else if (state == "R")
        {
            continue; // cube is rejected
        }
        else if (state == "A")
        {
            res += Volume(cube); // cube is accepted
        }
        else
        {
            foreach (var stm in rules[state].Split(","))
            {
                Cond cond = TryParseCond(stm);
                if (cond == null)
                {
                    q.Enqueue((cube, stm));
                }
                else if (cond.op == '<')
                {
                    var (cube1, cube2) = CutCube(cube, cond.dim, cond.num - 1);
                    q.Enqueue((cube1, cond.state));
                    cube = cube2;
                }
                else if (cond?.op == '>')
                {
                    var (cube1, cube2) = CutCube(cube, cond.dim, cond.num);
                    cube = cube1;
                    q.Enqueue((cube2, cond.state));
                }
            }
        }
    }
    return res;
}

static BigInteger Volume(Cube cube) =>
    cube.Aggregate(BigInteger.One, (m, r) => m * (r.end - r.begin + 1));

// Cuts a cube along the specified dimension, other dimensions are unaffected.
static (Cube lo, Cube hi) CutCube(Cube cube, int dim, int num)
{
    var r = cube[dim];
    return (
        cube.SetItem(dim, r with { end = Math.Min(num, r.end) }),
        cube.SetItem(dim, r with { begin = Math.Max(r.begin, num + 1) })
    );
}

static Cond TryParseCond(string st) =>
    st.Split('<', '>', ':') switch
    {
    ["x", var num, var state] => new Cond(0, st[1], int.Parse(num), state),
    ["m", var num, var state] => new Cond(1, st[1], int.Parse(num), state),
    ["a", var num, var state] => new Cond(2, st[1], int.Parse(num), state),
    ["s", var num, var state] => new Cond(3, st[1], int.Parse(num), state),
        _ => null
    };

static Rules ParseRules(string input) => (
    from line in input.Split(Environment.NewLine)
    let parts = line.Split('{', '}')
    select new KeyValuePair<string, string>(parts[0], parts[1])
).ToDictionary();

static IEnumerable<Cube> ParseUnitCube(string input) =>
    from line in input.Split(Environment.NewLine)
    let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value))
    select nums.Select(n => new Range(n, n)).ToImmutableArray();

record Range(int begin, int end);
record Cond(int dim, char op, int num, string state);