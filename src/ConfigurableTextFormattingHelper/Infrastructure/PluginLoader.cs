using System.Reflection;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	public static class PluginLoader
	{
		public static PluginDirectory LoadPlugins()
		{
			var localPath = Path.GetDirectoryName(typeof(PluginLoader).Assembly.Location);
			if (localPath == null)
			{
				throw new InvalidOperationException("Local path could not be determined.");
			}

			var pluginDirs = new List<PluginDirectory>();

			foreach (var dllPath in Directory.EnumerateFiles(localPath, "*.dll", new EnumerationOptions
			{
				IgnoreInaccessible = true,
				ReturnSpecialDirectories = false,
				RecurseSubdirectories = true
			}))
			{
				try
				{
					var pluginAssembly = Assembly.LoadFile(dllPath);

					pluginDirs.Add(FindPlugins(pluginAssembly));
				}
				catch (Exception ex)
				{

				}
			}

			return new(pluginDirs.SelectMany(pd => pd.Renderers));
		}

		private static PluginDirectory FindPlugins(Assembly pluginAssembly)
		{
			var renderers = new List<Rendering.IRendererFactory>();

			foreach (var type in pluginAssembly.ExportedTypes)
			{
				if (type is
					{
						IsClass: true,
						IsAbstract: false,
						IsGenericTypeDefinition: false,
						IsArray: false,
						ContainsGenericParameters: false
					})
				{
					if (type.IsAssignableTo(typeof(Rendering.IRendererFactory)))
					{
						var rendererFactory = (Rendering.IRendererFactory?)Activator.CreateInstance(type);
						if (rendererFactory != null)
						{
							renderers.Add(rendererFactory);
						}
					}
				}
			}

			return new(renderers);
		}
	}
}
