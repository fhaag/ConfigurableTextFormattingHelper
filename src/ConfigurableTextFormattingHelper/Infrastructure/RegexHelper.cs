using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal static class RegexHelper
	{
		internal static Regex ToPreciseLocationStartRegex(this string pattern) => new Regex(@"(?<=\G)" + pattern);

		public static Match? FindMatch(this IEnumerable<Regex> patterns, string text, int charIndex)
		{
			ArgumentNullException.ThrowIfNull(patterns);

			foreach (var pattern in patterns)
			{
				var match = pattern.Match(text, charIndex);
				if (match.Success)
				{
					return match;
				}
			}

			return null;
		}
	}
}
