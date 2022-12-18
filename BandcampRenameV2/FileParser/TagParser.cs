using File = TagLib.File;

namespace BandcampRenameV2.FileParser
{
    internal class TagParser : IFileParser
    {
        public string GetNewName(string filePath)
        {
            // Read data with TagLib Sharp
            // Open the file
            var file = File.Create(filePath);

            // Get the track number and format with leading 0 if necessary
            var trackStr = file.Tag.Track.ToString();
            if (file.Tag.Track == 0)
                trackStr = "01";
            else if (file.Tag.Track < 10)
                trackStr = "0" + trackStr;

            // Remove invalid characters for filename
            var titleForFilename = file.Tag.Title.Trim(Path.GetInvalidFileNameChars());

            // Set filename
            return $"{trackStr} {titleForFilename}.mp3";
        }
    }
}
