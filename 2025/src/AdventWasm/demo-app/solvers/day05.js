// Day 5: Range Merging and Validation
// Parse ranges, merge overlapping, validate IDs

function parseInput(input) {
    const sections = input.trim().split('\n\n');
    const rangeLines = sections[0].split('\n').filter(l => l.trim());
    const idLines = sections[1] ? sections[1].split('\n').filter(l => l.trim()) : [];

    const ranges = rangeLines.map(line => {
        const match = line.match(/(\d+)-(\d+)/);
        if (match) {
            return [BigInt(match[1]), BigInt(match[2])];
        }
        return null;
    }).filter(r => r !== null);

    const ids = idLines.map(line => BigInt(line.trim()));

    return { ranges, ids };
}

function mergeRanges(ranges) {
    if (ranges.length === 0) return [];

    // Sort by start
    let sorted = [...ranges].sort((a, b) => {
        if (a[0] < b[0]) return -1;
        if (a[0] > b[0]) return 1;
        return 0;
    });

    let changed = true;
    while (changed) {
        changed = false;
        const merged = [];

        for (let i = 0; i < sorted.length; i++) {
            const current = sorted[i];
            let wasMerged = false;

            for (let j = 0; j < merged.length; j++) {
                const existing = merged[j];

                // Check for overlap or adjacency
                if (current[0] <= existing[1] + 1n && current[1] >= existing[0] - 1n) {
                    merged[j] = [
                        current[0] < existing[0] ? current[0] : existing[0],
                        current[1] > existing[1] ? current[1] : existing[1]
                    ];
                    wasMerged = true;
                    changed = true;
                    break;
                }
            }

            if (!wasMerged) {
                merged.push([...current]);
            }
        }

        sorted = merged.sort((a, b) => {
            if (a[0] < b[0]) return -1;
            if (a[0] > b[0]) return 1;
            return 0;
        });
    }

    return sorted;
}

function isInRanges(id, ranges) {
    for (const [start, end] of ranges) {
        if (id >= start && id <= end) return true;
    }
    return false;
}

export function solvePart1(input) {
    const { ranges, ids } = parseInput(input);
    const merged = mergeRanges(ranges);

    let count = 0;
    for (const id of ids) {
        if (isInRanges(id, merged)) count++;
    }

    return count;
}

export function solvePart2(input) {
    const { ranges } = parseInput(input);
    const merged = mergeRanges(ranges);

    let total = 0n;
    for (const [start, end] of merged) {
        if (start > 0n) {
            total += end - start + 1n;
        }
    }

    return total.toString();
}

export const info = {
    title: "Range Merging",
    description: "Merge overlapping ranges and validate IDs against merged ranges.",
    demo: `1-10
5-15
20-30
25-35

7
12
22
50`
};
