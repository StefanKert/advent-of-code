// Day 3: Battery Aggregation
// Recursive aggregation of battery values

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines.map(line => line.split('').map(Number));
}

function aggregate(batteries, startIdx, count) {
    if (count === 0 || startIdx >= batteries.length) return 0n;

    let maxVal = 0n;
    let maxIdx = startIdx;

    for (let i = startIdx; i < batteries.length; i++) {
        const val = BigInt(batteries[i]);
        if (val > maxVal) {
            maxVal = val;
            maxIdx = i;
        }
    }

    const power = BigInt(10) ** BigInt(count - 1);
    const contribution = maxVal * power;

    return contribution + aggregate(batteries, maxIdx + 1, count - 1);
}

export function solvePart1(input) {
    const grid = parseInput(input);
    let total = 0n;

    for (const row of grid) {
        total += aggregate(row, 0, 2);
    }

    return total.toString();
}

export function solvePart2(input) {
    const grid = parseInput(input);
    let total = 0n;

    for (const row of grid) {
        total += aggregate(row, 0, 12);
    }

    return total.toString();
}

export const info = {
    title: "Battery Aggregation",
    description: "Recursively aggregate battery values by finding max in remaining range.",
    demo: `3443334373
5452432395
2533544335`
};
