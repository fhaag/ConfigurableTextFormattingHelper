namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class DefinedSpan : Span, IDefinedTextElement
	{
		public DefinedSpan(Syntax.SpanDef elementDef) : base()
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
		}

		public Syntax.SpanDef ElementDef { get; }

		Syntax.ElementDef IDefinedTextElement.ElementDef => ElementDef;

		private readonly Dictionary<string, string[]> arguments = new();

		public IDictionary<string, string[]> Arguments => arguments;

		IReadOnlyDictionary<string, string[]> IDefinedTextElement.Arguments => arguments;

		public override TextElement CloneDeep()
		{
			var result = new DefinedSpan(ElementDef);
			foreach (var child in Elements)
			{
				result.Elements.Add(child.CloneDeep());
			}
			return result;
		}
	}
}
