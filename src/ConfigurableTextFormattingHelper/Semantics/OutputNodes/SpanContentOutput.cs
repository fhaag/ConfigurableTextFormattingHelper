namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
{
	using Syntax;

	internal sealed class SpanContentOutput : Output
	{
		public string? ContextId { get; init; }

		public string? ContentId { get; init; }

		public bool HasContextSwitch => ContextId != null;

		public override IEnumerable<Documents.TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments)
		{
			if (process.CurrentElement is Documents.Span span)
			{
				if (HasContextSwitch)
				{
					process.SwitchContext(ContextId!);
				}

				var effectiveContentId = ContentId ?? SpanDef.DefaultContentId;
				var contentItems = span.GetContent(effectiveContentId);
				// TODO: retrieve other than first content?
				var result = process.Digest(contentItems.FirstOrDefault() ?? Array.Empty<Documents.TextElement>());

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
