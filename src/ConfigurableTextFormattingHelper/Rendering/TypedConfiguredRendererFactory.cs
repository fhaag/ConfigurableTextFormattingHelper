using static CommandLine.ParserResultExtensions;

namespace ConfigurableTextFormattingHelper.Rendering
{
	internal sealed class TypedConfiguredRendererFactory<TSettings> : ConfiguredRendererFactory
		where TSettings : CliSettings, new()
	{
		public TypedConfiguredRendererFactory(IRendererFactoryWithCliSettings<TSettings> rendererFactory) : base(rendererFactory)
		{
			RendererFactory = rendererFactory;
		}

		public new IRendererFactoryWithCliSettings<TSettings> RendererFactory { get; }

		public override CliSettings Settings => RendererFactory.Settings ?? throw new InvalidOperationException("Settings have not yet been loaded.");

		public override void LoadSettings(string[] args)
		{
			var parseResult = CommandLine.Parser.Default.ParseArguments<TSettings>(args);
			parseResult.WithParsed<TSettings>(settings =>
			{
				RendererFactory.Settings = settings;
			});
		}

		public override bool SettingsLoaded => RendererFactory.Settings != null;
	}
}
