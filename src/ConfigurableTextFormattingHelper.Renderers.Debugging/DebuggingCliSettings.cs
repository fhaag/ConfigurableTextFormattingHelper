using CommandLine;

namespace ConfigurableTextFormattingHelper.Renderers.Debugging
{
	using Rendering;

	public sealed class DebuggingCliSettings : CliSettings
	{
		[Option('o', "out", HelpText = "The path to the output file, if any. If left empty, output will be printed to the console.", Required = false)]
		public string? OutputPath { get; set; }

		[Option('c', "content", HelpText = "Include the text content of the input files.")]
		public bool IncludeContent { get; set; }
	}
}