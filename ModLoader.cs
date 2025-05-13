using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MonoMod;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework
{
    public class patch_Game
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();
        public extern void orig_Run();
        public virtual extern void orig_MoveNext();

        public void patch_Run()
        {
            AllocConsole();
            var stdOut = Console.OpenStandardOutput();
            var writer = new StreamWriter(stdOut) { AutoFlush = true };
            Console.SetOut(writer);
            Console.SetError(writer);
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));

            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QuaverPlugins");

            if (!Directory.Exists(pluginDirectory))
                Directory.CreateDirectory(pluginDirectory);

            foreach (var dll in Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var loadContext = new PluginLoadContext(dll);
                    var assembly = loadContext.LoadFromAssemblyPath(dll);

                    var pluginType = assembly.GetType("Plugin");
                    var initializeMethod = pluginType?.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Static);
                    initializeMethod?.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load plugin {dll}: {ex}");
                }
            }


            orig_Run();

        }

    }
}
