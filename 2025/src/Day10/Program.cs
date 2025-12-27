var inputFile = args.Length > 0 ? args[0] : "input.txt";
var lines = File.ReadAllLines(inputFile);

long totalPart1 = 0;
long totalPart2 = 0;

foreach (var line in lines)
{
    var (goal, buttons, joltage) = ParseLine(line);
    totalPart1 += FindMinPressesPart1(goal, buttons);
    totalPart2 += FindMinPressesPart2(joltage, buttons);
}

Console.WriteLine($"Part 1: {totalPart1}");
Console.WriteLine($"Part 2: {totalPart2}");

(int[] goal, List<List<int>> buttons, int[] joltage) ParseLine(string line)
{
    var patternStart = line.IndexOf('[');
    var patternEnd = line.IndexOf(']');
    var pattern = line.Substring(patternStart + 1, patternEnd - patternStart - 1);
    var goal = pattern.Select(c => c == '#' ? 1 : 0).ToArray();

    var buttons = new List<List<int>>();
    var rest = line.Substring(patternEnd + 1);

    int i = 0;
    while (i < rest.Length)
    {
        if (rest[i] == '(')
        {
            int end = rest.IndexOf(')', i);
            var content = rest.Substring(i + 1, end - i - 1);
            var nums = content.Split(',').Select(s => int.Parse(s.Trim())).ToList();
            buttons.Add(nums);
            i = end + 1;
        }
        else if (rest[i] == '{')
        {
            break;
        }
        else
        {
            i++;
        }
    }

    var joltStart = rest.IndexOf('{');
    var joltEnd = rest.IndexOf('}');
    var joltContent = rest.Substring(joltStart + 1, joltEnd - joltStart - 1);
    var joltage = joltContent.Split(',').Select(s => int.Parse(s.Trim())).ToArray();

    return (goal, buttons, joltage);
}

int FindMinPressesPart1(int[] goal, List<List<int>> buttons)
{
    var numLights = goal.Length;
    var numButtons = buttons.Count;
    var minPresses = int.MaxValue;

    for (int mask = 0; mask < (1 << numButtons); mask++)
    {
        var state = new int[numLights];
        var presses = 0;

        for (int b = 0; b < numButtons; b++)
        {
            if ((mask & (1 << b)) != 0)
            {
                presses++;
                foreach (var light in buttons[b])
                {
                    state[light] ^= 1;
                }
            }
        }

        bool matches = true;
        for (int j = 0; j < numLights; j++)
        {
            if (state[j] != goal[j])
            {
                matches = false;
                break;
            }
        }

        if (matches && presses < minPresses)
        {
            minPresses = presses;
        }
    }

    return minPresses;
}

long FindMinPressesPart2(int[] target, List<List<int>> buttons)
{
    int n = buttons.Count;
    int m = target.Length;

    // Build matrix A where A[i,j] = 1 if button j affects counter i
    var matrix = new double[m, n + 1];
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            matrix[i, j] = buttons[j].Contains(i) ? 1 : 0;
        }
        matrix[i, n] = target[i];
    }

    // Gaussian elimination with partial pivoting
    int[] pivotCol = new int[m];
    for (int i = 0; i < m; i++) pivotCol[i] = -1;

    int row = 0;
    for (int col = 0; col < n && row < m; col++)
    {
        // Find pivot
        int maxRow = row;
        for (int i = row + 1; i < m; i++)
        {
            if (Math.Abs(matrix[i, col]) > Math.Abs(matrix[maxRow, col]))
                maxRow = i;
        }

        if (Math.Abs(matrix[maxRow, col]) < 1e-9)
            continue;

        // Swap rows
        for (int j = 0; j <= n; j++)
        {
            (matrix[row, j], matrix[maxRow, j]) = (matrix[maxRow, j], matrix[row, j]);
        }

        pivotCol[row] = col;

        // Eliminate
        for (int i = 0; i < m; i++)
        {
            if (i != row && Math.Abs(matrix[i, col]) > 1e-9)
            {
                double factor = matrix[i, col] / matrix[row, col];
                for (int j = 0; j <= n; j++)
                {
                    matrix[i, j] -= factor * matrix[row, j];
                }
            }
        }

        row++;
    }

    int rank = row;

    // Identify free variables
    var freeVars = new List<int>();
    var basicVars = new int[rank];
    int basicIdx = 0;
    for (int j = 0; j < n; j++)
    {
        bool isBasic = false;
        for (int i = 0; i < rank; i++)
        {
            if (pivotCol[i] == j)
            {
                isBasic = true;
                basicVars[basicIdx++] = j;
                break;
            }
        }
        if (!isBasic) freeVars.Add(j);
    }

    // If no free variables, we have a unique solution (if consistent)
    if (freeVars.Count == 0)
    {
        var solution = new double[n];
        for (int i = rank - 1; i >= 0; i--)
        {
            int col = pivotCol[i];
            solution[col] = matrix[i, n] / matrix[i, col];
        }

        // Check if all non-negative integers
        long total = 0;
        for (int j = 0; j < n; j++)
        {
            long val = (long)Math.Round(solution[j]);
            if (val < 0 || Math.Abs(solution[j] - val) > 1e-6)
                return long.MaxValue; // No valid solution
            total += val;
        }
        return total;
    }

    // With free variables, search over their values
    // Free variables can range from 0 to some maximum
    int numFree = freeVars.Count;
    var freeMax = new int[numFree];
    for (int f = 0; f < numFree; f++)
    {
        int btnIdx = freeVars[f];
        freeMax[f] = int.MaxValue;
        foreach (int c in buttons[btnIdx])
        {
            freeMax[f] = Math.Min(freeMax[f], target[c]);
        }
        if (buttons[btnIdx].Count == 0) freeMax[f] = 0;
        // Also limit by sum of targets
        freeMax[f] = Math.Min(freeMax[f], target.Max());
    }

    long best = long.MaxValue;
    var freeVals = new int[numFree];
    SearchFreeVars(0, freeVals, freeMax, freeVars, basicVars, matrix, pivotCol, rank, n, ref best);

    return best;
}

void SearchFreeVars(int idx, int[] freeVals, int[] freeMax, List<int> freeVars,
                     int[] basicVars, double[,] matrix, int[] pivotCol, int rank, int n, ref long best)
{
    if (idx == freeVals.Length)
    {
        // Compute basic variables from free variables
        var solution = new double[n];
        for (int f = 0; f < freeVals.Length; f++)
        {
            solution[freeVars[f]] = freeVals[f];
        }

        for (int i = rank - 1; i >= 0; i--)
        {
            int col = pivotCol[i];
            double rhs = matrix[i, n];
            for (int j = 0; j < n; j++)
            {
                if (j != col)
                {
                    rhs -= matrix[i, j] * solution[j];
                }
            }
            solution[col] = rhs / matrix[i, col];
        }

        // Check if valid (all non-negative integers)
        long total = 0;
        for (int j = 0; j < n; j++)
        {
            long val = (long)Math.Round(solution[j]);
            if (val < 0 || Math.Abs(solution[j] - val) > 0.01)
                return;
            total += val;
        }

        if (total < best)
        {
            best = total;
        }
        return;
    }

    // Prune: current sum already exceeds best
    long currentSum = 0;
    for (int f = 0; f < idx; f++)
    {
        currentSum += freeVals[f];
    }
    if (currentSum >= best) return;

    for (int v = 0; v <= freeMax[idx]; v++)
    {
        freeVals[idx] = v;
        SearchFreeVars(idx + 1, freeVals, freeMax, freeVars, basicVars, matrix, pivotCol, rank, n, ref best);
    }
}
