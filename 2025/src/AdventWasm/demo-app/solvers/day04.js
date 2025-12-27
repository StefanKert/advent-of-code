// Day 4: Paper Roll Location Detection
// Count paper roll locations based on neighbor density

const DIRECTIONS = [
    [-1, -1], [-1, 0], [-1, 1],
    [0, -1],           [0, 1],
    [1, -1],  [1, 0],  [1, 1]
];

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines.map(line => line.split(''));
}

function countNeighbors(matrix, row, col, char) {
    let count = 0;
    for (const [dr, dc] of DIRECTIONS) {
        const nr = row + dr;
        const nc = col + dc;
        if (nr >= 0 && nr < matrix.length && nc >= 0 && nc < matrix[0].length) {
            if (matrix[nr][nc] === char) count++;
        }
    }
    return count;
}

export function solvePart1(input) {
    const matrix = parseInput(input);
    let count = 0;

    for (let r = 0; r < matrix.length; r++) {
        for (let c = 0; c < matrix[0].length; c++) {
            if (matrix[r][c] === '@') {
                const neighbors = countNeighbors(matrix, r, c, '@');
                if (neighbors < 4) count++;
            }
        }
    }

    return count;
}

export function solvePart2(input) {
    const matrix = parseInput(input);
    let totalRemoved = 0;

    let changed = true;
    while (changed) {
        changed = false;
        for (let r = 0; r < matrix.length; r++) {
            for (let c = 0; c < matrix[0].length; c++) {
                if (matrix[r][c] === '@') {
                    const neighbors = countNeighbors(matrix, r, c, '@');
                    if (neighbors < 4) {
                        matrix[r][c] = '.';
                        totalRemoved++;
                        changed = true;
                    }
                }
            }
        }
    }

    return totalRemoved;
}

export const info = {
    title: "Paper Roll Detection",
    description: "Count paper roll locations based on neighbor density (< 4 neighbors).",
    demo: `@@.@@@@@@@
@@@.@@@@@@
@@@@@@@@@@
@@@@@@.@@@
@@@@@@@@@@
@@@@@.@@@@
@@@@@@@@@@
@@@@@@@@@@
@@@@@@@.@@
@@@@@@@@@@`
};
