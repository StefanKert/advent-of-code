// Support both file input and WASM (stdin/env) input
string[] lines;
var aocInput = Environment.GetEnvironmentVariable("AOC_INPUT");
if (!string.IsNullOrEmpty(aocInput))
{
    lines = aocInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else if (Console.IsInputRedirected)
{
    lines = Console.In.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
}
else
{
    lines = File.ReadAllLines("input.txt");
}

var height = lines.Length;
var width = lines[0].Length;

var map = lines.SelectMany(line => line.ToCharArray().ToList()).ToList();
var startingPoint = map.IndexOf('S');
var memo = new Dictionary<(int x, int row), long>();

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(height, width, new List<char>(map), startingPoint));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(height, width, map, startingPoint));
}
else
{
    Console.WriteLine("Solution 1: " + Solution1(height, width, new List<char>(map), startingPoint));
    Console.WriteLine("Solution 2: " + Solution2(height, width, map, startingPoint));
}

int Index(int x, int y) => y * width + x;

bool InBounds(int x, int y, int width, int height) =>
    x >= 0 && y >= 0 && x < width && y < height;

long Solution1(int height, int width, List<char> map, int startingPoint)
{
    var currentRow = 1;
    var beams = new List<int>();
    map[Index(startingPoint, currentRow)] = '|';
    beams.Add(startingPoint);
    var splitCounter = 0;

    while (currentRow + 1 < height)
    {
        currentRow++;
        var nextBeams = new List<int>();
        foreach (var beam in beams)
        {
            if (map[Index(beam, currentRow)] == '.')
            {
                map[Index(beam, currentRow)] = '|';
                nextBeams.Add(beam);
            }
            else if (map[Index(beam, currentRow)] == '^')
            {
                if (InBounds(beam - 1, currentRow, width, height))
                {
                    map[Index(beam - 1, currentRow)] = '|';
                    nextBeams.Add(beam - 1);
                }

                if (InBounds(beam + 1, currentRow, width, height))
                {
                    map[Index(beam + 1, currentRow)] = '|';
                    nextBeams.Add(beam + 1);
                }
                splitCounter++;
            }
            else
            {
                map[Index(beam, currentRow)] = '|';
            }
        }
        beams = nextBeams;
    }
    return splitCounter;
}

long Solution2(int height, int width, List<char> map, int startingPoint)
{
    var currentRow = 1;
    var beams = new List<int>();
    map[Index(startingPoint, currentRow)] = '|';

    return WalkCount(height, width, map, startingPoint, currentRow);
}

long WalkCount(int height, int width, IReadOnlyList<char> map, int x, int row)
{
    if (memo.TryGetValue((x, row), out var v)) return v;

    int r = row;
    while (r + 1 < height)
    {
        r++;
        char c = map[r * width + x];
        if (c == '^')
        {
            long sum = 0;
            if (x - 1 >= 0) sum += WalkCount(height, width, map, x - 1, r - 1);
            if (x + 1 < width) sum += WalkCount(height, width, map, x + 1, r - 1);
            return memo[(x, row)] = sum;
        }
    }

    return memo[(x, row)] = 1;
}
