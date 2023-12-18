using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

var lines = File.ReadAllLines("puzzleinput.txt").ToList();
var numbers = GetNumbers();
var sum = 0;
var gearPositions = new List<(int row, int column)>();
for (var row = 0; row < lines.Count; row++)
{
    var line = lines[row];
    for (var column = 0; column < line.Length; column++)
    {
        var colValue = line[column];
        if (colValue == '*')
        {
            gearPositions.Add((row, column));
            System.Console.WriteLine($"Gear found at {row}, {column} with {GetNeighbors(row, column).Count} neighbors");
            var neighbors = GetNeighbors(row, column).Select(x => int.Parse(x)).ToList();
            if (neighbors.Count > 2)
            {
                throw new Exception("More than 2 neighbors");
            }
            if (neighbors.Count == 2)
            {
                sum += neighbors[0] * neighbors[1];
            }
        }
    }
}

List<(string number, int row, int fromColumn)> GetNumbers()
{
    var numbers = new List<(string number, int row, int fromColumn)>();
    for (var row = 0; row < lines.Count; row++)
    {
        var line = lines[row];
        var number = "";
        var validPart = false;
        var column = 0;
        for (column = 0; column < line.Length; column++)
        {
            var colValue = line[column];
            if (char.IsDigit(colValue))
            {
                number += colValue;
                continue;
            }
            else
            {
                if (number.Length > 0)
                {
                    numbers.Add((number, row, column - number.Length));
                    validPart = false;
                    number = "";
                }
                // todo check number
            }
        }

        if (number.Length > 0)
        {
            numbers.Add((number, row, column - number.Length));
        }
    }
    return numbers;
}

List<string> GetNeighbors(int row, int column)
{
    var rowMin = row == 0 ? 0 : row - 1;
    var rowMax = row > lines.Count ? lines.Count : row + 1;

    var columnMin = column == 0 ? 0 : column - 1;
    var columnMax = column > lines[0].Length ? lines[0].Length : column + 1;

    var sorroundingRows = numbers.Where(x => x.row >= rowMin && x.row <= rowMax);
    return sorroundingRows.Where(x => (x.fromColumn + x.number.Length - 1) >= columnMin && x.fromColumn <= columnMax).Select(x => x.number).ToList();
}
System.Console.WriteLine($"Sum: {sum}");