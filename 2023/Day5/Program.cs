using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

var lines = File.ReadAllLines("demo.txt");
var seeds = new List<long>();
var matchNumbersRegex = new Regex(@"(\d+)");
var mappingsRegex = new Regex(@"(\d+).(\d+).(\d+)");
seeds = matchNumbersRegex.Matches(lines[0]).Select(m => long.Parse(m.Value)).ToList();

System.Console.WriteLine($"Part 1: {string.Join(',', seeds)}");
var defaultMap = new Dictionary<long, long>();
for (long i = 0; i < 100; i++)
{
    defaultMap.Add(i, i);
}

var seedToSoilMap = new Dictionary<long, long>();

var seedToSoilRules = new List<(long lowerRange, long uperRange, long diff)>();
var soilToFertilizerRules = new List<(long lowerRange, long uperRange, long diff)>();
var fertilizerToWaterRules = new List<(long lowerRange, long uperRange, long diff)>();
var waterToLightRules = new List<(long lowerRange, long uperRange, long diff)>();
var lightToTemperatureRules = new List<(long lowerRange, long uperRange, long diff)>();
var temperatureToHumidityRules = new List<(long lowerRange, long uperRange, long diff)>();
var humidityTolocationRules = new List<(long lowerRange, long uperRange, long diff)>();

var mappingLists = new List<List<(long lowerRange, long uperRange, long diff)>>
{
    seedToSoilRules,
    soilToFertilizerRules,
    fertilizerToWaterRules,
    waterToLightRules,
    lightToTemperatureRules,
    temperatureToHumidityRules,
    humidityTolocationRules
};
var mappingListUsed = 0;
for (long i = 1; i < lines.Length; i++)
{
    if (string.IsNullOrWhiteSpace(lines[i]))
    {
        continue;
    }
    else
    {
        i++;
        var currList = mappingLists[mappingListUsed];
        while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
        {
            var result = lines[i].Split(" ").Select(x => long.Parse(x)).ToList();
            var destination = result[0];
            var source = result[1];
            var length = result[2];
            var rule = source + length - 1;
            long diff = 0;
            if (source < destination)
            {
                diff = destination - source;
            }
            else
            {
                diff = destination - source;
            }
            currList.Add((source, rule, diff));
            i++;
        }
        mappingListUsed++;
    }
}

// 98 - 48 => 50
// 99 - 48 => 51
// Range = (Left Number + Right Number -1)

// 52 + (48 - 1) => 99

// 50 + 2 => 52
// 50 + 47 => 97 => 99

// prlong all list 
//System.Console.WriteLine($"seedToSoilMap: {string.Join(',', seedToSoilMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"soilToFertilizerMap: {string.Join(',', soilToFertilizerMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"fertilizerToWaterMap: {string.Join(',', fertilizerToWaterMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"waterToLightMap: {string.Join(',', waterToLightMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"lightToTemperatureMap: {string.Join(',', lightToTemperatureMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"temperatureToHumidityMap: {string.Join(',', temperatureToHumidityMap.Select(x => $"{x.Key}=>{x.Value}"))}");
//System.Console.WriteLine($"humidityToLocationMap: {string.Join(',', humidityToLocationMap.Select(x => $"{x.Key}=>{x.Value}"))}");

var locations = new List<long>();
foreach (var seed in seeds)
{
    var soil = seed;
    var seedToSoilRule = seedToSoilRules.FirstOrDefault(x => x.lowerRange <= seed && x.uperRange >= seed);
    if (seedToSoilRule != default)
    {
        soil = seed + seedToSoilRule.diff;
    }

    var fertilizer = soil;
    var soilToFertilizerRule = soilToFertilizerRules.FirstOrDefault(x => x.lowerRange <= soil && x.uperRange >= soil);
    if (soilToFertilizerRule != default)
    {
        fertilizer = soil + soilToFertilizerRule.diff;
    }

    var water = fertilizer;
    var fertilizerToWaterRule = fertilizerToWaterRules.FirstOrDefault(x => x.lowerRange <= fertilizer && x.uperRange >= fertilizer);
    if (fertilizerToWaterRule != default)
    {
        water = fertilizer + fertilizerToWaterRule.diff;
    }

    var light = water;
    var waterToLightRule = waterToLightRules.FirstOrDefault(x => x.lowerRange <= water && x.uperRange >= water);
    if (waterToLightRule != default)
    {
        light = water + waterToLightRule.diff;
    }

    var temperature = light;
    var lightToTemperatureRule = lightToTemperatureRules.FirstOrDefault(x => x.lowerRange <= light && x.uperRange >= light);
    if (lightToTemperatureRule != default)
    {
        temperature = light + lightToTemperatureRule.diff;
    }

    var humidity = temperature;
    var temperatureToHumidityRule = temperatureToHumidityRules.FirstOrDefault(x => x.lowerRange <= temperature && x.uperRange >= temperature);
    if (temperatureToHumidityRule != default)
    {
        humidity = temperature + temperatureToHumidityRule.diff;
    }

    var location = humidity;
    var humidityToLocationRule = humidityTolocationRules.FirstOrDefault(x => x.lowerRange <= humidity && x.uperRange >= humidity);
    if (humidityToLocationRule != default)
    {
        location = humidity + humidityToLocationRule.diff;
    }
    locations.Add(location);
    System.Console.WriteLine($"Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");
}

Console.WriteLine("closest location: " + locations.Min());