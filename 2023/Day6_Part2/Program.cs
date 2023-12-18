var file = File.ReadAllLines("input.txt");

var timings = file[0].Replace("Time:", "").Replace(" ", "").Trim().Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
var distances = file[1].Replace("Distance:", "").Replace(" ", "").Trim().Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();

var winsPerGroup = new List<long>();
for (int i = 0; i < timings.Count; i++)
{
    var time = timings[i];
    var distance = distances[i];
    var win = 0;
    for (int timePressed = 0; timePressed < time; timePressed++)
    {
        var timeLeft = time - timePressed;
        var distanceTraveled = timePressed * timeLeft;
        if (distanceTraveled > distance)
        {
            win++;
        }
    }
    winsPerGroup.Add(win);
}

var result = winsPerGroup.Aggregate(1L, (acc, x) => acc * x);
System.Console.WriteLine(result);