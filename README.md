# Rubiks cube

This is a demo project. Its goal is to handle rotations of the famous Rubiks cube in a c# program,
and ultimately to build the graph of all reachable cube states: each state is a vertex, and each
rotation (reversible by its inverse) is an edge connecting two states.

## Model

- `Color` / `Orientation` — the six cube colors and the three axes (each axis is the pair of
  opposing colors it connects).
- `Piece` — an immutable physical piece (center, edge, or corner) carrying 1-3 colors.
- `CubePiece` — a `Piece` placed in the cube, recording which axis each of its colors currently faces.
- `CubeCoordinates` — a validated `(x, y, z)` position in the `3x3x3` cube.
- `CubeBuilder` — builds a `Cube` piece by piece; `CubeBuilder.CreateOrdered()` builds a solved cube.
- `Cube` — immutable cube state. `Rotate(RotationCommand)` returns the resulting state without
  mutating the original.
- `CubeStateId` — a unique, deterministic identifier for a cube state, usable as a graph vertex key.

## Solver

`RubiksCube.Solver` is a separate project that searches for a sequence of `RotationCommand`s
turning a given scrambled `Cube` into a solved one (a state where `Cube.GetSideColors` returns a
single color per face). It exposes solving strategies behind a common interface so they can be
swapped and benchmarked against each other. Planned/implemented approaches:

1. **Bidirectional BFS** — breadth-first search simultaneously from the scrambled state and from
   the solved state, meeting in the middle. Builds on the same move/state-id primitives as
   `CubeGraphBuilder`. Optimal (shortest move count) but only practical for scrambles a handful of
   moves deep, since the state space grows exponentially with search depth.
2. **IDA\* (iterative-deepening A\*)** with an admissible heuristic (e.g. number of pieces not yet
   in their correct position/orientation, divided by the maximum number of pieces a single move can
   fix). Depth-first, so memory stays low; works at arbitrary scramble depth but slows down without
   a strong heuristic.
3. **Pattern-database heuristic** — precompute exact solve distances for a subset of pieces
   (e.g. corners only) and use them to strengthen IDA*'s heuristic, trading an upfront table-build
   cost for much faster solves.
4. **Two-phase (Kociemba-style) group reduction** — phase 1 searches into a restricted subgroup of
   moves that fixes edge/corner orientation, phase 2 solves within that subgroup with a smaller move
   set. Depth-independent and fast, but the most involved to implement correctly.
5. **Beam search / greedy best-first** — a fast, non-optimal fallback for deep scrambles, used as a
   baseline to compare the exact methods against.

The first two (bidirectional BFS and IDA*) are the initial implementations.

## Build & test

```
dotnet build RubiksCube.sln
dotnet test RubiksCube.sln
```
