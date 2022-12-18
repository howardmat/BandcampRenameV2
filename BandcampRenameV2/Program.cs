namespace BandcampRenameV2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get all files in all folders
            var parser = new FolderParser();
            var files = parser.Parse();

            // Display all files to the user
            PrintToConsole(files);

            // Prompt the user to indicate if they want to proceed with renaming
            Console.WriteLine("Do you want to rename the files? [y/n]");

            // Get a valid user response
            var userResponse = Console.ReadLine()?.ToLower();
            while (userResponse != "y" && userResponse != "n")
            {
                Console.WriteLine("Invalid response. Please enter 'y' for Yes or 'n' for No (without the quotes)");
                userResponse = Console.ReadLine();
            }

            // Check the user response and act accordingly
            if (userResponse == "y")
            {
                RenameCollection(files);
            }
        }

        static void PrintToConsole(Folder[] folders)
        {
            foreach (var folder in folders)
            {
                if (folder.Files.Count > 0)
                {
                    Console.WriteLine("");
                    Console.WriteLine("------------------------------------");
                    Console.WriteLine(folder.Name);
                    Console.WriteLine("------------------------------------");
                    foreach (var file in folder.Files)
                    {
                        Console.WriteLine($"From: {file.CurrentName}");
                        Console.WriteLine($"To: {file.NewName}");
                        Console.WriteLine("");
                    }
                }
            }
        }

        static void RenameCollection(Folder[] folders)
        {
            // Iterate folder collection
            foreach (var folder in folders)
            {
                // Iterate files in each folder
                foreach (var file in folder.Files)
                {
                    if (!string.IsNullOrEmpty(file.FullPath)
                        && !string.IsNullOrEmpty(file.NewPath))
                    {
                        // Rename to new name
                        File.Move(file.FullPath, file.NewPath);
                    }
                }
            }
        }
    }
}