namespace ConfigurableTextFormattingHelper.Semantics
{
	internal sealed class SpanContentOutput : Output
	{
		public override IEnumerable<Documents.TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments)
		{
			if (process.CurrentElement is Documents.Span span)
			{
				return process.Digest(span.Elements);
			}

			return Array.Empty<Documents.TextElement>();
		}
	}
}
