using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MonoMod;
using Quaver.Shared.Database.Maps;
using Quaver.API.Maps;
using Wobble.Screens;
using Quaver.Shared.Screens.Loading;
using Quaver.Shared.Database.Scores;
using System.Runtime.InteropServices;

namespace Wobble
{
    public class patch_WobbleGame
    {
        public extern void orig_Initialize();
        public virtual extern void orig_MoveNext();

        public void patch_Initialize()
        {
            orig_Initialize();

            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QuaverPlugins");

            if (!Directory.Exists(pluginDirectory))
                Directory.CreateDirectory(pluginDirectory);

            foreach (var dll in Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var raw = File.ReadAllBytes(dll);
                    var assembly = Assembly.Load(raw);

                    // Look for a type named "Plugin"
                    var pluginType = assembly.GetType("Plugin");

                    // If found, look for a public static method called Initialize
                    var initializeMethod = pluginType?.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static);

                    // Invoke if it exists
                    initializeMethod?.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load plugin {dll}: {ex}");
                }
            }
        }

    }
}
