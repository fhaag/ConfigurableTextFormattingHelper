using System.Text;

namespace ConfigurableTextFormattingHelper.Renderers.Plain.CommonMark
{
	public sealed class CommonMarkRendererFactory : IRendererFactoryWithCliSettings<TextCliSettings>
	{
		public string Identifier => "MD";

		public string DisplayName => "CommonMark Markdown";

		public TextCliSettings? Settings { get; set; }

		public IRenderer CreateRenderer()
		{
			if (Settings == null)
			{
				throw new InvalidOperationException("The settings for the renderer factory have not been loaded.");
			}

			var destPath = Settings.EffectiveOutputPath(".md");
			Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
			return new CommonMarkRenderer(new StreamWriter(File.Create(destPath), Encoding.UTF8));
		}
	}
}
