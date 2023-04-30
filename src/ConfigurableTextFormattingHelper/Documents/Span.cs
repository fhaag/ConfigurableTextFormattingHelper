using ConfigurableTextFormattingHelper.Rendering;
using ConfigurableTextFormattingHelper.Syntax;
using System.ComponentModel;

namespace ConfigurableTextFormattingHelper.Documents
{
	internal class Span : TextElement
	{
		public IReadOnlyList<IReadOnlyList<TextElement>> GetContent(string contentId)
		{
			if (contentElements.TryGetValue(contentId, out var content))
			{
				return content;
			}

			return Array.Empty<IReadOnlyList<TextElement>>();
		}

		public IReadOnlyList<TextElement> GetDefaultContent() => GetContent(SpanDef.DefaultContentId).FirstOrDefault() ?? Array.Empty<TextElement>();

		private readonly Dictionary<string, List<List<TextElement>>> contentElements = new();

		private string? currentContentId;

		protected virtual string InitialContentId => SpanDef.DefaultContentId;

		public string CurrentContentId
		{
			get
			{
				if (currentContentId == null)
				{
					SwitchToContent(InitialContentId);
				}

				return currentContentId!;
			}
		}

		protected void SwitchToContent(string contentId, ContentMode mode)
		{
			if (!contentElements.TryGetValue(contentId, out var contentItems))
			{
				contentItems = new();
				contentElements[contentId] = contentItems;
			}

			switch (mode)
			{
				case ContentMode.Once:
					if (contentItems.Count > 0)
					{
						throw new InvalidOperationException($"Only one content with ID {contentId} is permitted.");
					}
					else
					{
						contentItems.Add(new());
					}
					break;
				case ContentMode.Append:
					if (contentItems.Count <= 0)
					{
						contentItems.Add(new());
					}
					break;
				case ContentMode.Multi:
					contentItems.Add(new());
					break;
				default:
					throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(ContentMode));
			}

			currentContentId = contentId;
		}

		public virtual void SwitchToContent(string contentId)
		{
			SwitchToContent(contentId, ContentMode.Append);
		}

		private List<TextElement> CurrentContent
		{
			get
			{
				return contentElements[CurrentContentId][^1];
			}
		}

		public void AddElement(TextElement element)
		{
			ArgumentNullException.ThrowIfNull(element);
			if (element.Parent != null)
			{
				throw new ArgumentException("The element is already attached to a document tree.");
			}

			CurrentContent.Add(element);
			element.Parent = this;
		}

		internal override void Render(IRenderer renderer)
		{
			// TODO: signal start and end of span?

			foreach (var element in GetDefaultContent())
			{
				element.Render(renderer);
			}
		}

		public override TextElement CloneDeep()
		{
			var result = new Span();

			CloneContent(result);

			return result;
		}

		protected void CloneContent(Span destination)
		{
			foreach (var content in contentElements)
			{
				var clonedContent = new List<List<TextElement>>();

				foreach (var item in content.Value)
				{
					clonedContent.Add(item.Select(te => te.CloneDeep()).ToList());
				}

				destination.contentElements[content.Key] = clonedContent;
			}
		}

		public virtual DefinedSpan? FindEnclosingSpan(string spanId) => Parent?.FindEnclosingSpan(spanId);
	}
}
