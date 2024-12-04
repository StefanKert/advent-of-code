using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var leftList = new List<int>();
var rightList = new List<int>();
foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split("   ");
    leftList.Add(int.Parse(parts[0]));
    rightList.Add(int.Parse(parts[1]));
}

leftList.Sort();
rightList.Sort();

var totalDistance = 0;
for(int i = 0; i < leftList.Count; i++)
{
    totalDistance = totalDistance + Math.Abs(leftList[i] - rightList[i]);
}

Console.WriteLine($"Part 1: The total distance {totalDistance}");


var similarityScore = 0;
for (int i = 0; i < leftList.Count; i++)
{
    var amounts = rightList.Count(x => x == leftList[i]);
    similarityScore = leftList[i] * amounts + similarityScore;
}

Console.WriteLine($"Part 2: The similarity score {similarityScore}");