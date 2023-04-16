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
			var idx = elements.FindIndex(e => e.ElementId == element.ElementId);
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
				elements.Insert(i, other.Elements[i]);
			}
		}
	}
}
