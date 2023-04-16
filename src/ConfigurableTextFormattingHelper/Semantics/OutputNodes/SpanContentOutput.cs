namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
{
	internal sealed class SpanContentOutput : Output
	{
		public string? ContextId { get; init; }

		public bool HasContextSwitch => ContextId != null;

		public override IEnumerable<Documents.TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments)
		{
			if (process.CurrentElement is Documents.Span span)
			{
				if (HasContextSwitch)
				{
					process.SwitchContext(ContextId!);
				}

				var result = process.Digest(span.Elements);

				if (HasContextSwitch)
				{
					process.SwitchContextBack();
				}

				return result;
			}

			return Array.Empty<Documents.TextElement>();
		}
	}
}
