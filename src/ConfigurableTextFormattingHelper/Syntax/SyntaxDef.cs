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

	/// <summary>
	/// A strongly-typed syntax definition ready for use.
	/// </summary>
	internal sealed class SyntaxDef
	{
		public void AddEscape(string pattern)
		{
			escapePatterns.Add(pattern.ToPreciseLocationStartRegex());
		}

		private readonly List<Regex> escapePatterns = new();

		internal IReadOnlyList<Regex> EscapePatterns => escapePatterns;

		internal Match? MatchEscape(string text, int charIndex) => escapePatterns.FindMatch(text, charIndex);

		public void AddElement(ElementDef element)
		{
			var idx = string.IsNullOrWhiteSpace(element.RuleId) ? -1 : elements.FindIndex(e => e.RuleId == element.RuleId);
			if (idx >= 0)
			{
				elements[idx] = element;
			}
			else
			{
				elements.Add(element);
			}
		}

		private readonly List<ElementDef> elements = new();

		public IReadOnlyList<ElementDef> Elements => elements;

		public (ElementDef Element, Match Match)? MatchElement(string text, int charIndex)
		{
			foreach (var el in elements)
			{
				if (el.FindInText(text, charIndex) is Match elementMatch)
				{
					return (el, elementMatch);
				}
			}

			return null;
		}

		public void Append(SyntaxDef other)
		{
			ArgumentNullException.ThrowIfNull(other);

			escapePatterns.AddRange(other.EscapePatterns);

			for (var i = 0; i < other.Elements.Count; i++)
			{
				var otherEl = other.Elements[i];
				if (!string.IsNullOrWhiteSpace(otherEl.RuleId))
				{
					elements.RemoveAll(el => el.RuleId == otherEl.RuleId);
				}
				elements.Insert(i, otherEl);
			}
		}
	}
}
