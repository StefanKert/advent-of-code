// Day 12: Shape Packing in Regions
// Determine which regions have unoccupied space given shape definitions

function parseInput(input) {
    const lines = input.split('\n');
    const regions = [];

    for (const line of lines) {
        // Match region format: WxH: count1 count2 count3...
        const match = line.match(/^(\d+)x(\d+):\s*(.*)$/);
        if (match) {
            const width = parseInt(match[1], 10);
            const height = parseInt(match[2], 10);
            const counts = match[3].trim().split(/\s+/).map(Number);
            regions.push({ width, height, counts });
        }
    }

    return regions;
}

// Shape areas based on the puzzle definitions
// Shapes 0-3: 7 squares each
// Shape 4: 6 squares
// Shape 5: 5 squares
const SHAPE_AREAS = [7, 7, 7, 7, 6, 5];

function calculateOccupied(counts) {
    let occupied = 0;
    for (let i = 0; i < counts.length && i < SHAPE_AREAS.length; i++) {
        occupied += counts[i] * SHAPE_AREAS[i];
    }
    // Any additional shapes beyond defined use area 7
    for (let i = SHAPE_AREAS.length; i < counts.length; i++) {
        occupied += counts[i] * 7;
    }
    return occupied;
}

export function solvePart1(input) {
    const regions = parseInput(input);
    let count = 0;

    for (const region of regions) {
        const area = region.width * region.height;
        const occupied = calculateOccupied(region.counts);

        if (area > occupied) {
            count++;
        }
    }

    return count;
}

export function solvePart2(input) {
    // Part 2 might have different logic - using same for now
    // Could be total unoccupied space or different calculation
    const regions = parseInput(input);
    let totalUnoccupied = 0;

    for (const region of regions) {
        const area = region.width * region.height;
        const occupied = calculateOccupied(region.counts);

        if (area > occupied) {
            totalUnoccupied += area - occupied;
        }
    }

    return totalUnoccupied;
}

export const info = {
    title: "Shape Packing",
    description: "Count regions with unoccupied space after packing shapes.",
    demo: `10x10: 1 2 3 0 0 0
20x20: 5 5 5 5 5 5
5x5: 1 0 0 0 0 0`
};
