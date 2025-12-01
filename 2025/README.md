# Advent of Code 2025

For 2025 the plan is to have a GitHub Page that can run the different samples in WebAssembly. The base site should allow hosting .WASM files that are executing the different things. 

## Add Days

```
dotnet new install BytecodeAlliance.Componentize.DotNet.Templates
dotnet new componentize.wasi.cli -o Day1
```