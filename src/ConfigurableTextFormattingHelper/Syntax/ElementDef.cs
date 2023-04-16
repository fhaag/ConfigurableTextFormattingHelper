using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	/// <summary>
	/// The base class for all syntax elements that can occur in a document.
	/// </summary>
	internal abstract class ElementDef
	{
		protected ElementDef(string elementId)
		{
			ElementId = elementId;
		}

		/// <summary>
		/// The generated element ID.
		/// </summary>
		public string ElementId { get; }

		/// <summary>
		/// Gets or sets the ID of the syntax rule.
		/// </summary>
		public string? RuleId { get; init; }

		/// <summary>
		/// Finds a match for the pattern that indicates the element's start in a source text.
		/// </summary>
		/// <param name="text">The source text.</param>
		/// <param name="charIndex">The character index in <paramref name="text"/>.</param>
		/// <returns>A detected match, or <see langword="null"/> if no match was found.</returns>
		public abstract Match? FindInText(string text, int charIndex);
	}
}
