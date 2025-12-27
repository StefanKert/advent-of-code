// Day 10: Beam Tree Traversal
// Recursive beam propagation with path counting (tree-based)

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines.map(line => line.split(''));
}

function findStart(grid) {
    for (let row = 0; row < grid.length; row++) {
        for (let col = 0; col < grid[0].length; col++) {
            if (grid[row][col] === 'S') return { row, col };
        }
    }
    return { row: 0, col: 0 };
}

export function solvePart1(input) {
    const grid = parseInput(input);
    const start = findStart(grid);
    let beams = [start.col];
    let splits = 0;

    for (let row = start.row + 1; row < grid.length; row++) {
        const newBeams = [];

        for (const col of beams) {
            if (col >= 0 && col < grid[0].length && grid[row][col] === '^') {
                splits++;
                newBeams.push(col - 1);
                newBeams.push(col + 1);
            } else {
                newBeams.push(col);
            }
        }

        beams = newBeams.filter(c => c >= 0 && c < grid[0].length);
    }

    return splits;
}

export function solvePart2(input) {
    const grid = parseInput(input);
    const start = findStart(grid);

    function countPaths(col, row, visited) {
        if (row >= grid.length) return 1n;
        if (col < 0 || col >= grid[0].length) return 0n;

        const key = `${col},${row}`;
        if (visited.has(key)) return visited.get(key);

        let result;
        if (grid[row][col] === '^') {
            result = countPaths(col - 1, row + 1, visited) + countPaths(col + 1, row + 1, visited);
        } else {
            result = countPaths(col, row + 1, visited);
        }

        visited.set(key, result);
        return result;
    }

    const memo = new Map();
    return countPaths(start.col, start.row + 1, memo).toString();
}

export const info = {
    title: "Beam Tree Traversal",
    description: "Count all possible paths through beam splits using tree traversal.",
    demo: `....S....
.........
....^....
...^.^...
..^...^..
.^.....^.
..........`
};
