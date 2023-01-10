using System.Text;
using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper
{
	/// <summary>
	/// Transforms raw source code to a <see cref="Documents.Document">structured document</see>, based on a syntax definition.
	/// </summary>
	public sealed class Parser
	{
		internal Parser(ProcessingManager processingManager)
		{
			ArgumentNullException.ThrowIfNull(processingManager);

			this.processingManager = processingManager;
		}

		private readonly ProcessingManager processingManager;

		internal Documents.Span Parse(string str)
		{
			ArgumentNullException.ThrowIfNull(str);

			// TODO: get "global" root span def from elsewhere?
			Documents.Span currentSpan = new(new Syntax.SpanDef("", Array.Empty<string>(), Array.Empty<string>()));
			StringBuilder currentLiteral = new();

			var chIdx = 0;

			#region helper methods

			void AddCurrentCharVerbatim()
			{
				if (char.IsSurrogatePair(str, chIdx))
				{
					currentLiteral.Append(str, chIdx, 2);
					chIdx += 2;
				}
				else
				{
					currentLiteral.Append(str[chIdx]);
					chIdx++;
				}
			}

			void SaveVerbatimContent()
			{
				if (currentLiteral.Length > 0)
				{
					currentSpan.Elements.Add(new Documents.Literal(currentLiteral.ToString()));
					currentLiteral.Clear();
				}
			}

			#endregion

			while (chIdx < str.Length)
			{
				if (processingManager.Syntax.MatchEscape(str, chIdx) is { } escapeMatch)
				{
					chIdx += escapeMatch.Length;
					if (chIdx < str.Length)
					{
						AddCurrentCharVerbatim();
					}
					continue;
				}

				if (currentSpan.ElementDef?.FindEndInText(str, chIdx) is { } endMatch)
				{
					if (currentSpan.Parent == null)
					{
						// TODO: error
						throw new InvalidOperationException();
					}

					SaveVerbatimContent();

					chIdx += endMatch.Length;
					StoreArguments(endMatch, currentSpan.Arguments);
					// TODO: if current syntax environment was started by this span, call processingManager.LeaveSyntaxEnvironment()
					currentSpan = currentSpan.Parent;
					continue;
				}

				if (processingManager.Syntax.MatchElement(str, chIdx) is { } elementMatch)
				{
					SaveVerbatimContent();

					chIdx += elementMatch.Match.Length;

					Documents.ActiveTextElement docElement;
					switch (elementMatch.Element)
					{
						case Syntax.CommandDef cmdDef:
							docElement = new Documents.Command(cmdDef);
							currentSpan.Elements.Add(docElement);
							break;
						case Syntax.SpanDef spanDef:
							docElement = new Documents.Span(spanDef);
							currentSpan.Elements.Add(docElement);
							currentSpan = (Documents.Span)docElement;
							// TODO: update syntax environment?
							// but if each span creates a new syntax environment, the terminatedBy condition is never found
							// (syntax environments are only terminated by end patterns)
							break;
						default:
							throw new InvalidOperationException();
					}

					StoreArguments(elementMatch.Match, docElement.Arguments);
					continue;
				}

				AddCurrentCharVerbatim();
			}

			SaveVerbatimContent();

			return currentSpan;
		}

		private void StoreArguments(Match match, IDictionary<string, string[]> destArguments)
		{
			foreach (var group in match.Groups.OfType<Group>().Where(g => !string.IsNullOrWhiteSpace(g.Name)))
			{
				destArguments[group.Name] = group.Captures.Select(c => c.Value).ToArray();
			}
		}
	}
}