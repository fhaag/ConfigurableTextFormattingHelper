﻿/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Text;

namespace ConfigurableTextFormattingHelper.Renderers.Html
{
	using Rendering;

	public sealed class HtmlRendererFactory : IRendererFactoryWithCliSettings<HtmlCliSettings>
	{
		public string Identifier => "HTML";

		public string DisplayName => "HyperText Markup Language";

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
