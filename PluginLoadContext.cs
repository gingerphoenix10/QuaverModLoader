using System.Reflection;
using System.Runtime.Loader;

public class PluginLoadContext : AssemblyLoadContext
{
    private readonly string pluginPath;

    public PluginLoadContext(string pluginPath)
    {
        this.pluginPath = pluginPath;
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string dependencyPath = Path.Combine(Path.GetDirectoryName(pluginPath), assemblyName.Name + ".dll");

        if (File.Exists(dependencyPath))
        {
            return LoadFromAssemblyPath(dependencyPath);
        }

        return null; // fallback to default context
    }
}
