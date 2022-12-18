using System.Text.RegularExpressions;

namespace BandcampRenameV2.FileParser
{
    internal class FileNameParser : IFileParser
    {
        public string GetNewName(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var newfilename = filename;

            var match = Regex.Match(filename, " - \\d\\d ");
            if (match.Success)
            {
                var index = match.Index;
                newfilename = filename.Substring(index + 3);

                newfilename = newfilename.Trim(Path.GetInvalidFileNameChars());
            }

            return newfilename;
        }
    }
}
