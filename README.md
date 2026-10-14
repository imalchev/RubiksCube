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

## CLI

`RubiksCube.Cli` is an interactive REPL for experimenting with a cube by hand: initialize it into
some state, print it out in full, and apply rotations one at a time to watch the state change.

```
dotnet run --project src/RubiksCube.Cli
```

Commands:

- `init` — wizard to initialize the cube as solved, scrambled with N random moves, or from a typed
  move sequence.
- `show` (alias `s`) — prints the cube's `CubeStateId` (so a state can be compared or looked up
  without eyeballing the matrix), then the cube itself as one big matrix: an opened-up box with all
  6 sides visible at once, laid out as the standard unfolded cross (White/Yellow above/below as
  up/down — `Y=2` is up, `Y=0` is down — with Green/Red/Blue/Orange side by side as
  left/front/right/back). Every tile shows its `(x,y,z)` coordinate, its current color, and the id
  of the physical piece it belongs to (e.g. `YGR` for the piece made of yellow/green/red tales) —
  the piece id is what lets you track one specific piece as it moves across rotations, since the
  coordinate alone only names the slot.
- `rotate` (alias `r`) `<move...>` — applies one or more moves, e.g. `rotate Red Blue' Green2`.
- `history` — lists the moves applied since the last `init`.
- `help`, `exit` / `quit`.

Moves are written as `<ColorName>[modifier]` (case-insensitive): a face's center color — `Red`,
`Orange`, `Yellow`, `White`, `Green`, or `Blue` — optionally followed by `'` for the inverse quarter
turn or `2` for a half turn. Color names are used instead of the usual Up/Down/Front/Back/Left/Right
letters because this project identifies faces by color, and a letter like "R" would otherwise be
ambiguous (Right vs. Red).

## Build & test

```
dotnet build RubiksCube.sln
dotnet test RubiksCube.sln
```
