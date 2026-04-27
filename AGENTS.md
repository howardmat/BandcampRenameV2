# Agent Guidelines (Must Follow)

- Keep this project C#/.NET; use .NET 10 and C# 14 unless explicitly told otherwise.
- Apply SOLID + DRY: keep responsibilities clear and avoid duplicated logic.
- Prefer dependency injection and interface-based boundaries at architecture seams.
- Keep code small and cohesive: one top-level type per file, named after the type.
- Keep `Program.cs` focused on orchestration only; move reusable/shared types out immediately.
- Use `async`/`await` correctly for I/O and propagate `CancellationToken` when applicable.
- Keep nullable reference types enabled and handle nullability explicitly.
- Never swallow exceptions; use structured logging with useful context, and add/update tests for behavior changes.
