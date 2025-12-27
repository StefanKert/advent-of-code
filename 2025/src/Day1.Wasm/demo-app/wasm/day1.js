// JavaScript implementation of the Day1 solver
// This serves as both a development mock and fallback implementation
// In production with .NET, run `npm run transpile` to generate from WASM
//
// Note: This file matches the WIT interface exported by the C# WASM component

const MAX_POINT = 99;
const MIN_POINT = 0;
const START_POSITION = 50;

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    const rotations = [];

    for (const line of lines) {
        const match = line.trim().match(/([LR])(\d+)/);
        if (match) {
            rotations.push({
                direction: match[1],
                steps: parseInt(match[2], 10)
            });
        }
    }

    return rotations;
}

function solvePart1(input) {
    const rotations = parseInput(input);
    let position = START_POSITION;
    let zeroCounts = 0;

    for (const rotation of rotations) {
        if (rotation.direction === 'L') {
            for (let j = 0; j < rotation.steps; j++) {
                position--;
                if (position < MIN_POINT) {
                    position = MAX_POINT;
                }
            }
        } else if (rotation.direction === 'R') {
            for (let j = 0; j < rotation.steps; j++) {
                position++;
                if (position > MAX_POINT) {
                    position = MIN_POINT;
                }
            }
        }

        if (position === 0) {
            zeroCounts++;
        }
    }

    return zeroCounts;
}

function solvePart2(input) {
    const rotations = parseInput(input);
    let position = START_POSITION;
    let zeroCounts = 0;

    for (const rotation of rotations) {
        if (rotation.direction === 'L') {
            for (let j = 0; j < rotation.steps; j++) {
                position--;
                if (position < MIN_POINT) {
                    position = MAX_POINT;
                }
                if (position === 0) {
                    zeroCounts++;
                }
            }
        } else if (rotation.direction === 'R') {
            for (let j = 0; j < rotation.steps; j++) {
                position++;
                if (position > MAX_POINT) {
                    position = MIN_POINT;
                }
                if (position === 0) {
                    zeroCounts++;
                }
            }
        }
    }

    return zeroCounts;
}

function solveAll(input) {
    const part1 = solvePart1(input);
    const part2 = solvePart2(input);
    return `Part 1: ${part1}\nPart 2: ${part2}`;
}

// Export the solver interface matching the WIT definition
export const solver = {
    solvePart1,
    solvePart2,
    solveAll
};
