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

// Detect iOS/iPadOS (has limited WASM stack size)
function isIOSDevice() {
    return /iPad|iPhone|iPod/.test(navigator.userAgent) ||
        (navigator.platform === 'MacIntel' && navigator.maxTouchPoints > 1);
}

// Check if running on unsupported platform
export function checkPlatformSupport() {
    if (isIOSDevice()) {
        return {
            supported: false,
            message: 'iOS/iPadOS not supported for in-browser execution due to WebAssembly stack limitations. Download the WASM file and run locally with wasmtime.'
        };
    }
    return { supported: true };
}

// Get base URL for WASM modules
function getWasmBaseUrl() {
    const base = new URL('./', window.location.href).href;
    return base;
}

// Cache for loaded modules
const moduleCache = new Map();

// Run a jco-transpiled day solver
async function runJcoModule(day, part, input) {
    const dayStr = day.toString().padStart(2, '0');
    const baseUrl = getWasmBaseUrl();
    const moduleUrl = `${baseUrl}wasm/day${dayStr}/day${dayStr}.js`;

    try {
        // Capture stdout output
        let output = '';

        // Import the preview2-shim to configure environment
        const shim = await import('@bytecodealliance/preview2-shim');

        // Configure environment variables
        if (shim.environment) {
            shim.environment.setEnv({
                AOC_PART: part.toString(),
                AOC_INPUT: input
            });
        }

        // Configure stdout capture
        if (shim.io && shim.io.stdout) {
            shim.io.stdout.handler = (data) => {
                if (data instanceof Uint8Array) {
                    output += new TextDecoder().decode(data);
                } else {
                    output += data;
                }
            };
        }

        // Clear any cached module to ensure fresh run with new env
        // Dynamic imports are cached, so we add a cache-busting parameter
        const cacheBuster = `?t=${Date.now()}`;

        // Import and run the transpiled module
        const module = await import(/* @vite-ignore */ moduleUrl + cacheBuster);

        // The jco-transpiled module typically exports a 'run' function for command components
        // or the exports are available directly
        if (typeof module.run === 'function') {
            await module.run();
        } else if (typeof module.default === 'function') {
            await module.default();
        }

        return output.trim() || 'No output';
    } catch (e) {
        console.error(`Error running Day ${day}:`, e);

        // Check if module not found
        if (e.message && e.message.includes('Failed to fetch')) {
            return `Error: Day ${day} module not found. Build may have failed.`;
        }

        return `Error: ${e.message}`;
    }
}

// Run a specific day's solver
async function runDaySolver(day, part, input) {
    // Check platform support first
    const platform = checkPlatformSupport();
    if (!platform.supported) {
        return `Unsupported: ${platform.message}`;
    }

    return await runJcoModule(day, part, input);
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
