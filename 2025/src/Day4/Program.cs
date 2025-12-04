List<List<char>> matrix = File.ReadAllLines("input.txt")
    .Select(line => line.ToCharArray().ToList())
    .ToList();


Solution1(matrix);
Solution2(matrix);

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

static void Solution1(List<List<char>> matrix)
{
    var validPoints = new List<Point>();
    var invalidPoints = new List<Point>();

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
    Console.WriteLine(validPoints.Count);
}

static void Solution2(List<List<char>> matrix)
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
    Console.WriteLine(validPoints.Count);
}

public record Point(int x, int y, char character);