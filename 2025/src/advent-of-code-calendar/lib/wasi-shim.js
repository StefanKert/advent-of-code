/**
 * WASI shim for running .NET WASM modules in the browser.
 * Implements WASI snapshot preview1 functions needed by .NET runtime.
 */

export class WasiShim {
  constructor(env = {}) {
    this.env = env;
    this.stdout = '';
    this.stderr = '';
    this.memory = null;
    this.instance = null;
  }

  getImports() {
    const self = this;

    return {
      wasi_snapshot_preview1: {
        // Environment
        environ_get: (environPtr, environBufPtr) => {
          const entries = Object.entries(self.env);
          const view = new DataView(self.memory.buffer);
          let bufOffset = environBufPtr;

          for (const [key, value] of entries) {
            view.setUint32(environPtr, bufOffset, true);
            environPtr += 4;

            const envStr = `${key}=${value}\0`;
            const encoded = new TextEncoder().encode(envStr);
            new Uint8Array(self.memory.buffer, bufOffset, encoded.length).set(encoded);
            bufOffset += encoded.length;
          }
          return 0;
        },

        environ_sizes_get: (countPtr, sizePtr) => {
          const entries = Object.entries(self.env);
          const view = new DataView(self.memory.buffer);
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
          const view = new DataView(self.memory.buffer);
          let written = 0;

          for (let i = 0; i < iovsLen; i++) {
            const ptr = view.getUint32(iovsPtr + i * 8, true);
            const len = view.getUint32(iovsPtr + i * 8 + 4, true);
            const bytes = new Uint8Array(self.memory.buffer, ptr, len);
            const text = new TextDecoder().decode(bytes);

            if (fd === 1) {
              self.stdout += text;
            } else if (fd === 2) {
              self.stderr += text;
            }
            written += len;
          }

          view.setUint32(nwrittenPtr, written, true);
          return 0;
        },

        fd_read: (fd, iovsPtr, iovsLen, nreadPtr) => {
          const view = new DataView(self.memory.buffer);
          view.setUint32(nreadPtr, 0, true);
          return 0; // EOF
        },

        fd_close: (fd) => 0,
        fd_seek: (fd, offsetLo, offsetHi, whence, newOffsetPtr) => 70, // ESPIPE

        fd_fdstat_get: (fd, statPtr) => {
          const view = new DataView(self.memory.buffer);
          view.setUint8(statPtr, fd <= 2 ? 2 : 4); // character device or regular file
          view.setUint16(statPtr + 2, 0, true); // flags
          view.setBigUint64(statPtr + 8, 0n, true); // rights base
          view.setBigUint64(statPtr + 16, 0n, true); // rights inheriting
          return 0;
        },

        fd_fdstat_set_flags: (fd, flags) => 0,
        fd_prestat_get: (fd, prestatPtr) => 8, // EBADF
        fd_prestat_dir_name: (fd, pathPtr, pathLen) => 8, // EBADF
        fd_sync: (fd) => 0,
        fd_advise: (fd, offset, len, advice) => 0,
        fd_allocate: (fd, offset, len) => 0,
        fd_datasync: (fd) => 0,
        fd_filestat_get: (fd, filestatPtr) => 8, // EBADF
        fd_filestat_set_size: (fd, size) => 0,
        fd_filestat_set_times: (fd, atim, mtim, fstFlags) => 0,
        fd_pread: (fd, iovsPtr, iovsLen, offset, nreadPtr) => 8, // EBADF
        fd_pwrite: (fd, iovsPtr, iovsLen, offset, nwrittenPtr) => 8, // EBADF
        fd_readdir: (fd, bufPtr, bufLen, cookie, sizePtr) => 8, // EBADF
        fd_renumber: (from, to) => 0,
        fd_tell: (fd, offsetPtr) => 70, // ESPIPE

        // Clock
        clock_time_get: (clockId, precision, timePtr) => {
          const view = new DataView(self.memory.buffer);
          const now = BigInt(Date.now()) * 1000000n;
          view.setBigUint64(timePtr, now, true);
          return 0;
        },

        clock_res_get: (clockId, resPtr) => {
          const view = new DataView(self.memory.buffer);
          view.setBigUint64(resPtr, 1000000n, true); // 1ms in nanoseconds
          return 0;
        },

        // Process
        proc_exit: (code) => {
          throw new WasiExit(code);
        },

        proc_raise: (sig) => 0,

        // Arguments
        args_get: (argvPtr, argvBufPtr) => 0,
        args_sizes_get: (argcPtr, argvBufSizePtr) => {
          const view = new DataView(self.memory.buffer);
          view.setUint32(argcPtr, 0, true);
          view.setUint32(argvBufSizePtr, 0, true);
          return 0;
        },

        // Random
        random_get: (bufPtr, bufLen) => {
          const bytes = new Uint8Array(self.memory.buffer, bufPtr, bufLen);
          crypto.getRandomValues(bytes);
          return 0;
        },

        // Path operations
        path_create_directory: (fd, pathPtr, pathLen) => 63, // ENOTSUP
        path_filestat_get: (fd, flags, pathPtr, pathLen, filestatPtr) => 44, // ENOENT
        path_filestat_set_times: (fd, flags, pathPtr, pathLen, atim, mtim, fstFlags) => 44,
        path_link: (oldFd, oldFlags, oldPathPtr, oldPathLen, newFd, newPathPtr, newPathLen) => 63,
        path_open: (fd, dirflags, pathPtr, pathLen, oflags, fsRightsBase, fsRightsInheriting, fdflags, fdPtr) => 44,
        path_readlink: (fd, pathPtr, pathLen, bufPtr, bufLen, sizePtr) => 44,
        path_remove_directory: (fd, pathPtr, pathLen) => 63,
        path_rename: (oldFd, oldPathPtr, oldPathLen, newFd, newPathPtr, newPathLen) => 63,
        path_symlink: (oldPathPtr, oldPathLen, fd, newPathPtr, newPathLen) => 63,
        path_unlink_file: (fd, pathPtr, pathLen) => 63,

        // Polling and scheduling
        poll_oneoff: (inPtr, outPtr, nsubscriptions, neventsPtr) => {
          const view = new DataView(self.memory.buffer);
          view.setUint32(neventsPtr, 0, true);
          return 0;
        },
        sched_yield: () => 0,

        // Socket operations (stubs)
        sock_accept: (fd, flags, fdPtr) => 63,
        sock_recv: (fd, riDataPtr, riDataLen, riFlags, sizePtr, roFlagsPtr) => 63,
        sock_send: (fd, siDataPtr, siDataLen, siFlags, sizePtr) => 63,
        sock_shutdown: (fd, how) => 63,
      }
    };
  }

  setMemory(memory) {
    this.memory = memory;
  }

  getOutput() {
    return this.stdout.trim();
  }

  getStderr() {
    return this.stderr.trim();
  }

  clearOutput() {
    this.stdout = '';
    this.stderr = '';
  }
}

// Custom error for WASI exit
export class WasiExit extends Error {
  constructor(code) {
    super(`WASI exit with code ${code}`);
    this.code = code;
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
    if (e instanceof WasiExit && e.code === 0) {
      // Normal exit
    } else if (e instanceof WasiExit) {
      console.warn(`WASI module exited with code ${e.code}`);
    } else {
      throw e;
    }
  }

  return wasi.getOutput();
}
