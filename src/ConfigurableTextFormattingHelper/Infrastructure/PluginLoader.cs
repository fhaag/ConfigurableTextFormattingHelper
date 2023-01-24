using System.Reflection;
using AssemblyLoadContext = System.Runtime.Loader.AssemblyLoadContext;

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

			//AssemblyLoadContext.Default.Resolving += ResolveAssembly;
			try
			{
				foreach (var dllPath in Directory.EnumerateFiles(localPath, "*.dll", new EnumerationOptions
				{
					IgnoreInaccessible = true,
					ReturnSpecialDirectories = false,
					RecurseSubdirectories = true
				}))
				{
					var dllDir = Path.GetDirectoryName(dllPath);

					var alc = new AssemblyLoadContext(dllPath, true);
					alc.Resolving += (ctx, asmName) =>
					{
						var fullPath = Path.Join(dllDir, asmName.Name + ".dll");
						if (File.Exists(fullPath))
						{
							return ctx.LoadFromAssemblyPath(fullPath);
						}

						return null;
					};
					try
					{
						Console.WriteLine($"Loading {dllPath} ...");
						var pluginAssembly = alc.LoadFromAssemblyPath(dllPath); //Assembly.LoadFile(dllPath);

						pluginDirs.Add(FindPlugins(pluginAssembly));
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
						alc.Unload();
					}
				}
			}
			finally
			{
				//AssemblyLoadContext.Default.Resolving -= ResolveAssembly;
			}

			return new(pluginDirs.SelectMany(pd => pd.Renderers));
		}

		private static Assembly? ResolveAssembly(AssemblyLoadContext alc, AssemblyName asmName)
		{
			return null;
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
