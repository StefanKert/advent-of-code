var input = File.ReadAllLines("demo.txt").ToList();
var lastIndex = input.LastIndexOf(string.Empty);

var shapeRows = input[..lastIndex];
var shape = new List<List<string>>();
for (int i = 0; i < shapeRows.Count; i++)
{
    string? row = shapeRows[i];
    if (row == "")
    {
        continue;
    }
    shape.Add(new List<string>
    {
        shapeRows[i + 1],
        shapeRows[i + 2],
        shapeRows[i + 3]
    });
    i += 3;
}
var regions = input[(lastIndex + 1)..].Select(x => (width: long.Parse(x.Split(":")[0].Split("x")[0]), height: long.Parse(x.Split(":")[0].Split("x")[1]), shapes: x.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList())).ToList();
var count = 0;
foreach(var region in regions)
{
    var area = region.width * region.height;
    var occupiedSquares = 7 * region.shapes[0] +
                      7 * region.shapes[1] +
                      7 * region.shapes[2] +
                      7 * region.shapes[3] +
                      6 * region.shapes[4] +
                      5 * region.shapes[5];
    if (area > occupiedSquares)
        count++;
}
Console.WriteLine(count);
