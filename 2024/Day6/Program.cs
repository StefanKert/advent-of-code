using System.Collections.Immutable;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

Complex Up = Complex.ImaginaryOne;
Complex TurnRight = -Complex.ImaginaryOne;

var demoMap = GetMap(File.ReadAllText("input.txt"));

Console.WriteLine($"Part 1: Steps walked '{Walk(demoMap.map, demoMap.start).positions.Count()}'");
Console.WriteLine($"Part 2: Steps walked '{GetLoops(demoMap.map, demoMap.start)}'");

(IEnumerable<Complex> positions, bool isLoop) Walk(Map map, Complex warden)
{
    var seen = new HashSet<(Complex pos, Complex dir)>();
    var dir = Up;
    while (map.ContainsKey(warden) && !seen.Contains((warden, dir)))
    {
        seen.Add((warden, dir));
        if (map.GetValueOrDefault(warden + dir) == '#')
        {
            dir *= TurnRight;
        }
        else
        {
            warden += dir;
        }
    }
    return (
        positions: seen.Select(s => s.pos).Distinct(),
        isLoop: seen.Contains((warden, dir))
    );
}

long GetLoops(Map map, Complex warden)
{
    var positions = Walk(map, warden).positions;
    var loops = 0;
    foreach (var block in positions.Where(pos => map[pos] == '.'))
    {
        map[block] = '#';
        if (Walk(map, warden).isLoop)
        {
            loops++;
        }
        map[block] = '.';
    }
    return loops;
}

(Map map, Complex start) GetMap(string input)
{
    var map = input.Split(Environment.NewLine);
    var dataMap = (
        from y in Enumerable.Range(0, map.Length)
        from x in Enumerable.Range(0, map[0].Length)
        select new KeyValuePair<Complex, char>(-Up * y + x, map[y][x])
    ).ToDictionary();
    return (dataMap, dataMap.Single(kvp => kvp.Value == '^').Key);
}