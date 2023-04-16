using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal sealed class MatchSettings
	{
		public MatchSettings(Regex pattern)
		{
			ArgumentNullException.ThrowIfNull(pattern);

			this.pattern = pattern;
		}

		private readonly Regex pattern;

		// TODO: conditions

		public Match? Match(string str, int charIndex)
		{
			var match = pattern.Match(str, charIndex);
			if (!match.Success)
			{
				return null;
			}

			// TODO: check conditions

			return match;
		}
	}
}
