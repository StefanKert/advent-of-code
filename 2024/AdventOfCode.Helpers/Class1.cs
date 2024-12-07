namespace AdventOfCode.Helpers
{
    public abstract class AbstractSolution
    {
        public abstract long Part1();
        public abstract long Part2();

        public void PrintSolutions()
        {
            Console.WriteLine($"Part 1: {Part1()}");
            Console.WriteLine($"Part 2: {Part2()}");
        }
    }

}
