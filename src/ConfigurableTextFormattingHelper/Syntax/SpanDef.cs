using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;
	using Infrastructure.Yaml;

	/// <summary>
	/// A syntax element whose start and end can be recognized, and that encloses other document content.
	/// </summary>
	internal sealed class SpanDef : ElementDef
	{
		public SpanDef(string elementId, IEnumerable<RawMatchSettings> startPatterns, IEnumerable<RawMatchSettings> endPatterns, Raw.LevelPolicy? level) : base(elementId)
		{
			this.startPatterns.AddRange(startPatterns.Select(ms => ms.CreateMatchSettings()));
			this.endPatterns.AddRange(endPatterns.Select(ms => ms.CreateMatchSettings()));
			this.level = level?.CreateLevelPolicy() ?? new();
		}

		private readonly List<MatchSettings> startPatterns = new();

		private readonly List<MatchSettings> endPatterns = new();

		private readonly LevelPolicy level;

		public override Match? FindInText(string text, int charIndex) => startPatterns.FindMatch(text, charIndex);

		public Match? FindEndInText(string text, int charIndex) => endPatterns.FindMatch(text, charIndex);

		public int DetermineLevel(int? enclosingLevel, IReadOnlyDictionary<string, string[]> arguments) => level.DetermineLevel(enclosingLevel, arguments);
	}
}
