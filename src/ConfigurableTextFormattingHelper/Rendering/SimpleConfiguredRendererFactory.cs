using static CommandLine.ParserResultExtensions;

namespace ConfigurableTextFormattingHelper.Rendering
{
	internal sealed class SimpleConfiguredRendererFactory : ConfiguredRendererFactory
	{
		public SimpleConfiguredRendererFactory(IRendererFactory rendererFactory) : base(rendererFactory)
		{
		}

		private CliSettings? settings;

		public override CliSettings Settings => settings ?? throw new InvalidOperationException("Settings have not yet been loaded.");

		public override void LoadSettings(string[] args)
		{
			var parseResult = CommandLine.Parser.Default.ParseArguments<CliSettings>(args);
			parseResult.WithParsed<CliSettings>(newSettings =>
			{
				settings = newSettings;
			});
		}

		public override bool SettingsLoaded => settings != null;
	}
}
