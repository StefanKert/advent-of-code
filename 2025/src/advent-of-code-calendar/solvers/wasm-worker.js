// WebWorker for running WASI WASM modules
// Running in a worker may help with iOS Safari stack limitations

import { WASI, File, OpenFile, ConsoleStdout } from '@bjorn3/browser_wasi_shim';

// Handle messages from main thread
self.onmessage = async function(e) {
    const { id, wasmUrl, env } = e.data;

    try {
        const result = await runWasiModule(wasmUrl, env);
        self.postMessage({ id, success: true, result });
    } catch (error) {
        self.postMessage({ id, success: false, error: error.message });
    }
};

async function runWasiModule(wasmUrl, env = {}) {
    let stdout = '';
    let stderr = '';

    // Convert env object to WASI format: ["KEY=value", ...]
    const envArray = Object.entries(env).map(([key, value]) => `${key}=${value}`);

    // Set up file descriptors
    const fds = [
        new OpenFile(new File([])), // stdin (empty)
        ConsoleStdout.lineBuffered(msg => { stdout += msg + '\n'; }), // stdout
        ConsoleStdout.lineBuffered(msg => { stderr += msg + '\n'; }), // stderr
    ];

    // Initialize WASI
    const wasi = new WASI(['solver'], envArray, fds);

    // Load and instantiate WebAssembly module
    const response = await fetch(wasmUrl);
    if (!response.ok) {
        throw new Error(`Failed to fetch ${wasmUrl}: ${response.status}`);
    }

    const wasmBytes = await response.arrayBuffer();
    const wasmModule = await WebAssembly.compile(wasmBytes);
    const instance = await WebAssembly.instantiate(wasmModule, {
        wasi_snapshot_preview1: wasi.wasiImport
    });

    // Execute the WASI program
    try {
        wasi.start(instance);
    } catch (e) {
        // WASI programs exit by throwing - check if it's a normal exit
        if (e.message && e.message.includes('exit')) {
            // Normal exit, ignore
        } else {
            throw e;
        }
    }

    return stdout;
}
