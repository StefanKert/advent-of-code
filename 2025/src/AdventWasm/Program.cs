using AdventWasm.Solvers;

// Simple console-based solver
// Usage: Pass day, part, and input via environment variables or stdin

var solvers = new Dictionary<int, ISolver>
{
    { 1, new Day01Solver() },
    { 2, new Day02Solver() },
    { 3, new Day03Solver() },
    { 4, new Day04Solver() },
    { 5, new Day05Solver() },
    { 6, new Day06Solver() },
    { 7, new Day07Solver() },
    { 8, new Day08Solver() },
    { 9, new Day09Solver() },
    { 10, new Day10Solver() },
    { 11, new Day11Solver() },
    { 12, new Day12Solver() },
};

// Get day and part from environment or args
var dayStr = Environment.GetEnvironmentVariable("AOC_DAY") ?? (args.Length > 0 ? args[0] : "1");
var partStr = Environment.GetEnvironmentVariable("AOC_PART") ?? (args.Length > 1 ? args[1] : "1");

if (!int.TryParse(dayStr, out int day) || !int.TryParse(partStr, out int part))
{
    Console.WriteLine("Invalid day or part");
    return;
}

// Read input from stdin or environment
var input = Environment.GetEnvironmentVariable("AOC_INPUT");
if (string.IsNullOrEmpty(input))
{
    input = Console.In.ReadToEnd();
}

if (!solvers.TryGetValue(day, out var solver))
{
    Console.WriteLine($"Day {day} not implemented");
    return;
}

var result = part switch
{
    1 => solver.SolvePart1(input),
    2 => solver.SolvePart2(input),
    _ => $"Invalid part: {part}"
};

Console.WriteLine(result);
