/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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
