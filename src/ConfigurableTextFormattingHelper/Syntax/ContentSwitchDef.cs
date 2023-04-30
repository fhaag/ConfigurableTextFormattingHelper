using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure.Yaml;
	using Infrastructure;

	internal sealed class ContentSwitchDef
	{
		public ContentSwitchDef(IEnumerable<RawMatchSettings> match, string toContent, IEnumerable<string>? fromContent = null)
		{
			ArgumentNullException.ThrowIfNull(match);
			ArgumentNullException.ThrowIfNull(toContent);

			this.match.AddRange(match.Select(m => m.CreateMatchSettings()));
			To = toContent;

			if (fromContent != null)
			{
				from.UnionWith(fromContent);
			}
		}

		private readonly HashSet<string> from = new();

		public string To { get; }

		private readonly List<MatchSettings> match = new();

		public Match? FindInText(string text, int charIndex, string currentContentId)
		{
			if (from.Count <= 0 || from.Contains(currentContentId))
			{
				return match.FindMatch(text, charIndex);
			}

			return null;
		}
	}
}
