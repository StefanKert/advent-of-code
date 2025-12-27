import { defineConfig } from 'vite';

export default defineConfig({
  base: process.env.BASE_URL || './',
  build: {
    outDir: 'dist',
    target: 'esnext',
    rollupOptions: {
      output: {
        // Ensure wasm files maintain their paths
        assetFileNames: (assetInfo) => {
          if (assetInfo.name && assetInfo.name.endsWith('.wasm')) {
            return '[name][extname]';
          }
          return 'assets/[name]-[hash][extname]';
        },
      },
    },
  },
  server: {
    port: 3000,
    open: true,
  },
  // Include .wasm files as assets
  assetsInclude: ['**/*.wasm'],
});
