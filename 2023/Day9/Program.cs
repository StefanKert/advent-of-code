using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var regex = new Regex(@"-?\d+");

var sumRight = new List<int>();
foreach (var line in lines)
{
    var numbers = regex.Matches(line).Select(x => int.Parse(x.Value)).ToList();
    numbers.Add(numbers.Last() + GetNextValue(numbers));
    sumRight.Add(numbers.Last());
    //  System.Console.WriteLine("Numbers: "+ string.Join(",", numbers));
}

var sumLeft = new List<int>();
foreach (var line in lines)
{
    var numbers = regex.Matches(line).Select(x => int.Parse(x.Value)).ToList();
    numbers.Insert(0, numbers.First() - GetPreviousvalue(numbers));
    sumLeft.Add(numbers.First());
    System.Console.WriteLine("Numbers: "+ string.Join(",", numbers));
}

System.Console.WriteLine("Sum (extend right): " + sumRight.Sum());
System.Console.WriteLine("Sum (extend left): " + sumLeft.Sum());

int GetNextValue(List<int> numbers)
{
    var differences = new List<int>();
    for (int i = 0; i < numbers.Count - 1; i++)
    {
        differences.Add(numbers[i + 1] - numbers[i]);
    }
    if (differences.All(x => x == 0))
    {
        return 0;
    }
    differences.Add(differences.Last() + GetNextValue(differences));
    return differences.Last();
}

int GetPreviousvalue(List<int> numbers)
{
    var differences = new List<int>();
    for (int i = 0; i < numbers.Count - 1; i++)
    {
        differences.Add(numbers[i + 1] - numbers[i]);
    }
    if (differences.All(x => x == 0))
    {
        return 0;
    }
    differences.Insert(0, differences.First() - GetPreviousvalue(differences));
    return differences.First();
}