using BandcampRenameV2.FileParser;

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
}
