using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	/// <summary>
	/// An atomic syntax element.
	/// </summary>
	internal sealed class CommandDef : ElementDef
	{
		public CommandDef(string id, IEnumerable<string> patterns) : base(id)
		{
			this.patterns.AddRange(patterns.Select(CreateRegex));
		}

		private readonly List<Regex> patterns = new();

		public override Match? FindInText(string text, int charIndex) => patterns.FindMatch(text, charIndex);
	}
}
