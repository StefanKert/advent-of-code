using System.Data;
using Map = char[][];

Console.WriteLine("Solution 1: {0}", Measure(Tilt(Parse(File.ReadAllText("input.txt")))));
Console.WriteLine("Solution 2: {0}", Measure(Iterate(Parse(File.ReadAllText("input.txt")), 1_000_000_000)));

static Map Iterate(Map map, int count)
{
    var history = new List<string>();
    while (count > 0)
    {
        for (var i = 0; i < 4; i++)
        {
            map = Turn90DegreeClockwise(Tilt(map));
        }
        count--;

        var mapString = string.Join("\n", map.Select(l => new string(l)));
        var idx = history.IndexOf(mapString);
        if (idx < 0)
        {
            history.Add(mapString);
        }
        else
        {
            var loopLength = history.Count - idx;
            var remainder = count % loopLength;
            return Parse(history[idx + remainder]);
        }
    }
    return map;
}

static Map Tilt(Map map)
{
    for (int icol = 0; icol < map[0].Length; icol++)
    {
        var irowT = 0;
        for (int irow = 0; irow < map.Length; irow++)
        {
            if (map[irow][icol] == '#')
            {
                irowT = irow + 1;
                continue;
            }

            if (map[irow][icol] == 'O')
            {
                map[irow][icol] = '.';
                map[irowT][icol] = 'O';
                irowT++;
            }
        }
    }
    return map;
}

static Map Turn90DegreeClockwise(Map src)
{
    var dst = new char[src.Length][];
    for (var irow = 0; irow < src[0].Length; irow++)
    {
        dst[irow] = new char[src[0].Length];
        for (var icol = 0; icol < src.Length; icol++)
        {
            dst[irow][icol] = src[src.Length - icol - 1][irow];
        }
    }
    return dst;
}

static int Measure(Map map) => map.Select((row, irow) => (map.Length - irow) * row.Count(ch => ch == 'O')).Sum();

static Map Parse(string input) => input.Split('\n').Select(x => x.ToCharArray()).ToArray(); 