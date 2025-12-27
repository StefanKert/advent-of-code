import { defineConfig } from 'vite';

export default defineConfig({
  base: process.env.BASE_URL || './',
  build: {
    outDir: 'dist',
    target: 'esnext',
  },
  server: {
    port: 3000,
    open: true,
  },
  optimizeDeps: {
    exclude: ['@bytecodealliance/preview2-shim'],
  },
});
