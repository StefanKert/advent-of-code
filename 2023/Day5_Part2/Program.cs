using System.Collections.Concurrent;

var lines = File.ReadAllLines("input.txt");
var seeds = new List<(long min, long max)>();
var line1 = lines[0];
line1 = line1.Replace("seeds: ", "");
var parts = line1.Split(" ");
var numbers = parts.Select(x => long.Parse(x)).ToList();
for (int i = 0; i < numbers.Count; i++)
{
    var min = numbers[i];
    i++;
    var max = numbers[i] + min - 1;
    seeds.Add((min, max));
}

Console.WriteLine($"seeds: {string.Join(',', seeds.Select(x => $"{x.min}=>{x.max}"))}");

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

var locations = new ConcurrentBag<long>();
Parallel.ForEach(seeds, new ParallelOptions
{
    MaxDegreeOfParallelism = 100
}, seedRange =>
{
    var innerLocations = new ConcurrentBag<long>();
    Parallel.For(seedRange.min, seedRange.max + 1, new ParallelOptions
    {
        MaxDegreeOfParallelism = 1000
    }, i =>
    {
        var seed = i;
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
        innerLocations.Add(location);
        //System.Console.WriteLine($"Seed {seed}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");
    });
    var innerMinValue = innerLocations.Min();
    System.Console.WriteLine("Done with seed range " + seedRange + " with min value " + innerMinValue);
    locations.Add(innerMinValue);
});

Console.WriteLine("closest location: " + locations.Min());