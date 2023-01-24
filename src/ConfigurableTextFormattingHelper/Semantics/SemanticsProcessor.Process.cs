namespace ConfigurableTextFormattingHelper.Semantics
{
	partial class SemanticsProcessor
	{
		internal sealed class SubstitutionProcess : ISubstitutionProcess
		{
			public SubstitutionProcess(SemanticsDef semantics)
			{
				ArgumentNullException.ThrowIfNull(semantics);

				Semantics = semantics;
				if (semantics.Contexts.Count() <= 0)
				{
					throw new InvalidOperationException("No semantics contexts defined.");
				}

				currentContext = semantics.Contexts.First();
			}

			public SemanticsDef Semantics { get; }

			private ContextDef currentContext;

			public IEnumerable<Documents.TextElement> Digest(IEnumerable<Documents.TextElement> textElements)
			{
				ArgumentNullException.ThrowIfNull(textElements);

				var result = new List<Documents.TextElement>();

				foreach (var te in textElements)
				{
					if (te is Documents.IDefinedTextElement defEl)
					{
						if (currentContext.Elements.TryGetValue(defEl.ElementDef.Id, out var elRule))
						{
							CurrentElement = te;

							foreach (var output in elRule.Output)
							{
								foreach (var outputEl in output.Generate(this, defEl.Arguments))
								{
									result.Add(outputEl);
								}
							}
						}
					}
					else if (te is Documents.Span spanEl)
					{
						var newSpan = new Documents.Span();
						foreach (var child in Digest(spanEl.Elements))
						{
							newSpan.Elements.Add(child);
						}
						result.Add(newSpan);
					}
					else
					{
						result.Add(te.CloneDeep());
					}
				}

				return result;
			}

			public Documents.TextElement? CurrentElement { get; private set; }
		}
	}
}
