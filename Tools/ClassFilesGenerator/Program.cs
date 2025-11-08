namespace ClassFilesGenerator;
class Program
{
    static void Main(string[] args)
    {
        // Validate arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run -- <source-directory> <destination-directory>");
            return;
        }

        var sourceDirectory = args[0];
        var destinationDirectory = args[1];

        if (!Directory.Exists(sourceDirectory))
        {
            Console.WriteLine($"Error: Source directory '{sourceDirectory}' does not exist.");
            return;
        }

        // Ensure destination directory exists
        Directory.CreateDirectory(destinationDirectory);

        // Filenames to look for (without extensions)
        var targetBaseNames = new List<string>
        {
            "DoubleClickBehaviour",
            "DoubleTapSelectOptions",
                
            "MyCustomComponent",
            "MyCustomNode",
            "CustomNodeOverview",
            "CustomNodeOverview.razor",
            "NodeOverview",
                
            "CustomGroup",
            "CustomGroupComponent",
            "CustomGroupOverview",
            "CustomGroupOverview.razor",
            "GroupOverview",
                
            "CustomPort",
            "CustomPortComponent",
            "CustomPositionPort",
            "CustomPortOverview",
            "CustomPortOverview.razor",
            "PortOverview",
                
            "OrthogonalLink",
            "CurvedLink",
            "LineLink",
            "ExtendedLink",
            "AnimatedLink",
            "AnimatedLinkComponent",
            "ExtendedLinkComponent",
            "CustomLinkOverview",
            "CustomLinkOverview.razor",
            
            "DoubleTapSelectBehaviour",
            "DoubleTapSelectOptions",
            "CustomBehaviours.razor"
        };

        // Collect matching files
        var foundFiles = Directory
            .EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories)
            .Where(file =>
            {
                var nameWithoutExt = Path.GetFileNameWithoutExtension(file).Trim();
                return targetBaseNames.Contains(nameWithoutExt, StringComparer.OrdinalIgnoreCase) && !file.EndsWith("txt");
            })
            .ToList();
            
        foundFiles.ForEach(x=> Console.WriteLine($"Found file:{x}"));

        if (foundFiles.Count == 0)
        {
            Console.WriteLine("No matching files found.");
            return;
        }

        // Copy each found file into destination as .txt
        var copiedCount = 0;
        foreach (var file in foundFiles)
        {
            try
            {
                var baseName = Path.GetFileNameWithoutExtension(file).Trim();
                var destFile = Path.Combine(destinationDirectory, baseName + ".txt");

                File.Copy(file, destFile, overwrite: true);
                copiedCount++;

                Console.WriteLine($"Copied: {file}  to  {destFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying '{file}': {ex.Message}");
            }
        }

        Console.WriteLine($"\n {copiedCount} files copied to '{destinationDirectory}'.");
    }
}