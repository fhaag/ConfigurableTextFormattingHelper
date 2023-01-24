using System.Text;

namespace ConfigurableTextFormattingHelper.Renderers.Debugging
{
	using Rendering;

	public sealed class DebugRendererFactory : IRendererFactoryWithCliSettings<DebuggingCliSettings>
	{
		public string Identifier => "DEBUG";

		public string DisplayName => "Structural Debugging Output";

		public DebuggingCliSettings? Settings { get; set; }

		public IRenderer CreateRenderer()
		{
			if (Settings == null)
			{
				throw new InvalidOperationException("The settings for the renderer factory have not been loaded.");
			}

			TextWriter? destination = null;

			if (Settings.OutputPath != null)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(Settings.OutputPath)!);
				destination = new StreamWriter(File.Create(Settings.OutputPath), Encoding.UTF8);
			}
			
			return new DebugRenderer(destination, Settings);
		}
	}
}
