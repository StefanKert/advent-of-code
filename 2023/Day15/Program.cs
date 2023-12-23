var data = Parse(File.ReadAllText("input.txt"));
Console.WriteLine("Solution 1: {0}", data.Sum(GetHashCode));
Console.WriteLine("Solution 2: {0}", CalculateSum(GetBoxes(data)));

static List<string> Parse(string input) => input.Split(',').ToList();

static int CalculateSum(Dictionary<int, List<(string, int)>> boxes)
{
    var sum = 0;
    foreach (var box in boxes)
    {
        for (int i = 0; i < box.Value.Count; i++)
        {
            sum += (box.Key + 1) * (i + 1) * box.Value[i].Item2;
        }
    }
    return sum;
}

static Dictionary<int, List<(string label, int focalLength)>> GetBoxes(List<string> data)
{
    var dict = new Dictionary<int, List<(string label, int focalLength)>>();
    foreach (var bucket in data)
    {
        var box = GetBox(bucket);
        if (dict.ContainsKey(box))
        {
            if (bucket.Contains("-"))
            {
                var lense = dict[box].FindIndex(lens => lens.label == bucket.TrimEnd('-'));
                if (lense >= 0)
                {
                    dict[box].RemoveAt(lense);
                }
            }
            else
            {
                var parts = bucket.Split('=');
                var lense = dict[box].FindIndex(lens => lens.label == parts[0]);
                if (lense >= 0)
                {
                    dict[box][lense] = (parts[0], int.Parse(parts[1]));
                }
                else
                {
                    dict[box].Add((parts[0], int.Parse(parts[1])));
                }
            }
        }
        else
        {
            if (bucket.Contains("="))
            {
                var parts = bucket.Split('=');
                dict[box] = new List<(string, int)>
                {
                    (parts[0], int.Parse(parts[1]))
                };
            }
        }
    }
    return dict;
}

static int GetBox(string bucket)
{
    if (bucket.Contains("="))
    {
        var parts = bucket.Split('=');
        return GetHashCode(parts[0]);
    }
    else
    {
        var parts = bucket.Split('-');
        return GetHashCode(parts[0]);
    }
}

static int GetHashCode(string bucket)
{
    var startValue = 0;
    foreach (var c in bucket)
    {
        var asciiValue = (int)c;
        startValue += asciiValue;
        startValue *= 17;
        startValue = startValue % 256;
    }

    return startValue;
}