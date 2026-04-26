# BandcampRenameV2

Small **.NET 10** console tool that scans **MP3** files under the directory you run it from, shows a **preview** of proposed renames (files and folders), and only applies changes after you confirm.

Renaming uses embedded **ID3/metadata** via [TagLib#](https://github.com/mono/taglib-sharp) (`taglib-sharp-netstandard2.0`).

## Features

- **Preview first**: prints a structured summary of folder and file renames before anything is moved.
- **Interactive confirmation**: answer `y` / `n` at the prompt; invalid keys are rejected until you choose.
- **File names**: `NN <track title>.mp3` (two-digit track number from tags; invalid filename characters in the title are replaced with `-`).
- **Folder names** (when the folder contains at least one MP3): `<artist> - <year> - <album>`, with invalid path characters stripped from each segment. Artist falls back to first performer, then `Unknown Artist`; year and album have sensible fallbacks when missing.
- **Rename order**: files are renamed first, then folders **deepest to shallowest** so paths stay valid.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (or newer compatible SDK that can build `net10.0`).

## Build

From the repository root:

```bash
dotnet build BandcampRenameV2.sln
```

## Run

The tool uses **`Directory.GetCurrentDirectory()`** as the root: run it from the folder that contains (or directly contains subfolders with) the MP3s you want to process.

```bash
cd path\to\your\music\folder
dotnet run --project path\to\BandcampRenameV2\BandcampRenameV2\BandcampRenameV2.csproj
```

Or, if your shell is already in the project directory:

```bash
cd BandcampRenameV2
dotnet run
```

After the preview, you will see:

```text
Proceed with rename? [y/n]:
```

- **`y`**: perform renames and print counts of folders/files moved.
- **`n`**: exit without changing anything.

## Scope and limitations

- Only **`.mp3`** files are considered.
- The scanner includes the **current directory** and **immediate subdirectories** only (it does not walk deeper nested trees).
- Tags must be present and correct for the output names you want; garbage in tags means garbage in names.
- There is **no undo**; use version control or backups for anything you care about.

## Project layout

- `BandcampRenameV2/` — console application and parsing/rename logic.

## License

This program is free software: you can redistribute it and/or modify it under the terms of the [GNU General Public License](https://www.gnu.org/licenses/gpl-3.0.html) as published by the Free Software Foundation, **version 3** of the License, or (at your option) any later version.

The full license text is in the [`COPYING`](COPYING) file at the repository root (verbatim copy of the official GPL-3.0).
