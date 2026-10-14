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

## Build & test

```
dotnet build RubiksCube.sln
dotnet test RubiksCube.sln
```
