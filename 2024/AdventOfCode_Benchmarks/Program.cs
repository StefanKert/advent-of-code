using AdventOfCode.Helpers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<AdventOfCode2024>();

[SimpleJob(RuntimeMoniker.Net90)]
public class AdventOfCode2024
{
    private AbstractSolution _day1;
    private SolutionDay2 _day2;
    private SolutionDay3 _day3;
    private SolutionDay4 _day4;
    private SolutionDay5 _day5;
    private SolutionDay6 _day6;
    private AbstractSolution _day7;
    private AbstractSolution _day8;
    private SolutionDay9 _day9;

    public AdventOfCode2024()
    {
        _day1 = new SolutionDay1("C:\\GitHub\\advent-of-code\\2024\\Day1\\input.txt");
        _day2 = new SolutionDay2("C:\\GitHub\\advent-of-code\\2024\\Day2\\input.txt");
        _day3 = new SolutionDay3("C:\\GitHub\\advent-of-code\\2024\\Day3\\input.txt");
        _day4 = new SolutionDay4("C:\\GitHub\\advent-of-code\\2024\\Day4\\input.txt");
        _day5 = new SolutionDay5("C:\\GitHub\\advent-of-code\\2024\\Day5\\input.txt");
        _day6 = new SolutionDay6("C:\\GitHub\\advent-of-code\\2024\\Day6\\input.txt");
        _day7 = new SolutionDay7("C:\\GitHub\\advent-of-code\\2024\\Day7\\input.txt");
        _day8 = new SolutionDay8("C:\\GitHub\\advent-of-code\\2024\\Day8\\input.txt");
        _day9 = new SolutionDay9("C:\\GitHub\\advent-of-code\\2024\\Day9\\input.txt");
    }

    //[Benchmark]
    //public void Day1_PartOne() => _day1.Part1();

    //[Benchmark]
    //public void Day1_PartTwo() => _day1.Part2();

    //[Benchmark]
    //public void Day2_PartOne() => _day2.Part1();

    //[Benchmark]
    //public void Day2_PartTwo() => _day2.Part2();

    //[Benchmark]
    //public void Day3_PartOne() => _day3.Part1();

    //[Benchmark]
    //public void Day3_PartTwo() => _day3.Part2();

    //[Benchmark]
    //public void Day4_PartOne() => _day4.Part1();

    //[Benchmark]
    //public void Day4_PartTwo() => _day4.Part2();

    //[Benchmark]
    //public void Day5_PartOne() => _day5.Part1();

    //[Benchmark]
    //public void Day5_PartTwo() => _day5.Part2();

    //[Benchmark]
    //public void Day6_PartOne() => _day6.Part1();

    //[Benchmark]
    //public void Day6_PartTwo() => _day6.Part2();

    //[Benchmark]
    //public void Day7_PartOne() => _day7.Part1();

    //[Benchmark]
    //public void Day7_PartTwo() => _day7.Part1();

    [Benchmark]
    public void Day8_PartOne() => _day8.Part1();

    [Benchmark]
    public void Day8_PartTwo() => _day8.Part2();

    //[Benchmark]
    //public void Day9_PartOne() => _day9.Part1();

    //[Benchmark]
    //public void Day9_PartTwo() => _day9.Part2();
}