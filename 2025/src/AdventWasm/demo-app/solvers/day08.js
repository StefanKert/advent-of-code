// Day 8: 3D Point Clustering via Minimum Spanning Tree
// Connect 3D points with shortest edges, identify clusters

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    return lines.map(line => {
        const [x, y, z] = line.split(',').map(Number);
        return { x, y, z };
    });
}

function distance3D(p1, p2) {
    const dx = p1.x - p2.x;
    const dy = p1.y - p2.y;
    const dz = p1.z - p2.z;
    return Math.sqrt(dx * dx + dy * dy + dz * dz);
}

function buildClusters(points) {
    // Calculate all pairwise distances
    const edges = [];
    for (let i = 0; i < points.length; i++) {
        for (let j = i + 1; j < points.length; j++) {
            edges.push({
                i,
                j,
                dist: distance3D(points[i], points[j])
            });
        }
    }

    // Sort by distance
    edges.sort((a, b) => a.dist - b.dist);

    // Union-Find structure
    const parent = points.map((_, i) => i);
    const rank = points.map(() => 0);

    function find(x) {
        if (parent[x] !== x) {
            parent[x] = find(parent[x]);
        }
        return parent[x];
    }

    function union(x, y) {
        const px = find(x);
        const py = find(y);
        if (px === py) return false;

        if (rank[px] < rank[py]) {
            parent[px] = py;
        } else if (rank[px] > rank[py]) {
            parent[py] = px;
        } else {
            parent[py] = px;
            rank[px]++;
        }
        return true;
    }

    // Build MST
    let edgesUsed = 0;
    let lastDist = 0;

    for (const edge of edges) {
        if (union(edge.i, edge.j)) {
            edgesUsed++;
            lastDist = edge.dist;
            if (edgesUsed === points.length - 1) break;
        }
    }

    // Count cluster sizes
    const clusters = new Map();
    for (let i = 0; i < points.length; i++) {
        const root = find(i);
        clusters.set(root, (clusters.get(root) || 0) + 1);
    }

    return {
        clusters: [...clusters.values()].sort((a, b) => b - a),
        lastDist: Math.round(lastDist)
    };
}

export function solvePart1(input) {
    const points = parseInput(input);
    const { lastDist } = buildClusters(points);
    return lastDist;
}

export function solvePart2(input) {
    const points = parseInput(input);
    const { clusters } = buildClusters(points);

    // Multiply top 3 cluster sizes
    const top3 = clusters.slice(0, 3);
    const product = top3.reduce((a, b) => a * b, 1);

    return product;
}

export const info = {
    title: "3D Clustering",
    description: "Build minimum spanning tree of 3D points, find largest clusters.",
    demo: `0,0,0
1,1,1
2,2,2
10,10,10
11,11,11
20,20,20`
};
