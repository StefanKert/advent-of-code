var lines = File.ReadAllLines("input.txt");

var height = lines.Length;
var width = lines[0].Length;

var map = lines.SelectMany(line => line.ToCharArray().ToList()).ToList();
var startingPoint = map.IndexOf('S');
var memo = new Dictionary<(int x, int row), long>();
//Solution1(height, width, map, startingPoint);
Solution2(height, width, map, startingPoint);

void PrintMap(List<char> map, int width, int height)
{
    var result = new System.Text.StringBuilder();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            result.Append(map[Index(x, y)]);
        }
        result.AppendLine();
    }
    Console.Clear();
    Console.WriteLine(result.ToString());
}

int Index(int x, int y) => y * width + x;

bool InBounds(int x, int y, int width, int height) =>
    x >= 0 && y >= 0 && x < width && y < height;

void Solution1(int height, int width, List<char> map, int startingPoint)
{
    var currentRow = 1;
    // since we always know the row we can only store the beam location
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
                map[Index(beam, currentRow)] = '|'; // Continue the beam downwards
                nextBeams.Add(beam);
                // in this case we can keep the beam
            }
            else if (map[Index(beam, currentRow)] == '^')
            {
                // In this case we split it
                if (InBounds(beam - 1, currentRow, width, height))
                {
                    map[Index(beam - 1, currentRow)] = '|'; // Continue the beam downwards
                    nextBeams.Add(beam - 1);
                }

                if (InBounds(beam + 1, currentRow, width, height))
                {
                    map[Index(beam + 1, currentRow)] = '|'; // Continue the beam downwards
                    nextBeams.Add(beam + 1);
                }
                splitCounter++;
            }
            else
            {
                map[Index(beam, currentRow)] = '|'; // Continue the beam downwards
            }
        }
        PrintMap(map, width, height);
        beams = nextBeams;
    }
    Console.WriteLine($"Starting Point: {splitCounter}");
}


void Solution2(int height, int width, List<char> map, int startingPoint)
{
    var currentRow = 1;
    // since we always know the row we can only store the beam location
    var beams = new List<int>();
    map[Index(startingPoint, currentRow)] = '|';

    var splitCounter = WalkCount(height, width, map, startingPoint, currentRow);
    Console.WriteLine($"Starting Point: {splitCounter}");
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