var lines = File.ReadAllLines("input.txt");

var instructionSet = lines[0];

var map = new Dictionary<string, (string left, string right)>();
for (int i = 2; i < lines.Length; i++)
{
    var line = lines[i];
    var parts = line.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var paths = parts[1].Replace('(', ' ').Replace(')', ' ').Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    map.Add(parts[0], (paths[0], paths[1]));
}



var steps = 0;
var instruction = 0;
var postion = "AAA";
while (postion != "ZZZ")
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
System.Console.WriteLine(steps);
