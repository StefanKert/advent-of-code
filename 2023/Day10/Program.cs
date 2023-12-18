using Spectre.Console;
using System.Numerics;
using System.Text;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

var map = ParseMap(File.ReadAllText("input.txt"));
var loop = LoopPositions(map);
var data = map.Keys.Count(position => Inside(position, map, loop));

AnsiConsole.WriteLine(GetLayout(map, loop));
File.WriteAllText("layout.txt", GetLayout(map, loop));
AnsiConsole.WriteLine("Part 1: " + loop.Count / 2);
AnsiConsole.WriteLine("Part 2: " + data);

static string GetLayout(Map map, HashSet<Complex> loop)
{
    return map.Select(x => x.Key).Aggregate(new StringBuilder(), (sb, pos) =>
    {
        if (Inside(pos, map, loop))
        {
            sb.Append("I");
        }
        else if (loop.Contains(pos))
        {
            sb.Append("#");
        }
        else
        {
            sb.Append(map[pos]);
        }
        if (pos.Real == map.Keys.Max(p => p.Real))
        {
            sb.AppendLine();
        }
        return sb;
    }).ToString();
}

// why not 501
static HashSet<Complex> LoopPositions(Map map)
{
    var position = map.Keys.Single(k => map[k] == 'S');
    var positions = new HashSet<Complex>();

    var dir = Constants.Dirs.First(dir => Constants.Exits[map[position + dir]].Contains(-dir));
    for (; ; )
    {
        positions.Add(position);
        position += dir;
        if (map[position] == 'S')
        {
            break;
        }
        dir = Constants.Exits[map[position]].Single(exit => exit != -dir);
    }
    return positions;
}

static Map ParseMap(string input)
{
    var rows = input.Split("\n");
    return (
        from irow in Enumerable.Range(0, rows.Length)
        from icol in Enumerable.Range(0, rows[0].Length)
        let pos = new Complex(icol, irow)
        let cell = rows[irow][icol]
        select new KeyValuePair<Complex, char>(pos, cell)
    ).ToDictionary();
}

static bool Inside(Complex position, Map map, HashSet<Complex> loop)
{
    if (loop.Contains(position))
    {
        return false;
    }

    var containsLoopElement = false;
    var tempPosition = position;
    while (map.ContainsKey(tempPosition))
    {
        if (loop.Contains(tempPosition))
        {
            containsLoopElement = true;
        }
        tempPosition += Constants.Right;
    }

    if (!containsLoopElement)
    {
        return false;
    }

    var inside = false;
    position += Constants.Left;
    while (map.ContainsKey(position))
    {
        //
        // a vertical bar of the loop
        // a NW loop corner if the previous was a SE loop corner
        //a SW loop corner if the previous was a NE loop corner
        //
        if (loop.Contains(position))
        {
            if (map[position] == '|')
            {
                inside = !inside;
            }
            else if (map[position] == 'L')
            {
                inside = !inside;
            }
            else if (map[position] == 'J')
            {
                inside = !inside;
            }
        }
        position += Constants.Left;
    }
    return inside;
}

public static class Constants
{
    public static Complex Up = -Complex.ImaginaryOne;
    public static Complex Down = Complex.ImaginaryOne;
    public static Complex Left = -Complex.One;
    public static Complex Right = Complex.One;
    public static Complex[] Dirs = [Up, Right, Down, Left];
    public static Dictionary<char, Complex[]> Exits = new Dictionary<char, Complex[]>{
        {'7', [Left, Down] },
        {'F', [Right, Down]},
        {'L', [Up, Right]},
        {'J', [Up, Left]},
        {'|', [Up, Down]},
        {'-', [Left, Right]},
        {'S', [Up, Down, Left, Right]},
        {'.', []},
    };
}