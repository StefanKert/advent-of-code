// DEMO 357
Console.WriteLine("Solution 1 (Demo):" + GetRawData(true).Sum(bank => Aggregate(bank, 2)));
// Input 17452
Console.WriteLine("Solution 1 (Input):" + GetRawData(false).Sum(bank => Aggregate(bank, 2)));

// DEMO 3121910778619
Console.WriteLine("Solution 2 (Demo):" + GetRawData(true).Sum(bank => Aggregate(bank, 12)));
// Input 173300819005913
Console.WriteLine("Solution 2 (Input):" + GetRawData(false).Sum(bank => Aggregate(bank, 12)));

static long Aggregate(List<long> batteries, int batteriesEnabled)
{
    if (batteriesEnabled == 0)
    {
        return 0;
    }

    var batteriesToEnable = batteriesEnabled - 1;
    var highestNumber = batteries[0..^batteriesToEnable].Max();
    return highestNumber * (long) Math.Pow(10, batteriesToEnable) + Aggregate(batteries[(batteries.IndexOf(highestNumber) + 1)..], batteriesToEnable);
}

static List<List<long>> GetRawData(bool demo)
{
    return File.ReadAllText(demo ? "demo.txt" : "input.txt").Split(Environment.NewLine).Select(x => x.Select(y => long.Parse(y.ToString())).ToList()).ToList();
}