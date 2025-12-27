// Day 6: Operator-Based Aggregation
// Aggregate rows or columns using operators in last row

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines;
}

function findOperatorRanges(operatorLine) {
    const ranges = [];
    let start = 0;
    let currentOp = null;

    for (let i = 0; i < operatorLine.length; i++) {
        const char = operatorLine[i];
        if (char === '+' || char === '*') {
            if (currentOp !== null) {
                ranges.push({ start, end: i, op: currentOp });
            }
            currentOp = char;
            start = i;
        }
    }

    if (currentOp !== null) {
        ranges.push({ start, end: operatorLine.length, op: currentOp });
    }

    return ranges;
}

export function solvePart1(input) {
    const lines = parseInput(input);
    if (lines.length < 2) return 0;

    const operatorLine = lines[lines.length - 1];
    const dataLines = lines.slice(0, -1);
    const ranges = findOperatorRanges(operatorLine);

    let total = 0n;

    for (const range of ranges) {
        let aggregated = range.op === '+' ? 0n : 1n;

        for (const line of dataLines) {
            const segment = line.slice(range.start, range.end).trim();
            const num = segment ? BigInt(segment.replace(/\s+/g, '')) : 0n;

            if (range.op === '+') {
                aggregated += num;
            } else {
                aggregated *= num === 0n ? 1n : num;
            }
        }

        total += aggregated;
    }

    return total.toString();
}

export function solvePart2(input) {
    const lines = parseInput(input);
    if (lines.length < 2) return 0;

    const operatorLine = lines[lines.length - 1];
    const dataLines = lines.slice(0, -1);
    const ranges = findOperatorRanges(operatorLine);

    // Transpose: work with columns instead of rows
    const maxLen = Math.max(...lines.map(l => l.length));
    let total = 0n;

    for (const range of ranges) {
        for (let col = range.start; col < range.end && col < maxLen; col++) {
            let aggregated = range.op === '+' ? 0n : 1n;

            for (const line of dataLines) {
                const char = line[col];
                if (char && /\d/.test(char)) {
                    const num = BigInt(char);
                    if (range.op === '+') {
                        aggregated += num;
                    } else {
                        aggregated *= num;
                    }
                }
            }

            total += aggregated;
        }
    }

    return total.toString();
}

export const info = {
    title: "Operator Aggregation",
    description: "Aggregate rows/columns using + and * operators from the last row.",
    demo: `6 5 8 7
7 1 4 8
3 6 5 2
* + * +`
};
