using System;
using System.IO;

public static class Plugin
{
    public static void Initialize()
    {
        string gameDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(gameDirectory, "Mod Running.txt");

        try
        {
            File.WriteAllText(filePath, "The mod is running successfully!");
            Console.WriteLine("Plugin initialized: 'Mod Running.txt' created.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create file: {ex}");
        }
    }
}
