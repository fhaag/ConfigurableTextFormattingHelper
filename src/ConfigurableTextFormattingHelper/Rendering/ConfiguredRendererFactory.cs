namespace ConfigurableTextFormattingHelper.Rendering
{
	internal abstract class ConfiguredRendererFactory
	{
		protected ConfiguredRendererFactory(IRendererFactory rendererFactory)
		{
			ArgumentNullException.ThrowIfNull(rendererFactory);

			RendererFactory = rendererFactory;
		}

		public IRendererFactory RendererFactory { get; }

		public abstract CliSettings Settings { get; }

		public abstract void LoadSettings(string[] args);

		public abstract bool SettingsLoaded { get; }
	}
}
