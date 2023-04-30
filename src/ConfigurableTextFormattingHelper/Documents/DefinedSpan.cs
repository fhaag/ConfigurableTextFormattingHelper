namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class DefinedSpan : Span, IDefinedTextElement
	{
		public DefinedSpan(Syntax.SpanDef elementDef, int level) : base()
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
			Level = level;

			SwitchToContent(elementDef.InitialContent);
		}

		public Syntax.SpanDef ElementDef { get; }

		public int Level { get; }

		Syntax.ElementDef IDefinedTextElement.ElementDef => ElementDef;

		private readonly Dictionary<string, string[]> arguments = new();

		public IDictionary<string, string[]> Arguments => arguments;

		IReadOnlyDictionary<string, string[]> IDefinedTextElement.Arguments => arguments;

		public override TextElement CloneDeep()
		{
			var result = new DefinedSpan(ElementDef, Level);

			CloneContent(result);

			return result;
		}

		public override DefinedSpan? FindEnclosingSpan(string spanId)
		{
			if (ElementDef.ElementId == spanId)
			{
				return this;
			}

			return base.FindEnclosingSpan(spanId);
		}

		public override void SwitchToContent(string contentId)
		{
			var contentDef = ElementDef.GetContentDefinition(contentId);
			SwitchToContent(contentId, contentDef.Mode);
		}

		protected override string InitialContentId => ElementDef.InitialContent;
	}
}
