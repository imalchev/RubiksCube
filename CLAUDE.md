# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

A demo C# project modeling a Rubik's cube and its rotations (`net10.0`, nullable enabled). Four projects: `RubiksCube` (library), `RubiksCube.Test` (xUnit tests), `RubiksCube.Solver` (solving algorithms) and `RubiksCube.Solver.Test` (xUnit tests).

## Workflow

- Never run `git commit` unless the user's most recent prompt explicitly asks for a commit. Finishing a task, fixing a bug, or the user saying "looks good" is not consent to commit — wait for an explicit instruction in that turn.
- Before committing, run `dotnet build RubiksCube.sln` and confirm it succeeds with no errors. If any test project exists for the changed code, also run `dotnet test RubiksCube.sln` and confirm all tests pass. Do not commit if either fails — fix the issue first.

## Commands

- Build: `dotnet build RubiksCube.sln`
- Run all tests: `dotnet test RubiksCube.sln`
- Run a single test: `dotnet test --filter "FullyQualifiedName~RubiksCube.Test.CubeTests.RotateOnYellowWhiteOnFirstRowWithOneTurnover_Works"`
- Run one test class: `dotnet test --filter "FullyQualifiedName~CubeTests"`

## Architecture

The cube is a `CubePiece[3,3,3]` 3D array (`Cube._state`). Index `[1,1,1]` (the geometric center) is always `null`/unused — it has no visible tales, so helpers like `Extensions.AsEnumerable` and `EnumerateEmptySpaces` skip it explicitly.

Core model, bottom-up:
- **`Color`** (`Pieces.cs`) — `[Flags]` enum, one bit per cube color (White, Yellow, Green, Blue, Red, Orange).
- **`Orientation`** (`Pieces.cs`) — `[Flags]` enum of the three cube axes, each defined as the OR of its two opposing `Color`s: `RedOrange`, `YellowWhite`, `GreenBlue`. Orientation values double as rotation axis identifiers.
- **`Piece`** (`Pieces.cs`) — an immutable, orientation-agnostic physical piece carrying 1–3 colors (`Tale1/2/3`; `Tale3`/`Tale2` are `Color.None` when unused). Center pieces have 1 color, edge pieces 2, corner pieces 3. All 26 non-center pieces are predefined as static instances on the `Pieces` static class (e.g. `Pieces.YellowRedGreen`). Equality is structural (XOR-independent OR of colors), not identity.
- **`CubePiece`** (`CubePiece.cs`) — binds a `Piece` to its current placement by recording which `Orientation` (axis) each of the piece's tales currently faces (`Tale1Orientation` etc.). `GetColor(orientation)` looks up which color currently faces a given axis — this is how a piece's rotated state is read.
- **`CubeCoordinates`** (`CubeCoordinates.cs`) — a validated `(X,Y,Z)` value in `[0,2]^3`, implicitly convertible from `(int,int,int)` tuples. `HasOneOuterTale` / `HasTwoOuterTales` / `HasThreeOuterTales` classify a position as center/edge/corner slot by counting odd coordinates — used to validate that a `Piece` is being placed in a geometrically compatible slot.
- **`CubeBuilder`** (`CubeBuilder.cs`) — mutable builder over the `[3,3,3]` array. `CreateEmpty()` pre-seeds only the 6 center pieces (fixed axis-aligned, non-rotatable by convention: Yellow/White on Y axis, Red/Orange on Z axis, Green/Blue on X axis — note the axis-to-color mapping is fixed here). `CreateOrdered()` additionally places all edge/corner pieces to build a solved cube. `AddPiece` validates slot/piece compatibility and orientation consistency before writing. `Build()` requires `EmptyPieceSpaces == 0` (i.e. all 26 non-center slots filled) and returns an immutable `Cube`.
- **`Cube`** (`Cube.cs`) — immutable; `Rotate(RotationCommand)` returns a *new* `Cube` (never mutates). Rotation logic, per axis:
  1. Fix the rotating axis's coordinate (`Segment`: `First`=0 or `Third`=2 — middle slice rotation isn't modeled).
  2. For the other two axes, walk the 8 positions around that face (`GetRotationIndexes`, skipping the untouched face-center) and remap each to its destination index via `RotateIndexes`, which walks a 4-cycle for edge positions and a separate 4-cycle for corner positions, advancing by `Turnovers` (0–3, one 90° turn each).
  3. For each moved piece, `RotatePiece`/`RotateTale` recompute which axis each tale now faces: an odd number of turnovers swaps the piece's non-rotation-axis tale orientations between the *other two* axes; even turnovers leave orientations unchanged. The tale on the rotation axis itself never changes.
  - `GetSideColors` reads off a 3x3 face's colors by fixing one coordinate and reading `GetColor` for the corresponding axis `Orientation` across the other two.
- **`RotationCommand`** (`RotationCommand.cs`) — input to `Cube.Rotate`: `(Orientation axis, RotationSegment segment, int turnovers)`. `turnovers` is normalized mod 4 in the constructor.
- **`Extensions.cs`** — array-enumeration helpers over the `[3,3,3]` state, all center-slot-aware as noted above.

When modifying rotation logic, the axis-rotation math in `Cube.RotateIndexes`/`RotateTale` is the crux of correctness (see git history: "fix bug in rotation on axes Red-Orange" — these paths are easy to get subtly wrong per-axis). Favor adding/adjusting `CubeTests` cases (checking `GetSideColors` on all affected faces, not just the rotated one) over reasoning about the math in isolation.
