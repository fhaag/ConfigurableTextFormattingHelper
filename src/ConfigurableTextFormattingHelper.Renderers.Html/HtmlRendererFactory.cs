using System.Text;

namespace ConfigurableTextFormattingHelper.Renderers.Html
{
	using Rendering;

	public sealed class HtmlRendererFactory : IRendererFactoryWithCliSettings<HtmlCliSettings>
	{
		public string Identifier => "HTML";

		public string DisplayName => "HTML";

		public HtmlCliSettings? Settings { get; set; }

		public IRenderer CreateRenderer()
		{
			if (Settings == null)
			{
				throw new InvalidOperationException("The settings for the renderer factory have not been loaded.");
			}

			var destPath = Settings.EffectiveOutputPath;
			Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);

			var htmlDoc = new HtmlAgilityPack.HtmlDocument();
			var docRoot = htmlDoc.DocumentNode.AppendChild(htmlDoc.CreateElement("html"));
			docRoot.AppendChild(htmlDoc.CreateElement("head"));
			var body = docRoot.AppendChild(htmlDoc.CreateElement("body"));

			return new HtmlRenderer(htmlDoc, body, destPath);
		}
	}
}
