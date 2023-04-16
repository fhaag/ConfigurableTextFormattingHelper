namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class DefinedSpan : Span, IDefinedTextElement
	{
		public DefinedSpan(Syntax.SpanDef elementDef, int level) : base()
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
			Level = level;
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
			foreach (var child in Elements)
			{
				result.Elements.Add(child.CloneDeep());
			}
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
	}
}
