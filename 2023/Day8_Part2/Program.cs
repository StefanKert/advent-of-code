var lines = File.ReadAllLines("input.txt");
var instructionSet = lines[0].Where(x => x == 'R' || x == 'L').ToArray();
var map = new Dictionary<string, (string left, string right)>();
for (int i = 2; i < lines.Length; i++)
{
    var line = lines[i];
    var parts = line.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var data = parts[1].Replace('(', ' ').Replace(')', ' ').Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    map.Add(parts[0], (data[0], data[1]));
}

var paths = map.Where(x => x.Key.Last() == 'A').Select(x => x.Key).ToList();
var steps = 0;
var instruction = 0;

var stepsList = new List<long>();
foreach (var path in paths)
{
    var step = GetSteps(map, path, "Z", instructionSet);
    stepsList.Add(step);
    System.Console.WriteLine(path + ": " + step);
}
System.Console.WriteLine(lcm(stepsList));

static long gcd(long n1, long n2)
{
    if (n2 == 0)
    {
        return n1;
    }
    else
    {
        return gcd(n2, n1 % n2);
    }
}

static long lcm(List<long> numbers)
{
    return numbers.Aggregate((S, val) => S * val / gcd(S, val));
}

static long GetSteps(Dictionary<string, (string left, string right)> map, string current, string data, char[] instructionSet)
{
    var steps = 0;
    var instruction = 0;
    var postion = current;
    while (!postion.EndsWith(data))
    {
        var nextStep = instructionSet[instruction];
        instruction++;
        if (instruction == instructionSet.Length)
        {
            instruction = 0;
        }

        postion = nextStep switch
        {
            'L' => map[postion].left,
            'R' => map[postion].right,
        };
        steps++;
    }
    return steps;
}