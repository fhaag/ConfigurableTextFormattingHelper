using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	/// <summary>
	/// The base class for all syntax elements that can occur in a document.
	/// </summary>
	internal abstract class ElementDef
	{
		protected ElementDef(string id)
		{
			Id = id;
		}

		public string Id { get; }

		/// <summary>
		/// Finds a match for the pattern that indicates the element's start in a source text.
		/// </summary>
		/// <param name="text">The source text.</param>
		/// <param name="charIndex">The character index in <paramref name="text"/>.</param>
		/// <returns>A detected match, or <see langword="null"/> if no match was found.</returns>
		public abstract Match? FindInText(string text, int charIndex);

		protected static Regex CreateRegex(string pattern) => pattern.ToPreciseLocationStartRegex();
	}
}
