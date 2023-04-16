using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal static class MatchSettingsHelper
	{
		public static Match? FindMatch(this IEnumerable<MatchSettings> items, string text, int charIndex)
		{
			ArgumentNullException.ThrowIfNull(items);

			foreach (var item in items)
			{
				var match = item.Match(text, charIndex);
				if (match != null)
				{
					return match;
				}
			}

			return null;
		}
	}
}
