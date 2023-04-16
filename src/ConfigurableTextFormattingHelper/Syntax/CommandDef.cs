using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;
	using Infrastructure.Yaml;

	/// <summary>
	/// An atomic syntax element.
	/// </summary>
	internal sealed class CommandDef : ElementDef
	{
		public CommandDef(string elementId, IEnumerable<RawMatchSettings> patterns) : base(elementId)
		{
			this.match.AddRange(patterns.Select(ms => ms.CreateMatchSettings()));
		}

		private readonly List<MatchSettings> match = new();

		public override Match? FindInText(string text, int charIndex) => match.FindMatch(text, charIndex);
	}
}
