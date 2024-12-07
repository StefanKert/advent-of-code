namespace AdventOfCode.Helpers
{
    public abstract class AbstractSolution
    {
        protected string _input;

        protected AbstractSolution()
        {
            _input = File.ReadAllText("input.txt");
        }

        protected AbstractSolution(string filepath)
        {
            _input = File.ReadAllText(filepath);
        }

        public abstract long Part1();
        public abstract long Part2();

        public void PrintSolutions()
        {
            Console.WriteLine($"Part 1: {Part1()}");
            Console.WriteLine($"Part 2: {Part2()}");
        }
    }

}
