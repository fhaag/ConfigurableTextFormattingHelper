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

using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;
	using ExprValue = Infrastructure.Expressions.Value;

	/// <summary>
	/// A syntax element whose start and end can be recognized, and that encloses other document content.
	/// </summary>
	internal sealed class SpanDef : ElementDef
	{
		public const string DefaultContentId = "_default";

		public SpanDef(string elementId, IEnumerable<MatchSettings> startPatterns, IEnumerable<MatchSettings>? endPatterns, IEnumerable<Raw.ContentDef>? contentSettings = null, string? initialContent = null, IEnumerable<Raw.ContentSwitchDef>? contentSwitches = null, LevelPolicy? level = null) : base(elementId)
		{
			this.startPatterns.AddRange(startPatterns);
			if (endPatterns != null)
			{
				this.endPatterns.AddRange(endPatterns);
			}

			if (contentSwitches != null)
			{
				this.contentSwitches.AddRange(contentSwitches.Select(cs => cs.CreateContentSwitch()));
			}

			if (contentSettings != null)
			{
				foreach (var rawCs in contentSettings)
				{
					var cs = rawCs.CreateContentDef();
					this.contentSettings[cs.Id] = cs;
				}
			}

			this.level = level ?? new();
			InitialContent = initialContent ?? DefaultContentId;
		}

		private readonly List<MatchSettings> startPatterns = new();

		private readonly List<MatchSettings> endPatterns = new();

		private readonly LevelPolicy level;

		public override Match? FindInText(string text, int charIndex) => startPatterns.FindMatch(text, charIndex);

		public Match? FindEndInText(string text, int charIndex) => endPatterns.FindMatch(text, charIndex);

		public int DetermineLevel(int? enclosingLevel, IReadOnlyDictionary<string, ExprValue> arguments) => level.DetermineLevel(enclosingLevel, arguments);

		private readonly List<ContentSwitchDef> contentSwitches = new();

		private readonly Dictionary<string, ContentDef> contentSettings = new();

		public string InitialContent { get; }

		public (Match Match, string NewContentId)? FindContentSwitchInText(string text, int charIndex, string currentContentId)
		{
			var matchingResult = contentSwitches.Select(cs => new
			{
				SwitchDef = cs,
				Match = cs.FindInText(text, charIndex, currentContentId)
			}).FirstOrDefault(csInfo => csInfo.Match != null);

			if (matchingResult != null)
			{
				return (matchingResult.Match!, matchingResult.SwitchDef.To);
			}

			return null;
		}

		public ContentDef GetContentDefinition(string contentId)
		{
			contentSettings.TryGetValue(contentId, out var contentDef);
			return contentDef ?? new ContentDef
			{
				Id = contentId,
				Mode = ContentMode.Append
			};
		}
	}
}
