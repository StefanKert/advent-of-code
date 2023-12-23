var patterns = ParseMaps(File.ReadAllText("input.txt"));
var matchNum = 0;
foreach (var pattern in patterns)
{
    var (dim, loc) = FindReflection(pattern,1);
    var multiply = !dim ? 100 : 1;
    matchNum += loc * multiply;
}

Console.WriteLine("Amount of maps: " + matchNum);

static (bool, int) FindReflection(char[,] pattern, int smudges)
{
    var xl = pattern.GetLength(0);
    var yl = pattern.GetLength(1);

    for (int x = 1; x < xl; x++)
    {
        if (FoundVerticalReflection(pattern, x, smudges))
            return (true, x);
    }

    for (int y = 1; y < yl; y++)
    {
        if (FoundHorizontalReflection(pattern, y, smudges))
            return (false, y);
    }

    throw new NotImplementedException();
}

static bool FoundVerticalReflection(char[,] pattern, int x, int smudges)
{
    var xl = pattern.GetLength(0);
    var yl = pattern.GetLength(1);
    var dl = DLength(x, xl);
    int sCount = 0;
    for (int d = 0; d < dl; d++)
    {
        for (int y = 0; y < yl; y++)
        {
            if (pattern[x - 1 - d, y] != pattern[x + d, y])
                sCount++;
        }
    }

    return smudges == sCount;
}

static bool FoundHorizontalReflection(char[,] pattern, int y, int smudges)
{
    var xl = pattern.GetLength(0);
    var yl = pattern.GetLength(1);
    var dl = DLength(y, yl);
    int sCount = 0;
    for (int d = 0; d < dl; d++)
    {
        for (int x = 0; x < xl; x++)
        {
            if (pattern[x, y - 1 - d] != pattern[x, y + d])
                sCount++;
        }
    }

    return smudges == sCount;
}

static int DLength(int point, int length) => Math.Min(point, length - point);

static List<char[,]> ParseMaps(string input) => input.Split($"\n\n").Select(ParseMap).ToList();

//static List<char[,]> ParseMaps(string input) => input.Split($"{Environment.NewLine}{Environment.NewLine}").Select(ParseMap).ToList();

static char[,] ParseMap(string input)
{
    var rows = input.Split("\n", StringSplitOptions.TrimEntries);
    int yl = rows.Length;
    int xl = rows.First().Length;
    char[,] arr = new char[xl, yl];
    for (int y = 0; y < yl; y++)
    {
        for (int x = 0; x < xl; x++)
        {
            arr[x, y] = rows[y][x];
        }
    }
    return arr;
}