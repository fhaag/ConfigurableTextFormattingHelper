namespace ConfigurableTextFormattingHelper
{
	using Infrastructure;
	using Rendering;

	/// <summary>
	/// Triggers the text processing.
	/// </summary>
	public sealed class Runner
	{
		public Runner()
		{
			plugins = PluginLoader.LoadPlugins();
		}

		public Runner(PluginDirectory plugins)
		{
			ArgumentNullException.ThrowIfNull(plugins);

			this.plugins = plugins;
		}

		private readonly PluginDirectory plugins;

		public void Execute(string rendererId, string[] args)
		{
			var rendererFactory = plugins.FindRendererById(rendererId);
			if (rendererFactory != null)
			{
				var cfgRendererFactory = PrepareRenderer(rendererFactory);
				cfgRendererFactory.LoadSettings(new[] { "" }.Concat(args).ToArray());
				var renderer = cfgRendererFactory.RendererFactory.CreateRenderer();
				// TODO: actually execute conversion
			}
			else
			{
				Console.WriteLine($"No renderer factory with ID {rendererId} found.");
			}
		}

		private ConfiguredRendererFactory PrepareRenderer(IRendererFactory renderer)
		{
			if (renderer is IRendererFactoryWithCliSettings rendererWithSettings)
			{
				var cfgRendererType = typeof(TypedConfiguredRendererFactory<>).MakeGenericType(rendererWithSettings.SettingsType);
				var result = (ConfiguredRendererFactory?)Activator.CreateInstance(cfgRendererType, renderer);

				if (result == null)
				{
					throw new InvalidOperationException($"Failed to initialize renderer factory of type {renderer.GetType()}.");
				}

				return result;
			}

			return new SimpleConfiguredRendererFactory(renderer);
		}
	}
}
