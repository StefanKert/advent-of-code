using AdventWasm.Solvers;

namespace AdventWasm;

public class SolverImpl : exports.advent.solvers.ISolver
{
    private static readonly Dictionary<byte, ISolver> Solvers = new()
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

    public static string Solve(byte day, byte part, string input)
    {
        if (!Solvers.TryGetValue(day, out var solver))
        {
            return $"Day {day} not implemented";
        }

        return part switch
        {
            1 => solver.SolvePart1(input),
            2 => solver.SolvePart2(input),
            _ => $"Invalid part: {part}"
        };
    }

    public static List<byte> GetAvailableDays()
    {
        return Solvers.Keys.OrderBy(k => k).ToList();
    }

    public static exports.advent.solvers.ISolver.DayInfo? GetDayInfo(byte day)
    {
        if (!Solvers.TryGetValue(day, out var solver))
        {
            return null;
        }

        return new exports.advent.solvers.ISolver.DayInfo(solver.Title, solver.Description);
    }
}
