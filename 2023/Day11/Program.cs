using System.Net;
using System.Numerics;
using System.Text;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, (char value, int distanceX, int distanceY)>;

var map = ParseMap(File.ReadAllText("input.txt"), 1000000 - 1);
var galaxies = map.Where(c => char.IsDigit(c.Value.value)).Distinct().ToList();
// Find all distinct pairs of galaxies
var pairs = galaxies.SelectMany((g1, i) => galaxies.Skip(i + 1).Select(g2 => (g1, g2))).ToList();
var distance = 0L;
foreach (var pair in pairs)
{
    var xDistances = 0L;
    if (pair.g1.Key.Real > pair.g2.Key.Real)
    {
        var elements = map.Where(x => x.Key.Imaginary == pair.g1.Key.Imaginary && x.Value.distanceX > 0).ToList();
        xDistances = elements.Where(x => x.Key.Real < pair.g1.Key.Real && x.Key.Real > pair.g2.Key.Real).Sum(x => x.Value.distanceX);
    }
    else
    {
        var elements = map.Where(x => x.Key.Imaginary == pair.g1.Key.Imaginary && x.Value.distanceX > 0).ToList();
        xDistances = elements.Where(x => x.Key.Real > pair.g1.Key.Real && x.Key.Real < pair.g2.Key.Real).Sum(x => x.Value.distanceX);
    }

    var yDistances = 0L;
    if (pair.g1.Key.Imaginary > pair.g2.Key.Imaginary)
    {
        var elements = map.Where(x => x.Key.Real == pair.g1.Key.Real && x.Value.distanceY > 0);
        yDistances = elements.Where(x => x.Key.Imaginary < pair.g1.Key.Imaginary && x.Key.Imaginary > pair.g2.Key.Imaginary).Sum(x => x.Value.distanceY);
    }
    else
    {
        var elements = map.Where(x => x.Key.Real == pair.g1.Key.Real && x.Value.distanceY > 0);
        yDistances = elements.Where(x => x.Key.Imaginary > pair.g1.Key.Imaginary && x.Key.Imaginary < pair.g2.Key.Imaginary).Sum(x => x.Value.distanceY);
    }
    distance += Math.Abs((long)pair.g1.Key.Real - (long)pair.g2.Key.Real) + Math.Abs((long)pair.g1.Key.Imaginary - (long)pair.g2.Key.Imaginary) + xDistances + yDistances;
}
//Console.WriteLine(GetLayout(map));
System.Console.WriteLine("Distance: " + distance);

static string GetLayout(Map map)
{
    var sb = new StringBuilder();
    for (int irow = 0; irow <= map.Keys.Max(x => x.Imaginary); irow++)
    {
        for (int icol = 0; icol <= map.Keys.Where(x => x.Imaginary == irow).Max(x => x.Real); icol++)
        {
            if (map[new Complex(icol, irow)].distanceX > 0 || map[new Complex(icol, irow)].distanceY > 0)
            {
                sb.Append("Ä");
            }
            else
            {
                sb.Append(map[new Complex(icol, irow)].value);
            }
        }
        sb.AppendLine();
    }
    return sb.ToString();
}

static Map ParseMap(string input, int emptyDistance)
{
    var galaxy = 1;
    var rows = input.Split("\n");
    var data = new List<KeyValuePair<Complex, (char value, int distanceX, int distanceY)>>();
    foreach (var irow in Enumerable.Range(0, rows.Length))
    {
        var distanceY = 0;
        if (IsRowEmpty(rows, irow))
        {
            distanceY = emptyDistance;
        }

        foreach (var icol in Enumerable.Range(0, rows[0].Length))
        {
            var distanceX = 0;
            if (IsColumnEmpty(rows, icol))
            {
                distanceX = emptyDistance;
            }

            var pos = new Complex(icol, irow);
            var cell = rows[irow][icol];
            if (cell == '#')
            {
                data.Add(new KeyValuePair<Complex, (char value, int distanceX, int distanceY)>(pos, (galaxy.ToString()[0], 0, 0)));
                galaxy++;
            }
            else
            {
                data.Add(new KeyValuePair<Complex, (char value, int distanceX, int distanceY)>(pos, (cell, distanceX, distanceY)));
            }
        }
    }
    return data.ToDictionary();
}

static bool IsRowEmpty(string[] rows, int irow)
{
    var emptyRow = true;
    for (int icol = 0; icol < rows[irow].Length; icol++)
    {
        var cell = rows[irow][icol];
        if (cell != '.')
        {
            emptyRow = false;
        }
    }
    return emptyRow;
}

static bool IsColumnEmpty(string[] rows, int icol)
{
    var emptyRow = true;
    for (int irow = 0; irow < rows.Length; irow++)
    {
        var pos = new Complex(icol, irow);
        var cell = rows[irow][icol];
        if (cell != '.')
        {
            emptyRow = false;
        }
    }
    return emptyRow;
}