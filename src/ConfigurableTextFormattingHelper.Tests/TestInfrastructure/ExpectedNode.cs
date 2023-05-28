using ConfigurableTextFormattingHelper.Documents;
using ConfigurableTextFormattingHelper.Infrastructure;

namespace ConfigurableTextFormattingHelper.Tests.TestInfrastructure
{
	internal sealed class ExpectedNode
	{
		public static ExpectedNode Literal(String text)
		{
			return new(typeof(Literal))
			{
				Text = text
			};
		}

		public static ExpectedNode Span(IEnumerable<ExpectedNode> defaultContent)
		{
			return new(typeof(Span))
			{
				DefaultContent = defaultContent.ToArray()
			};
		}

		public static ExpectedNode Span(ExpectedNode firstDefaultContentElement, params ExpectedNode[] moreDefaultContentElements)
		{
			return Span(CollectionHelper.ItemsToEnumerable(firstDefaultContentElement, moreDefaultContentElements));
		}

		public static ExpectedNode DefinedSpan(String spanId, Int32 level, IEnumerable<ExpectedNode> defaultContent)
		{
			return new(typeof(DefinedSpan))
			{
				ElementId = spanId,
				Level = level,
				DefaultContent = defaultContent.ToArray()
			};
		}

		public static ExpectedNode DefinedSpan(String spanId, Int32 level, ExpectedNode firstDefaultContentElement, params ExpectedNode[] moreDefaultContentElements)
		{
			return DefinedSpan(spanId, level, CollectionHelper.ItemsToEnumerable(firstDefaultContentElement, moreDefaultContentElements));
		}

		private ExpectedNode(Type nodeType)
		{
			NodeType = nodeType;
		}

		public Type NodeType { get; }

		public String? Text { get; private init; }

		public String? ElementId { get; private init; }

		public Int32? Level { get; private init; }

		public IReadOnlyList<ExpectedNode>? DefaultContent { get; private init; }
	}
}
