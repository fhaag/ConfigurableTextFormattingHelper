using CommandLine;

namespace ConfigurableTextFormattingHelper.Rendering
{
	public class CliSettings
	{
		[Value(0, HelpText = "Specifies the path of the project to process.", MetaName = "project", Required = true)]
		public string? ProjectPath { get; set; }

		public string GetDefaultOutputName(string extension)
		{
			if (ProjectPath == null)
			{
				throw new InvalidOperationException("No input project path set.");
			}

			return Path.Combine(Path.GetDirectoryName(ProjectPath) ?? Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(ProjectPath) + extension);
		}
	}
}
