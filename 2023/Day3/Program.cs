var lines = File.ReadAllLines("input.txt").ToList();
var symbolPositons = new List<(char symbol, int row, int column)>();
var sum = 0;
for (var row = 0; row < lines.Count; row++)
{
    var line = lines[row];
    for (var column = 0; column < line.Length; column++)
    {
        var colValue = line[column];
        if (!char.IsDigit(colValue) && colValue != '.')
        {
            symbolPositons.Add((colValue, row, column));
        }
    }
}

for (var row = 0; row < lines.Count; row++)
{
    var line = lines[row];
    var number = "";
    var validPart = false;
    for (var column = 0; column < line.Length; column++)
    {
        var colValue = line[column];
        if (char.IsDigit(colValue))
        {
            number += colValue;
            if (HasNeighborSymbol(row, column))
            {
                validPart = true;
            }
            continue;
        }
        else
        {
            if (number.Length > 0)
            {
                if (validPart)
                {
                    sum += int.Parse(number);
                    Console.WriteLine($"Valid number {number} at row {row} column {column}");
                }
                else
                {
                    Console.WriteLine($"Invalid number {number} at row {row} column {column}");
                }
                validPart = false;
                number = "";
            }
            // todo check number
        }
    }

    if (number.Length > 0)
    {
        if (validPart)
        {
            sum += int.Parse(number);
        }
        else
        {
        }
        validPart = false;
        number = "";
    }
}

bool HasNeighborSymbol(int row, int column)
{
    var validPositionsRowFrom = row - 1;
    var validPositionsRowTo = row + 1;

    var validPositionsColumnFrom = column - 1;
    var validPositionsColumnTo = column + 1;

    var symbol = symbolPositons.FirstOrDefault(x => x.row >= validPositionsRowFrom && x.row <= validPositionsRowTo && x.column >= validPositionsColumnFrom && x.column <= validPositionsColumnTo);
    if (symbol != default)
    {
        return true;
    }
    return false;
}
System.Console.WriteLine(sum);