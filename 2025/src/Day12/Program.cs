// Note: input.txt should be in the same format as demo.txt:
// - Shape definitions (0:, 1:, etc.) with 3x3 grid patterns using # and .
// - Region definitions (WxH: count0 count1 count2 count3 count4 count5)
var input = File.ReadAllLines("input.txt").ToList();
var lastIndex = input.LastIndexOf(string.Empty);

// Parse shapes and count their cell sizes
var shapeRows = input[..lastIndex];
var shapeSizes = new List<int>();
for (int i = 0; i < shapeRows.Count; i++)
{
    string row = shapeRows[i];
    if (row == "" || !row.Contains(':'))
    {
        continue;
    }
    // Found a shape header (e.g., "0:")
    var shapeSize = 0;
    for (int j = 1; j <= 3 && i + j < shapeRows.Count; j++)
    {
        shapeSize += shapeRows[i + j].Count(c => c == '#');
    }
    shapeSizes.Add(shapeSize);
    i += 3;
}

// Parse regions
var regions = input[(lastIndex + 1)..]
    .Where(x => !string.IsNullOrEmpty(x))
    .Select(x => (
        width: long.Parse(x.Split(":")[0].Split("x")[0]),
        height: long.Parse(x.Split(":")[0].Split("x")[1]),
        shapes: x.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList()
    )).ToList();

// Part 1: Count regions where presents can fit
var count = 0L;
foreach (var region in regions)
{
    var area = region.width * region.height;
    var occupiedSquares = 0L;
    for (int i = 0; i < region.shapes.Count && i < shapeSizes.Count; i++)
    {
        occupiedSquares += shapeSizes[i] * region.shapes[i];
    }
    if (area >= occupiedSquares)
        count++;
}
Console.WriteLine($"Solution 1: {count}");

// Part 2: Just placing the star - no computation needed
Console.WriteLine("Solution 2: *");
