using CommandLine;

namespace ConfigurableTextFormattingHelper.Renderers.Html
{
	using Rendering;

	public sealed class HtmlCliSettings : CliSettings
	{
		[Option('o', "out", HelpText = "The path to the output file.", Required = false)]
		public string? OutputPath { get; set; }

		internal string EffectiveOutputPath => OutputPath ?? GetDefaultOutputName(".html");
	}
}
