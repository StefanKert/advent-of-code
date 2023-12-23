using Map = char[][];

var map = Parse(File.ReadAllText("demo.txt"));

var currentRow = 0;
var currentCol = 0;
var walkingDirectionHorizontal = MovingDirections.Right;
var curVal = map[currentRow][currentCol];
map[currentRow][currentCol] = '#';

Data(map, 0, 0, MovingDirections.Right, true);


static void Data(Map map, int startRow, int startColumn, MovingDirections direction, bool printLayout = false)
{
    var walkingDirectionHorizontal = direction;
    var currentRow = startRow;
    var currentCol = startColumn;
    var curVal = map[currentRow][currentCol];
    map[currentRow][currentCol] = '#';
    while (true)
    {
        if (curVal == '.')
        {
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
        }

        if (curVal == '|')
        {
            switch (walkingDirectionHorizontal)
            {
                case MovingDirections.Left:
                    Data(map, currentRow--, currentCol, MovingDirections.Up);
                    Data(map, currentRow++, currentCol, MovingDirections.Down);
                    break;
                case MovingDirections.Right:
                    Data(map, currentRow--, currentCol, MovingDirections.Up);
                    Data(map, currentRow++, currentCol, MovingDirections.Down);
                    break;
                case MovingDirections.Up:
                    currentRow--;
                    break;
                case MovingDirections.Down:
                    currentRow++;
                    break;
            }
        }


        if (curVal == '-')
        {
            switch (walkingDirectionHorizontal)
            {
                case MovingDirections.Left:
                    currentCol--;
                    break;
                case MovingDirections.Right:
                    currentCol++;
                    break;
                case MovingDirections.Up:
                    Data(map, currentRow, currentCol++, MovingDirections.Right);
                    Data(map, currentRow, currentCol--, MovingDirections.Left);
                    break;
                case MovingDirections.Down:
                    Data(map, currentRow, currentCol++, MovingDirections.Right);
                    Data(map, currentRow, currentCol--, MovingDirections.Left);
                    break;
            }
        }


        if (curVal == '/')
        {
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
        }

        if (curVal == '\\')
        {
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
        }

        if (!CheckBounds(map, currentRow, currentCol))
        {
            break;
        }
        curVal = map[currentRow][currentCol];
        map[currentRow][currentCol] = '#';

        if (printLayout)
        {
            PrintLayout(map);
        }
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
            Console.Write(map[irow][icol]);
        }
        Console.WriteLine();
    }
}

static Map Parse(string input) => input.Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();

public enum MovingDirections
{
    Up,
    Down,
    Right,
    Left
}