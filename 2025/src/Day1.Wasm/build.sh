#!/bin/bash
set -e

echo "==================================="
echo "Building Day1 WASM Plugin"
echo "==================================="

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

echo ""
echo "Step 1: Restoring NuGet packages..."
dotnet restore

echo ""
echo "Step 2: Building and publishing the WASM component..."
dotnet publish -c Release

echo ""
echo "Step 3: Setting up demo application..."
cd demo-app

if [ ! -d "node_modules" ]; then
    echo "Installing npm dependencies..."
    npm install
fi

echo ""
echo "Step 4: Transpiling WASM component to JavaScript..."
npm run transpile

echo ""
echo "==================================="
echo "Build Complete!"
echo "==================================="
echo ""
echo "The WASM file is located at:"
echo "  bin/Release/net9.0/wasi-wasm/publish/Day1.Wasm.wasm"
echo ""
echo "To run the demo application:"
echo "  cd demo-app"
echo "  npm run dev"
echo ""
