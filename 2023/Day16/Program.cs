using Map = (char symbol, int energized)[][];

var map = Parse(File.ReadAllText("input.txt"));
Data(map, 0, 0, MovingDirections.Right);

Console.WriteLine("Solution 1 " + CalcEnergized(map));


static void Data(Map map, int startRow, int startColumn, MovingDirections direction)
{
    if (!CheckBounds(map, startRow, startColumn))
    {
        return;
    }

    var walkingDirectionHorizontal = direction;
    var currentRow = startRow;
    var currentCol = startColumn;
    var alreadyEnergized = false;
    if (map[currentRow][currentCol].energized > 1000)
    {
        return;
    }
    map[currentRow][currentCol].energized++;

    while (true)
    {
        var curVal = map[currentRow][currentCol];
        switch (curVal.symbol)
        {
            case '.':
                switch (walkingDirectionHorizontal)
                {
                    case MovingDirections.Left:
                        currentCol--;
                        break;
                    case MovingDirections.Right:
                        currentCol++;
                        break;
                    case MovingDirections.Up:
                        currentRow--;
                        break;
                    case MovingDirections.Down:
                        currentRow++;
                        break;
                }
                break;
            case '|':
                switch (walkingDirectionHorizontal)
                {
                    case MovingDirections.Left:
                    case MovingDirections.Right:
                        Data(map, currentRow - 1, currentCol, MovingDirections.Up);
                        Data(map, currentRow + 1, currentCol, MovingDirections.Down);
                        return;
                    case MovingDirections.Up:
                        currentRow--;
                        break;
                    case MovingDirections.Down:
                        currentRow++;
                        break;
                }
                break;
            case '-':
                switch (walkingDirectionHorizontal)
                {
                    case MovingDirections.Left:
                        currentCol--;
                        break;
                    case MovingDirections.Right:
                        currentCol++;
                        break;
                    case MovingDirections.Up:
                    case MovingDirections.Down:
                        Data(map, currentRow, currentCol + 1, MovingDirections.Right);
                        Data(map, currentRow, currentCol - 1, MovingDirections.Left);
                        return;
                }
                break;
            case '/':
                switch (walkingDirectionHorizontal)
                {
                    case MovingDirections.Left:
                        walkingDirectionHorizontal = MovingDirections.Down;
                        currentRow++;
                        break;
                    case MovingDirections.Right:
                        walkingDirectionHorizontal = MovingDirections.Up;
                        currentRow--;
                        break;
                    case MovingDirections.Up:
                        walkingDirectionHorizontal = MovingDirections.Right;
                        currentCol++;
                        break;
                    case MovingDirections.Down:
                        walkingDirectionHorizontal = MovingDirections.Left;
                        currentCol--;
                        break;
                }
                break;
            case '\\':
                switch (walkingDirectionHorizontal)
                {
                    case MovingDirections.Left:
                        walkingDirectionHorizontal = MovingDirections.Up;
                        currentRow--;
                        break;
                    case MovingDirections.Right:
                        walkingDirectionHorizontal = MovingDirections.Down;
                        currentRow++;
                        break;
                    case MovingDirections.Up:
                        walkingDirectionHorizontal = MovingDirections.Left;
                        currentCol--;
                        break;
                    case MovingDirections.Down:
                        walkingDirectionHorizontal = MovingDirections.Right;
                        currentCol++;
                        break;
                }
                break;
        }

        if (!CheckBounds(map, currentRow, currentCol))
        {
            break;
        }

        if (map[currentRow][currentCol].energized > 0 && alreadyEnergized)
        {
            return;
        }

        map[currentRow][currentCol].energized++;
        //PrintLayout(map);
    }
}

static bool CheckBounds(Map map, int currentRow, int currentCol)
{
    int rows = map.Length - 1;
    int cols = map[0].Length - 1;
    if (currentRow > rows || currentRow < 0)
    {
        return false;
    }

    if (currentCol > cols || currentCol < 0)
    {
        return false;
    }
    return true;
}

static void PrintLayout(Map map)
{
    Console.Clear();

    for (var irow = 0; irow < map.Length; irow++)
    {
        for (var icol = 0; icol < map[0].Length; icol++)
        {
            var value = map[irow][icol];
            if (value.energized > 0)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(value.symbol);
            }
        }
        Console.WriteLine();
    }
}

static int CalcEnergized(Map map)
{
    var sum = 0;
    for (var irow = 0; irow < map.Length; irow++)
    {
        for (var icol = 0; icol < map[0].Length; icol++)
        {
            var value = map[irow][icol];
            if (value.energized > 0)
            {
                sum++;
            }
        }
    }
    return sum;
}

static Map Parse(string input) => input.Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray().Select(x => x.Select(y => (y, 0)).ToArray()).ToArray();

public enum MovingDirections
{
    Up = 1,
    Down = 2,
    Right = 4,
    Left = 8
}