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

List<List<char>> matrix = lines
    .Select(line => line.ToCharArray().ToList())
    .ToList();

var partStr = Environment.GetEnvironmentVariable("AOC_PART");
if (partStr == "1")
{
    Console.WriteLine(Solution1(matrix));
}
else if (partStr == "2")
{
    Console.WriteLine(Solution2(matrix));
}
else
{
    Console.WriteLine("Solution 1: " + Solution1(matrix));
    Console.WriteLine("Solution 2: " + Solution2(matrix));
}

static List<Point> GetNeighbours(int x, int y, List<List<char>> matrix)
{
    var neighbours = new List<Point>();
    var directions = new (int dx, int dy)[]
    {
        (-1, -1), (-1, 0), (-1, 1),
        (0, -1),          (0, 1),
        (1, -1), (1, 0), (1, 1)
    };
    foreach (var (dx, dy) in directions)
    {
        int newX = x + dx;
        int newY = y + dy;
        if (newX >= 0 && newX < matrix.Count && newY >= 0 && newY < matrix[0].Count)
        {
            neighbours.Add(new Point(newX, newY, matrix[newX][newY]));
        }
    }
    return neighbours;
}

static int Solution1(List<List<char>> matrix)
{
    var validPoints = new List<Point>();

    for (var i = 0; i < matrix.Count; i++)
    {
        for (var j = 0; j < matrix[i].Count; j++)
        {
            if (matrix[i][j] == '@')
            {
                var paperRollsCount = GetNeighbours(i, j, matrix).Count(x => x.character == '@');
                if (paperRollsCount < 4)
                {
                    validPoints.Add(new Point(i, j, matrix[i][j]));
                }
            }
        }
    }
    return validPoints.Count;
}

static int Solution2(List<List<char>> matrix)
{
    int i = 0;
    int j = 0;
    var validPoints = new List<Point>();
    while (i < matrix.Count)
    {
    Restart:
        while (j < matrix[i].Count)
        {
            if (matrix[i][j] == '@')
            {
                var paperRollsCount = GetNeighbours(i, j, matrix).Count(x => x.character == '@');
                if (paperRollsCount < 4)
                {
                    matrix[i][j] = '.';
                    validPoints.Add(new Point(i, j, matrix[i][j]));
                    i = 0;
                    j = 0;
                    goto Restart;
                }
            }
            j++;
        }
        i++;
        j = 0;
    }
    return validPoints.Count;
}

public record Point(int x, int y, char character);
