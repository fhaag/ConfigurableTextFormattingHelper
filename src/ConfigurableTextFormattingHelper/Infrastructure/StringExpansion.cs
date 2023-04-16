using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	/// <summary>
	/// This helper class contains methods for replacing placeholders in strings.
	/// </summary>
	internal static class StringExpansion
	{
		private static readonly Regex PlaceholderPattern = new(@"\[\[(?<token>[A-Za-z0-9-_]+)(?:\.(?<indexNumber>[-+]?[0-9]+))?(?<operation>#|\|)?\]\]");

		public static string Expand(this string str, IReadOnlyDictionary<string, string[]> arguments)
		{
			ArgumentNullException.ThrowIfNull(str);
			ArgumentNullException.ThrowIfNull(arguments);

			string ReplaceMatch(Match match)
			{
				var tokenGroup = match.Groups["token"];
				var indexNumberGroup = match.Groups["indexNumber"];
				var operationGroup = match.Groups["operation"];

				if (arguments.TryGetValue(tokenGroup.Value, out var values))
				{
					if (operationGroup.Value == "#")
					{
						return values.Length.ToString(InvariantCulture);
					}

					if (values.Length > 0)
					{
						var index = indexNumberGroup.Success ? int.Parse(indexNumberGroup.Value, InvariantCulture) : 0;
						if (index < 0)
						{
							index = values.Length + index;
						}
						index = Math.Max(Math.Min(index, values.Length - 1), 0);

						if (operationGroup.Success)
						{
							switch (operationGroup.Value)
							{
								case "|":
									return values[index].Length.ToString(InvariantCulture);
								default:
									throw new NotImplementedException("A compiler error should be returned here.");
							}
						}
						else
						{
							return values[index];
						}
					}
				}

				return "";
			}

			return PlaceholderPattern.Replace(str, ReplaceMatch);
		}
	}
}
