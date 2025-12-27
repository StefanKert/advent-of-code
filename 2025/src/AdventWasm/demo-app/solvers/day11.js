// Day 11: Dependency Chain Traversal
// Walk through a directed graph counting paths to terminal node

function parseInput(input) {
    const lines = input.split('\n').filter(line => line.trim());
    const graph = new Map();

    for (const line of lines) {
        const match = line.match(/^(\w+):\s*(.*)$/);
        if (match) {
            const name = match[1];
            const deps = match[2].trim().split(/\s+/).filter(d => d);
            graph.set(name, deps);
        }
    }

    return graph;
}

export function solvePart1(input) {
    const graph = parseInput(input);
    const memo = new Map();

    function countPaths(node) {
        if (node.includes('out')) return 1n;
        if (memo.has(node)) return memo.get(node);

        const deps = graph.get(node);
        if (!deps || deps.length === 0) return 0n;

        let total = 0n;
        for (const dep of deps) {
            total += countPaths(dep);
        }

        memo.set(node, total);
        return total;
    }

    // Start from "you" node
    if (graph.has('you')) {
        return countPaths('you').toString();
    }

    // If no "you" node, try first node
    const firstNode = graph.keys().next().value;
    return firstNode ? countPaths(firstNode).toString() : '0';
}

export function solvePart2(input) {
    const graph = parseInput(input);
    const memo = new Map();

    function countPathsFiltered(node, hasDac, hasFft) {
        const newDac = hasDac || node === 'dac';
        const newFft = hasFft || node === 'fft';

        if (node.includes('out')) {
            return (newDac && newFft) ? 1n : 0n;
        }

        const key = `${node}:${newDac}:${newFft}`;
        if (memo.has(key)) return memo.get(key);

        const deps = graph.get(node);
        if (!deps || deps.length === 0) return 0n;

        let total = 0n;
        for (const dep of deps) {
            total += countPathsFiltered(dep, newDac, newFft);
        }

        memo.set(key, total);
        return total;
    }

    // Start from "svr" node for part 2
    if (graph.has('svr')) {
        return countPathsFiltered('svr', false, false).toString();
    }

    return '0';
}

export const info = {
    title: "Dependency Chains",
    description: "Count paths through a dependency graph, filtering by required nodes.",
    demo: `you: a b
a: c d
b: d e
c: out1
d: out2
e: out3`
};
