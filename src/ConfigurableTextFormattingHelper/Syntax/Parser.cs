using ConfigurableTextFormattingHelper.Infrastructure;
using System.Text;
using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	/// <summary>
	/// Transforms raw source code to a <see cref="Documents.Document">structured document</see>, based on a syntax definition.
	/// </summary>
	public sealed partial class Parser
	{
		internal Parser(ProcessingManager processingManager)
		{
			ArgumentNullException.ThrowIfNull(processingManager);

			this.processingManager = processingManager;
		}

		private readonly ProcessingManager processingManager;

		internal Documents.Span Parse(SyntaxDef syntax, string str)
		{
			ArgumentNullException.ThrowIfNull(syntax);
			ArgumentNullException.ThrowIfNull(str);

			var process = new ParsingProcess(syntax);

			// TODO: get "global" root span def from elsewhere?
			Documents.Span currentSpan = new();

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
					currentSpan.AddElement(new Documents.Literal(currentLiteral.ToString()));
					currentLiteral.Clear();
				}
			}

			#endregion

			while (chIdx < str.Length)
			{
				if (process.Syntax.MatchEscape(str, chIdx) is { } escapeMatch)
				{
					chIdx += escapeMatch.Length;
					if (chIdx < str.Length)
					{
						AddCurrentCharVerbatim();
					}
					continue;
				}

				if (currentSpan is Documents.DefinedSpan currentDefinedSpan)
				{
					if (currentDefinedSpan.ElementDef?.FindContentSwitchInText(str, chIdx, currentSpan.CurrentContentId) is { } contentSwitchMatch)
					{
						currentSpan.SwitchToContent(contentSwitchMatch.NewContentId);

						chIdx += contentSwitchMatch.Match.Length;
						var args = ExtractArguments(contentSwitchMatch.Match);
						currentDefinedSpan.Arguments.MergeEntries(args);
						continue;
					}

					if (currentDefinedSpan.ElementDef?.FindEndInText(str, chIdx) is { } endMatch)
					{
						if (currentSpan.Parent == null)
						{
							// TODO: error
							throw new InvalidOperationException();
						}

						SaveVerbatimContent();

						chIdx += endMatch.Length;
						var args = ExtractArguments(endMatch);
						currentDefinedSpan.Arguments.MergeEntries(args);
						// TODO: if current syntax environment was started by this span, call processingManager.LeaveSyntaxEnvironment()
						currentSpan = currentSpan.Parent;
						continue;
					}
				}

				if (process.Syntax.MatchElement(str, chIdx) is { } elementMatch)
				{
					SaveVerbatimContent();

					chIdx += elementMatch.Match.Length;

					switch (elementMatch.Element)
					{
						case CommandDef cmdDef:
							{
								var args = ExtractArguments(elementMatch.Match);
								var docElement = new Documents.Command(cmdDef);
								docElement.Arguments.MergeEntries(args);
								currentSpan.AddElement(docElement);
							}
							break;
						case SpanDef spanDef:
							{
								var enclosingSpanOfSameType = currentSpan.FindEnclosingSpan(spanDef.ElementId);

								var args = ExtractArguments(elementMatch.Match);
								var newLevel = spanDef.DetermineLevel(enclosingSpanOfSameType?.Level, args);

								var docElement = new Documents.DefinedSpan(spanDef, newLevel);
								docElement.Arguments.MergeEntries(args);
								currentSpan.AddElement(docElement);
								
								currentSpan = docElement;
							}
							// TODO: update syntax environment?
							// but if each span creates a new syntax environment, the terminatedBy condition is never found
							// (syntax environments are only terminated by end patterns)
							break;
						default:
							throw new InvalidOperationException();
					}
					
					continue;
				}

				AddCurrentCharVerbatim();
			}

			SaveVerbatimContent();

			return currentSpan;
		}

		private IReadOnlyDictionary<string, string[]> ExtractArguments(Match match)
		{
			var result = new Dictionary<string, string[]>();

			foreach (var group in match.Groups.OfType<Group>().Where(g => !string.IsNullOrWhiteSpace(g.Name)))
			{
				result[group.Name] = group.Captures.Select(c => c.Value).ToArray();
			}

			return result;
		}
	}
}