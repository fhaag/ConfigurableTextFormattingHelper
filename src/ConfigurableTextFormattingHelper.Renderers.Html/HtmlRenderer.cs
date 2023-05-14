/*
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

using HtmlAgilityPack;

namespace ConfigurableTextFormattingHelper.Renderers.Html
{
	using Rendering;

	internal sealed class HtmlRenderer : IRenderer, IDisposable
	{
		internal HtmlRenderer(HtmlDocument destination, HtmlNode parentContainer, string destPath)
		{
			this.destination = destination;
			this.destPath = destPath;
			globalParentContainer = parentContainer;
			currentParent = parentContainer;
		}

		private readonly HtmlDocument destination;

		private readonly string destPath;

		private HtmlNode globalParentContainer;

		private HtmlNode currentParent;

		public void AppendLiteral(string literal)
		{
			currentParent.AppendChild(destination.CreateTextNode(literal));
		}

		public void AppendRenderingInstruction(string instruction, IReadOnlyDictionary<string, string[]> arguments)
		{
			switch (instruction)
			{
				case "linebreak":
					currentParent.AppendChild(destination.CreateElement("br"));
					break;
				case "parbreak":
					{
						if (currentParent != globalParentContainer)
						{
							currentParent = currentParent.ParentNode;
						}

						var newParent = destination.CreateElement("div");
						currentParent.AppendChild(newParent);
						currentParent = newParent;
					}
					break;
				default:
					throw new NotSupportedException($"Unsupported rendering instruction: {instruction}");
			}
		}

		public void Dispose()
		{
			destination.Save(destPath);
		}
	}
}
