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


				currentContexts.Push(semantics.Contexts.First());
			}

			public SemanticsDef Semantics { get; }

			private readonly Stack<ContextDef> currentContexts = new();

			public void SwitchContext(string id)
			{
				ArgumentNullException.ThrowIfNull(id);

				id = id.ToLowerInvariant();
				var newCtxDef = Semantics.Contexts.FirstOrDefault(c => c.Id.ToLowerInvariant() == id);
				if (newCtxDef == null)
				{
					throw new InvalidOperationException($"Context {id} is not defined.");
				}

				currentContexts.Push(newCtxDef);
			}

			public void SwitchContextBack()
			{
				if (currentContexts.Count <= 1)
				{
					throw new InvalidOperationException();
				}

				currentContexts.Pop();
			}

			public IEnumerable<Documents.TextElement> Digest(IEnumerable<Documents.TextElement> textElements)
			{
				ArgumentNullException.ThrowIfNull(textElements);

				var result = new List<Documents.TextElement>();

				foreach (var te in textElements)
				{
					if (te is Documents.IDefinedTextElement defEl)
					{
						var currentContext = currentContexts.Peek();

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
