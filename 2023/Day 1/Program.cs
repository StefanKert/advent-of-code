
using System.Text.RegularExpressions;

var numberValues = new Dictionary<string, string> {
   { "one", "1"},
   { "two",  "2"},
   { "three",  "3"},
   { "four",  "4"},
   { "five",  "5"},
   { "six",  "6"},
   { "seven",  "7"},
   { "eight",  "8"},
   { "nine",  "9"}
};

var regex = new Regex(@"(?=(\d|one|two|three|four|five|six|seven|eight|nine))");

var regexums = 0;
var data = File.ReadAllLines("puzzleinput.txt").ToList();

foreach (var line in data)
{
    var regexex = regex.Matches(line);
    var firstRegex = regexex.FirstOrDefault().Groups[1].Value;
    var lastRegex = regexex.LastOrDefault().Groups[1].Value;
    if (numberValues.ContainsKey(firstRegex))
    {
        firstRegex = numberValues[firstRegex];
    }
    if (numberValues.ContainsKey(lastRegex))
    {
        lastRegex = numberValues[lastRegex];
    }
    regexums += int.Parse("" + firstRegex + lastRegex);
}
Console.WriteLine(regexums);