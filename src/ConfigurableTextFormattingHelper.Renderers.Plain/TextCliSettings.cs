using CommandLine;

namespace ConfigurableTextFormattingHelper.Renderers.Plain
{
	public sealed class TextCliSettings : CliSettings
	{
		[Option('o', "out", HelpText = "The path to the output file.", Required = false)]
		public string? OutputPath { get; set; }

		internal string EffectiveOutputPath(string defaultExtension) => OutputPath ?? GetDefaultOutputName(defaultExtension);
	}
}
