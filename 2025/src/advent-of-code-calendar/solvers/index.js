// Day information with demo inputs
export const dayInfo = {
    1: {
        title: 'Rotation Counter',
        description: 'Count passes through position zero on a circular dial.',
        demo: 'L10\nR50\nL30\nR20\nL15'
    },
    2: {
        title: 'ID Validation',
        description: 'Find invalid IDs based on palindrome and repeat patterns.',
        demo: '123123-123130'
    },
    3: {
        title: 'Battery Aggregation',
        description: 'Aggregate battery values recursively to find maximum.',
        demo: '123456789\n987654321\n135792468'
    },
    4: {
        title: 'Paper Roll Detection',
        description: 'Count rolls with specific neighbor patterns.',
        demo: '@@@...\n..@@@.\n.@.@.@'
    },
    5: {
        title: 'Range Merging',
        description: 'Merge overlapping ranges and validate IDs.',
        demo: '1-5\n3-8\n10-15\n\n4\n7\n12\n20'
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
        title: 'Polygon Rectangle',
        description: 'Find largest rectangles between coordinates and inside polygons.',
        demo: '0,0\n10,0\n10,10\n0,10'
    },
    10: {
        title: 'Button Puzzle',
        description: 'Find minimum button presses to match light patterns.',
        demo: '[##..](0,1)(2,3){1,1,0,0}'
    },
    11: {
        title: 'Dependency Chain',
        description: 'Walk dependency chains and count paths.',
        demo: 'start: you next\nnext: dac fft\ndac: out\nfft: out'
    },
    12: {
        title: 'Shape Packing',
        description: 'Check if shapes fit in regions based on occupied squares.',
        demo: '0:\n###\n.#.\n\n5x5: 1'
    }
};

// Get WASM file URL for a day (returns absolute URL for worker compatibility)
function getWasmUrl(day) {
    const dayStr = day.toString().padStart(2, '0');
    // Use document base URL since WASM files are in public/wasm (copied to site root)
    // import.meta.url points to /assets/ which is wrong for public folder files
    const base = new URL('./', window.location.href).href;
    return `${base}wasm/day${dayStr}.wasm`;
}

// Worker management
let wasmWorker = null;
let workerMessageId = 0;
const pendingRequests = new Map();

// Initialize the WebWorker
function initWorker() {
    if (wasmWorker) return true;

    try {
        // Create worker from the worker file
        wasmWorker = new Worker(
            new URL('./wasm-worker.js', import.meta.url),
            { type: 'module' }
        );

        wasmWorker.onmessage = (e) => {
            const { id, success, result, error } = e.data;
            const pending = pendingRequests.get(id);
            if (pending) {
                pendingRequests.delete(id);
                if (success) {
                    pending.resolve(result);
                } else {
                    pending.reject(new Error(error));
                }
            }
        };

        wasmWorker.onerror = (e) => {
            console.error('Worker error:', e);
            // Reject all pending requests
            for (const [id, pending] of pendingRequests) {
                pending.reject(new Error(`Worker error: ${e.message}`));
                pendingRequests.delete(id);
            }
        };

        return true;
    } catch (e) {
        console.warn('Failed to initialize WebWorker:', e);
        return false;
    }
}

// Run WASM in WebWorker
function runWasiModuleInWorker(wasmUrl, env) {
    return new Promise((resolve, reject) => {
        const id = ++workerMessageId;
        pendingRequests.set(id, { resolve, reject });

        // Set a timeout for the request
        const timeout = setTimeout(() => {
            if (pendingRequests.has(id)) {
                pendingRequests.delete(id);
                reject(new Error('WASM execution timed out'));
            }
        }, 30000); // 30 second timeout

        // Wrap resolve/reject to clear timeout
        const origResolve = resolve;
        const origReject = reject;
        pendingRequests.set(id, {
            resolve: (result) => { clearTimeout(timeout); origResolve(result); },
            reject: (error) => { clearTimeout(timeout); origReject(error); }
        });

        wasmWorker.postMessage({ id, wasmUrl, env });
    });
}

// Run a specific day's solver
async function runDaySolver(day, part, input) {
    const wasmUrl = getWasmUrl(day);
    const env = {
        AOC_PART: part.toString(),
        AOC_INPUT: input
    };

    try {
        // Try to use WebWorker (better for iOS Safari stack limits)
        if (initWorker()) {
            const output = await runWasiModuleInWorker(wasmUrl, env);
            return output.trim() || 'No output';
        } else {
            throw new Error('WebWorker not available');
        }
    } catch (e) {
        console.error(`Error running Day ${day}:`, e);
        return `Error: ${e.message}`;
    }
}

// Create solver wrapper for a specific day
function createSolver(day) {
    return {
        async solvePart1(input) {
            return await runDaySolver(day, 1, input);
        },
        async solvePart2(input) {
            return await runDaySolver(day, 2, input);
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
