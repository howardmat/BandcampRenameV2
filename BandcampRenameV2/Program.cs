// BandcampRenameV2 — console tool to preview and rename MP3 files from embedded metadata.
// Copyright (C) 2026  Mathew Howard
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using BandcampRenameV2;
using BandcampRenameV2.FileParser;

// Get all files in all folders and pull info from the MP3 tags 
var currentDirectory = Directory.GetCurrentDirectory();

var parser = new FolderParser(currentDirectory, new TagParser());
var files = parser.Parse();
var foldersWithFiles = files.Where(folder => folder.Files.Count > 0).ToArray();
var fileCount = foldersWithFiles.Sum(folder => folder.Files.Count);

PrintHeader(currentDirectory, foldersWithFiles.Length, fileCount);
PrintToConsole(foldersWithFiles);

// Prompt the user to indicate if they want to proceed with renaming
Console.WriteLine();
Console.Write("Proceed with rename? [y/n]: ");

// Get a valid user response
var userResponse = Console.ReadKey(intercept: true);
var response = char.ToLowerInvariant(userResponse.KeyChar);
while (response != 'y' && response != 'n')
{
    Console.WriteLine();
    Console.Write("Invalid input. Enter 'y' or 'n': ");
    userResponse = Console.ReadKey(intercept: true);
    response = char.ToLowerInvariant(userResponse.KeyChar);
}

Console.WriteLine(response);

// Check the user response and act accordingly
if (response == 'y')
{
    var renameSummary = RenameCollection(files);

    Console.WriteLine();
    PrintDivider();
    Console.WriteLine("Rename complete");
    PrintDivider();
    Console.WriteLine($"Folders renamed: {renameSummary.FoldersRenamed}");
    Console.WriteLine($"Files renamed:   {renameSummary.FilesRenamed}");
}
else
{
    Console.WriteLine();
    PrintDivider();
    Console.WriteLine("Rename canceled. No files or folders were changed.");
    PrintDivider();
}

static void PrintHeader(string directoryPath, int folderCount, int fileCount)
{
    Console.WriteLine("Bandcamp Rename Preview");
    PrintDivider();
    Console.WriteLine($"Root:    {directoryPath}");
    Console.WriteLine($"Folders: {folderCount}");
    Console.WriteLine($"Files:   {fileCount}");
    PrintDivider();
}

static void PrintToConsole(Folder[] folders)
{
    if (folders.Length == 0)
    {
        Console.WriteLine("No MP3 files found to preview.");
        return;
    }

    var folderIndex = 1;

    foreach (var folder in folders)
    {
        Console.WriteLine();
        Console.WriteLine($"[{folderIndex}] Folder");
        Console.WriteLine($"  From: {folder.Name}");
        Console.WriteLine($"  To:   {folder.NewName ?? folder.Name}");
        Console.WriteLine("  Files:");

        foreach (var file in folder.Files)
        {
            Console.WriteLine($"    - {file.CurrentName}");
            Console.WriteLine($"      -> {file.NewName}");
        }

        folderIndex++;
    }
}

static RenameSummary RenameCollection(Folder[] folders)
{
    var filesRenamed = 0;
    var foldersRenamed = 0;

    // Rename files first so all file paths are still valid.
    foreach (var folder in folders)
    {
        foreach (var file in folder.Files)
        {
            if (!string.IsNullOrEmpty(file.FullPath)
                && !string.IsNullOrEmpty(file.NewPath))
            {
                File.Move(file.FullPath, file.NewPath);
                filesRenamed++;
            }
        }
    }

    // Rename folders from deepest to shallowest to avoid path invalidation.
    foreach (var folder in folders.OrderByDescending(f => f.FullPath?.Length ?? 0))
    {
        if (!string.IsNullOrWhiteSpace(folder.FullPath)
            && !string.IsNullOrWhiteSpace(folder.NewPath)
            && !string.Equals(folder.FullPath, folder.NewPath, StringComparison.OrdinalIgnoreCase)
            && Directory.Exists(folder.FullPath)
            && !Directory.Exists(folder.NewPath))
        {
            Directory.Move(folder.FullPath, folder.NewPath);
            foldersRenamed++;
        }
    }

    return new RenameSummary(foldersRenamed, filesRenamed);
}

static void PrintDivider()
{
    Console.WriteLine(new string('-', 60));
}