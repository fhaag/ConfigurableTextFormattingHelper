using System.Text;

namespace ConfigurableTextFormattingHelper.Renderers.Plain.PlainText
{
	public sealed class PlainTextRendererFactory : IRendererFactoryWithCliSettings<TextCliSettings>
	{
		public string Identifier => "TXT";

		public string DisplayName => "Plain Text";

		public TextCliSettings? Settings { get; set; }

		public IRenderer CreateRenderer()
		{
			if (Settings == null)
			{
				throw new InvalidOperationException("The settings for the renderer factory have not been loaded.");
			}

			var destPath = Settings.EffectiveOutputPath(".txt");
			return new PlainTextRenderer(new StreamWriter(File.Create(destPath), Encoding.UTF8));
		}
	}
}
