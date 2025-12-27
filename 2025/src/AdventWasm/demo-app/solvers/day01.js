// Day 1: Rotation Counter
// Track passes through position 0 on a circular dial (0-99)

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

export function solvePart1(input) {
    const rotations = parseInput(input);
    let position = START_POSITION;
    let zeroCounts = 0;

    for (const rotation of rotations) {
        if (rotation.direction === 'L') {
            for (let j = 0; j < rotation.steps; j++) {
                position--;
                if (position < MIN_POINT) position = MAX_POINT;
            }
        } else {
            for (let j = 0; j < rotation.steps; j++) {
                position++;
                if (position > MAX_POINT) position = MIN_POINT;
            }
        }

        if (position === 0) zeroCounts++;
    }

    return zeroCounts;
}

export function solvePart2(input) {
    const rotations = parseInput(input);
    let position = START_POSITION;
    let zeroCounts = 0;

    for (const rotation of rotations) {
        if (rotation.direction === 'L') {
            for (let j = 0; j < rotation.steps; j++) {
                position--;
                if (position < MIN_POINT) position = MAX_POINT;
                if (position === 0) zeroCounts++;
            }
        } else {
            for (let j = 0; j < rotation.steps; j++) {
                position++;
                if (position > MAX_POINT) position = MIN_POINT;
                if (position === 0) zeroCounts++;
            }
        }
    }

    return zeroCounts;
}

export const info = {
    title: "Rotation Counter",
    description: "Count passes through position zero on a circular dial (0-99).",
    demo: `L68
L30
R48
L5
R60
L55
L1
L99
R14
L82`
};
