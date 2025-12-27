# Advent of Code 2025 - Day 1 WASM Plugin

This project compiles the Day 1 solution to WebAssembly using the WASI Component Model, allowing it to be loaded as a plugin in web applications.

## Prerequisites

- .NET 9.0 SDK or later
- Node.js 18+ and npm
- [WASI SDK](https://github.com/WebAssembly/wasi-sdk) (installed automatically by the NuGet package)

## Project Structure

```
Day1.Wasm/
├── Day1.Wasm.csproj      # C# project file with WASI configuration
├── Program.cs            # Entry point that registers the solver
├── Day1Solver.cs         # Solver implementation
├── wit/
│   └── world.wit         # WIT interface definition
├── demo-app/
│   ├── package.json      # Node.js dependencies
│   ├── index.html        # Demo web application
│   ├── vite.config.js    # Vite configuration
│   └── wasm/             # Transpiled JavaScript (generated)
├── build.sh              # Linux/macOS build script
├── build.cmd             # Windows build script
├── input.txt             # Puzzle input
└── demo.txt              # Demo input
```

## Building

### Quick Build

**Linux/macOS:**
```bash
chmod +x build.sh
./build.sh
```

**Windows:**
```cmd
build.cmd
```

### Manual Build

1. **Build the WASM component:**
   ```bash
   dotnet publish -c Release
   ```

2. **Set up the demo app:**
   ```bash
   cd demo-app
   npm install
   ```

3. **Transpile the WASM to JavaScript:**
   ```bash
   npm run transpile
   ```

4. **Run the demo:**
   ```bash
   npm run dev
   ```

## WIT Interface

The WASM component exports a `solver` interface with the following functions:

```wit
interface solver {
    /// Solve part 1 of the puzzle with the given input
    solve-part1: func(input: string) -> s32;

    /// Solve part 2 of the puzzle with the given input
    solve-part2: func(input: string) -> s32;

    /// Solve both parts and return formatted results
    solve-all: func(input: string) -> string;
}
```

## Using the Plugin in JavaScript

After transpiling, you can use the plugin like this:

```javascript
import { solver } from './wasm/day1.js';

const input = `L68
L30
R48
...`;

const part1Result = solver.solvePart1(input);
const part2Result = solver.solvePart2(input);

console.log(`Part 1: ${part1Result}`);
console.log(`Part 2: ${part2Result}`);
```

## How It Works

1. **C# to WASM:** The .NET code is compiled to WebAssembly using the `BytecodeAlliance.Componentize.DotNet.Wasm.SDK` package, which implements the WASI Component Model.

2. **WIT Interface:** The `wit/world.wit` file defines the component's interface using WebAssembly Interface Types (WIT).

3. **JavaScript Bindings:** The `jco` tool transpiles the WASM component into JavaScript modules that can be imported in web applications.

4. **Browser Runtime:** The `@bytecodealliance/preview2-shim` package provides the WASI preview2 APIs needed to run the component in the browser.

## Troubleshooting

### Build Errors

If you encounter build errors, ensure you have:
- .NET 9.0 SDK installed: `dotnet --version`
- The correct runtime identifier: The project targets `wasi-wasm`

### Runtime Errors

If the WASM module fails to load:
1. Check the browser console for detailed errors
2. Ensure the WASM file was published: check `bin/Release/net9.0/wasi-wasm/publish/`
3. Verify the transpilation completed: check `demo-app/wasm/` for generated files

## License

MIT
