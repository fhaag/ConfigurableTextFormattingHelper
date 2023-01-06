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
			//escapePatterns.Add(new Regex(@"\G(?:" + pattern + ")"));
			escapePatterns.Add(pattern.ToPreciseLocationStartRegex());
		}

		private readonly List<Regex> escapePatterns = new();

		internal IReadOnlyList<Regex> EscapePatterns => escapePatterns;

		internal Match? MatchEscape(string text, int charIndex) => escapePatterns.FindMatch(text, charIndex);

		public void AddElement(ElementDef element)
		{
			var idx = elements.FindIndex(e => e.Id == element.Id);
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
	}
}
