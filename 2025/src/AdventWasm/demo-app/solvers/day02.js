// Day 2: Invalid ID Detection
// Validate numbers based on palindromic and repeating pattern properties

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    const numbers = [];

    for (const line of lines) {
        const ranges = line.split(',');
        for (const range of ranges) {
            const match = range.trim().match(/(\d+)-(\d+)/);
            if (match) {
                const start = parseInt(match[1], 10);
                const end = parseInt(match[2], 10);
                for (let i = start; i <= end; i++) {
                    numbers.push(i);
                }
            }
        }
    }

    return numbers;
}

function isPalindromicHalves(num) {
    const str = num.toString();
    if (str.length % 2 !== 0) return false;
    const half = str.length / 2;
    return str.slice(0, half) === str.slice(half);
}

function hasRepeatingPattern(num) {
    const str = num.toString();
    const maxPrefixLen = Math.floor(str.length / 2);

    for (let prefixLen = 1; prefixLen <= maxPrefixLen; prefixLen++) {
        const pattern = str.slice(0, prefixLen);
        let pos = 0;
        let valid = true;

        while (pos < str.length) {
            if (!str.slice(pos).startsWith(pattern)) {
                valid = false;
                break;
            }
            pos += pattern.length;
        }

        if (valid && pos === str.length) return true;
    }

    return false;
}

export function solvePart1(input) {
    const numbers = parseInput(input);
    let sum = 0;

    for (const num of numbers) {
        if (isPalindromicHalves(num)) {
            sum += num;
        }
    }

    return sum;
}

export function solvePart2(input) {
    const numbers = parseInput(input);
    let sum = 0;

    for (const num of numbers) {
        if (hasRepeatingPattern(num)) {
            sum += num;
        }
    }

    return sum;
}

export const info = {
    title: "Invalid ID Detection",
    description: "Validate numbers based on palindromic halves and repeating patterns.",
    demo: `11-22,95-115,998-1012,1188511880-1188511890`
};
