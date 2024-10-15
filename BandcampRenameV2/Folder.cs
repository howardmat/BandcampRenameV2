namespace BandcampRenameV2;

internal class Folder
{
    public string? FullPath { get; set; }
    public string? Name { get; set; }
    public ICollection<FolderFile> Files { get; set; } = [];
}
