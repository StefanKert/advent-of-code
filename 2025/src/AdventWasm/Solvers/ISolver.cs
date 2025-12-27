namespace AdventWasm.Solvers;

public interface ISolver
{
    string Title { get; }
    string Description { get; }
    string SolvePart1(string input);
    string SolvePart2(string input);
}
