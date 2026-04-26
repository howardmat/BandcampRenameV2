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

using BandcampRenameV2.FileParser;
using System.Linq;
using File = TagLib.File;

namespace BandcampRenameV2;

internal class FolderParser(string directoryPath, IFileParser parser)
{
    private readonly string _currentPath = directoryPath;
    private readonly IFileParser _fileParser = parser;

    public Folder[] Parse()
    {
        var folderCollection = new List<Folder>();

        // Including the current folder, get all folders within the current path
        var folders = new List<string> { _currentPath };
        folders.AddRange(GetAllFolders(_currentPath));

        // Get all of the files from each folder
        foreach (var folder in folders)
        {
            var files = GetFilesInFolder(folder);

            // Build the object to be returned
            var folderObj = new Folder
            {
                FullPath = folder,
                Name = Path.GetFileName(folder)
            };

            if (files.Length > 0)
            {
                var newFolderName = BuildNewFolderName(files[0]);
                folderObj.NewName = newFolderName;
                folderObj.NewPath = Path.Combine(Path.GetDirectoryName(folder) ?? _currentPath, newFolderName);
            }

            foreach (var file in files)
            {
                var currentname = Path.GetFileName(file);
                var newname = _fileParser.GetNewName(file);

                folderObj.Files.Add(new FolderFile
                {
                    FullPath = file,
                    CurrentName = currentname,
                    NewName = newname,
                    NewPath = Path.Combine(folder, newname)
                });
            }

            folderCollection.Add(folderObj);
        }

        return folderCollection.ToArray();
    }

    private static string[] GetAllFolders(string path)
    {
        var folders = new List<string>();

        // Add any other folders in this directory as well
        foreach (var dir in Directory.GetDirectories(path))
        {
            folders.Add(dir);
        }

        return folders.ToArray();
    }

    private static string[] GetFilesInFolder(string path)
    {
        var files = new List<string>();

        // Iterate over all files in directory
        foreach (var file in Directory.GetFiles(path))
        {
            // We only want .mp3 files
            if (Path.GetExtension(file) == ".mp3")
            {
                files.Add(file);
            }
        }

        return files.ToArray();
    }

    private static string BuildNewFolderName(string filePath)
    {
        using var file = File.Create(filePath);

        var albumArtist = file.Tag.FirstAlbumArtist ?? file.Tag.FirstPerformer ?? "Unknown Artist";
        var albumYear = file.Tag.Year > 0 ? file.Tag.Year.ToString() : "Unknown Year";
        var albumName = file.Tag.Album ?? "Unknown Album";

        return
            $"{RemoveInvalidPathChars(albumArtist)} - {RemoveInvalidPathChars(albumYear)} - {RemoveInvalidPathChars(albumName)}";
    }

    private static string RemoveInvalidPathChars(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Concat(value.Where(c => !invalidChars.Contains(c)));
    }
}
