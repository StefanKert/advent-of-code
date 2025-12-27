/**
 * Minimal WASI shim for running AoC WASM solvers in the browser.
 * Only implements the functions needed: environment variables and stdout.
 */

export class WasiShim {
  constructor(env = {}) {
    this.env = env;
    this.stdout = '';
    this.stderr = '';
    this.memory = null;
  }

  getImports() {
    return {
      wasi_snapshot_preview1: {
        // Environment
        environ_get: (environPtr, environBufPtr) => {
          const entries = Object.entries(this.env);
          const view = new DataView(this.memory.buffer);
          let bufOffset = environBufPtr;

          for (const [key, value] of entries) {
            view.setUint32(environPtr, bufOffset, true);
            environPtr += 4;

            const envStr = `${key}=${value}\0`;
            const encoded = new TextEncoder().encode(envStr);
            new Uint8Array(this.memory.buffer, bufOffset, encoded.length).set(encoded);
            bufOffset += encoded.length;
          }
          return 0;
        },

        environ_sizes_get: (countPtr, sizePtr) => {
          const entries = Object.entries(this.env);
          const view = new DataView(this.memory.buffer);
          view.setUint32(countPtr, entries.length, true);

          let totalSize = 0;
          for (const [key, value] of entries) {
            totalSize += new TextEncoder().encode(`${key}=${value}\0`).length;
          }
          view.setUint32(sizePtr, totalSize, true);
          return 0;
        },

        // File descriptors
        fd_write: (fd, iovsPtr, iovsLen, nwrittenPtr) => {
          const view = new DataView(this.memory.buffer);
          let written = 0;

          for (let i = 0; i < iovsLen; i++) {
            const ptr = view.getUint32(iovsPtr + i * 8, true);
            const len = view.getUint32(iovsPtr + i * 8 + 4, true);
            const bytes = new Uint8Array(this.memory.buffer, ptr, len);
            const text = new TextDecoder().decode(bytes);

            if (fd === 1) {
              this.stdout += text;
            } else if (fd === 2) {
              this.stderr += text;
            }
            written += len;
          }

          view.setUint32(nwrittenPtr, written, true);
          return 0;
        },

        fd_read: (fd, iovsPtr, iovsLen, nreadPtr) => {
          const view = new DataView(this.memory.buffer);
          view.setUint32(nreadPtr, 0, true);
          return 0;
        },

        fd_close: () => 0,
        fd_seek: () => 0,
        fd_fdstat_get: (fd, statPtr) => {
          const view = new DataView(this.memory.buffer);
          // Set filetype to character device (2)
          view.setUint8(statPtr, 2);
          return 0;
        },
        fd_prestat_get: () => 8, // EBADF - no preopened directories
        fd_prestat_dir_name: () => 8,

        // Clock
        clock_time_get: (clockId, precision, timePtr) => {
          const view = new DataView(this.memory.buffer);
          const now = BigInt(Date.now()) * 1000000n;
          view.setBigUint64(timePtr, now, true);
          return 0;
        },

        // Process
        proc_exit: (code) => {
          throw new Error(`Process exited with code ${code}`);
        },

        args_get: () => 0,
        args_sizes_get: (argcPtr, argvBufSizePtr) => {
          const view = new DataView(this.memory.buffer);
          view.setUint32(argcPtr, 0, true);
          view.setUint32(argvBufSizePtr, 0, true);
          return 0;
        },

        // Random
        random_get: (bufPtr, bufLen) => {
          const bytes = new Uint8Array(this.memory.buffer, bufPtr, bufLen);
          crypto.getRandomValues(bytes);
          return 0;
        },

        // Stubs for other WASI functions
        path_open: () => 44, // ENOENT
        path_filestat_get: () => 44,
        fd_fdstat_set_flags: () => 0,
        fd_sync: () => 0,
        poll_oneoff: () => 0,
        sched_yield: () => 0,
      }
    };
  }

  setMemory(memory) {
    this.memory = memory;
  }

  getOutput() {
    return this.stdout.trim();
  }

  clearOutput() {
    this.stdout = '';
    this.stderr = '';
  }
}

/**
 * Load and run a WASI WASM module
 */
export async function runWasiModule(wasmUrl, env = {}) {
  const wasi = new WasiShim(env);

  const response = await fetch(wasmUrl);
  const wasmBytes = await response.arrayBuffer();

  const imports = wasi.getImports();
  const { instance } = await WebAssembly.instantiate(wasmBytes, imports);

  wasi.setMemory(instance.exports.memory);

  try {
    if (instance.exports._start) {
      instance.exports._start();
    } else if (instance.exports.main) {
      instance.exports.main();
    }
  } catch (e) {
    if (!e.message.includes('Process exited with code 0')) {
      throw e;
    }
  }

  return wasi.getOutput();
}
