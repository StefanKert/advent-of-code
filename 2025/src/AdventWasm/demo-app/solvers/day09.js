// Day 9: 2D Area Calculation
// Calculate bounding box area from coordinate pairs

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines.map(line => {
        const [x, y] = line.split(',').map(Number);
        return { x, y };
    });
}

function distance2D(p1, p2) {
    const dx = p1.x - p2.x;
    const dy = p1.y - p2.y;
    return Math.sqrt(dx * dx + dy * dy);
}

function findFarthestPair(points) {
    let maxDist = 0;
    let pair = [points[0], points[1]];

    for (let i = 0; i < points.length; i++) {
        for (let j = i + 1; j < points.length; j++) {
            const dist = distance2D(points[i], points[j]);
            if (dist > maxDist) {
                maxDist = dist;
                pair = [points[i], points[j]];
            }
        }
    }

    return pair;
}

export function solvePart1(input) {
    const points = parseInput(input);
    if (points.length < 2) return 0;

    const [p1, p2] = findFarthestPair(points);

    const left = Math.min(p1.x, p2.x);
    const right = Math.max(p1.x, p2.x);
    const bottom = Math.min(p1.y, p2.y);
    const top = Math.max(p1.y, p2.y);

    const area = BigInt(right - left + 1) * BigInt(top - bottom + 1);
    return area.toString();
}

export function solvePart2(input) {
    // Part 2 uses same logic but might have different interpretation
    // Based on the code, it seems to be the same calculation
    return solvePart1(input);
}

export const info = {
    title: "2D Area Calculation",
    description: "Find farthest pair of points and calculate bounding box area.",
    demo: `0,0
10,0
10,10
0,10
5,5`
};
