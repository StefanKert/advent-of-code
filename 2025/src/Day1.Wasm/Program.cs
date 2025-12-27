// WASI Component entrypoint for Advent of Code 2025 Day 1
// This file registers the solver implementation with the WASI runtime

using Day1Wasm;

// Register the solver implementation for the exported interface
AdventExports.Advent.Day1.Solver.Instance = new Day1SolverImpl();
