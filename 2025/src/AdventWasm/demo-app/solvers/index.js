// This file will be replaced by the transpiled WASM component during build
// For local development without WASM, this provides a stub implementation

// Day information with demo inputs
export const dayInfo = {
    1: {
        title: 'Rotation Counter',
        description: 'Count passes through position zero on a circular dial.',
        demo: 'D:10\nD:50\nU:30\nD:20\nD:15'
    },
    2: {
        title: 'ID Validation',
        description: 'Find invalid IDs based on palindrome and repeat patterns.',
        demo: 'ABC123CBA123\nXYZ456ZYX789\nAAA111AAA111\nDEF123FED456'
    },
    3: {
        title: 'Battery Aggregation',
        description: 'Aggregate battery values recursively to find maximum.',
        demo: '10 20 30\n5 15 25\n8 12 18'
    },
    4: {
        title: 'Paper Roll Detection',
        description: 'Count rolls with specific neighbor patterns.',
        demo: '###...\n..###.\n.#.#.#'
    },
    5: {
        title: 'Range Merging',
        description: 'Merge overlapping ranges and validate IDs.',
        demo: '1-5\n3-8\n10-15\n12-20'
    },
    6: {
        title: 'Operator Aggregation',
        description: 'Aggregate rows/columns using + and * operators from the last row.',
        demo: '123\n456\n789\n+*+'
    },
    7: {
        title: 'Beam Propagation',
        description: 'Simulate light beams splitting at ^ characters as they move down.',
        demo: '...S...\n.......\n...^...\n.......\n..^.^..'
    },
    8: {
        title: '3D Clustering',
        description: 'Build minimum spanning tree of 3D points, find largest clusters.',
        demo: '0,0,0\n1,1,1\n10,10,10\n11,11,11\n100,100,100'
    },
    9: {
        title: '2D Distance Calculation',
        description: 'Find largest distance pair and calculate bounding box area.',
        demo: '0,0\n5,5\n10,0\n5,-5'
    },
    10: {
        title: 'Beam Tree Traversal',
        description: 'Walk beam paths through a map, counting splits and distinct paths.',
        demo: '...S...\n.......\n...^...\n.......\n..^.^..'
    },
    11: {
        title: 'Dependency Chain Traversal',
        description: 'Walk dependency chains and count paths to out with optional filtering.',
        demo: 'start: you next\nnext: dac fft\ndac: out\nfft: out'
    },
    12: {
        title: 'Shape Packing',
        description: 'Check if shapes fit in regions based on occupied squares.',
        demo: '###\n.#.\n.#.\n\n10x10: 1 0 0 0 0 0'
    }
};

// Placeholder message for when WASM is not loaded
const wasmNotLoadedMessage = 'WASM module not loaded - build the C# project first';

// This will be replaced by the actual WASM solver during build
let wasmSolver = null;

// Try to load the WASM module dynamically
async function loadWasmSolver() {
    try {
        const module = await import('./advent/advent.mjs');
        wasmSolver = module;
        console.log('WASM solver loaded successfully');
        return true;
    } catch (e) {
        console.warn('WASM solver not available, using stub implementation:', e.message);
        return false;
    }
}

// Attempt to load WASM on module initialization
loadWasmSolver();

// Solver wrapper that delegates to WASM when available
function createSolver(day) {
    return {
        solvePart1(input) {
            if (wasmSolver) {
                return wasmSolver.solver.solve(day, 1, input);
            }
            return wasmNotLoadedMessage;
        },
        solvePart2(input) {
            if (wasmSolver) {
                return wasmSolver.solver.solve(day, 2, input);
            }
            return wasmNotLoadedMessage;
        }
    };
}

// Export solvers for all days
export const solvers = {
    1: createSolver(1),
    2: createSolver(2),
    3: createSolver(3),
    4: createSolver(4),
    5: createSolver(5),
    6: createSolver(6),
    7: createSolver(7),
    8: createSolver(8),
    9: createSolver(9),
    10: createSolver(10),
    11: createSolver(11),
    12: createSolver(12),
};

// Export function to check if WASM is loaded
export function isWasmLoaded() {
    return wasmSolver !== null;
}

// Export function to manually load WASM
export { loadWasmSolver };
