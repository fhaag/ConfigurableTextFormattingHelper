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

		public void AppendRenderingInstruction(string instruction)
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
