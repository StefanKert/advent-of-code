// Advent of Code 2025 - All Day Solvers
// This module exports all available solvers and their metadata

import * as day01 from './day01.js';
import * as day02 from './day02.js';
import * as day03 from './day03.js';
import * as day04 from './day04.js';
import * as day05 from './day05.js';
import * as day06 from './day06.js';
import * as day07 from './day07.js';
import * as day08 from './day08.js';
import * as day09 from './day09.js';
import * as day10 from './day10.js';
import * as day11 from './day11.js';
import * as day12 from './day12.js';

// Export solvers indexed by day number
export const solvers = {
    1: { solvePart1: day01.solvePart1, solvePart2: day01.solvePart2 },
    2: { solvePart1: day02.solvePart1, solvePart2: day02.solvePart2 },
    3: { solvePart1: day03.solvePart1, solvePart2: day03.solvePart2 },
    4: { solvePart1: day04.solvePart1, solvePart2: day04.solvePart2 },
    5: { solvePart1: day05.solvePart1, solvePart2: day05.solvePart2 },
    6: { solvePart1: day06.solvePart1, solvePart2: day06.solvePart2 },
    7: { solvePart1: day07.solvePart1, solvePart2: day07.solvePart2 },
    8: { solvePart1: day08.solvePart1, solvePart2: day08.solvePart2 },
    9: { solvePart1: day09.solvePart1, solvePart2: day09.solvePart2 },
    10: { solvePart1: day10.solvePart1, solvePart2: day10.solvePart2 },
    11: { solvePart1: day11.solvePart1, solvePart2: day11.solvePart2 },
    12: { solvePart1: day12.solvePart1, solvePart2: day12.solvePart2 },
};

// Export day info (titles, descriptions, demo inputs)
export const dayInfo = {
    1: day01.info,
    2: day02.info,
    3: day03.info,
    4: day04.info,
    5: day05.info,
    6: day06.info,
    7: day07.info,
    8: day08.info,
    9: day09.info,
    10: day10.info,
    11: day11.info,
    12: day12.info,
};

// Utility function to get available days
export function getAvailableDays() {
    return Object.keys(solvers).map(Number).sort((a, b) => a - b);
}

// Utility function to solve a specific day and part
export function solve(day, part, input) {
    const solver = solvers[day];
    if (!solver) {
        throw new Error(`No solver available for day ${day}`);
    }

    if (part === 1) {
        return solver.solvePart1(input);
    } else if (part === 2) {
        return solver.solvePart2(input);
    } else {
        throw new Error(`Invalid part: ${part}. Must be 1 or 2.`);
    }
}
