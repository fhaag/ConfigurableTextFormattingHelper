/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

namespace ConfigurableTextFormattingHelper.Semantics
{
	using Infrastructure.Conditions;

	partial class SemanticsProcessor
	{
		internal sealed class SubstitutionProcess : ISubstitutionProcess
		{
			public SubstitutionProcess(SemanticsDef semantics)
			{
				ArgumentNullException.ThrowIfNull(semantics);

				Semantics = semantics;

				foreach (var val in semantics.Values.Values)
				{
					Values[val.Id] = val.Clone();
				}
			}

			public SemanticsDef Semantics { get; }

			public IEnumerable<Documents.TextElement> Digest(IEnumerable<Documents.TextElement> textElements)
			{
				ArgumentNullException.ThrowIfNull(textElements);

				var result = new List<Documents.TextElement>();

				foreach (var te in textElements)
				{
					if (te is Documents.IDefinedTextElement defEl)
					{
						if (Semantics.Elements.TryGetValue(defEl.ElementDef.ElementId, out var elRules))
						{
							var matchingRule = elRules.FirstOrDefault(er => er.Condition?.Evaluate(this) ?? true);
							if (matchingRule != null)
							{
								CurrentElement = te;

								foreach (var output in matchingRule.Output)
								{
									foreach (var outputEl in output.Generate(this, defEl.Arguments))
									{
										result.Add(outputEl);
									}
								}
							}
						}
					}
					else if (te is Documents.Span spanEl)
					{
						var newSpan = new Documents.Span();
						foreach (var child in Digest(spanEl.GetDefaultContent()))
						{
							newSpan.AddElement(child);
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

			private readonly Dictionary<string, Value> values = new();

			public IDictionary<string, Value> Values => values;

			IReadOnlyDictionary<string, Value> IValueProvider.Values => values;
		}
	}
}
