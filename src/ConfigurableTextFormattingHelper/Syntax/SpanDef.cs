using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	/// <summary>
	/// A syntax element whose start and end can be recognized, and that encloses other document content.
	/// </summary>
	internal sealed class SpanDef : ElementDef
	{
		public SpanDef(string id, IEnumerable<string> startPatterns, IEnumerable<string> endPatterns) : base(id)
		{
			this.startPatterns.AddRange(startPatterns.Select(CreateRegex));
			this.endPatterns.AddRange(endPatterns.Select(CreateRegex));
		}

		private readonly List<Regex> startPatterns = new();

		private readonly List<Regex> endPatterns = new();

		public override Match? FindInText(string text, int charIndex) => startPatterns.FindMatch(text, charIndex);

		public Match? FindEndInText(string text, int charIndex) => endPatterns.FindMatch(text, charIndex);
	}
}
