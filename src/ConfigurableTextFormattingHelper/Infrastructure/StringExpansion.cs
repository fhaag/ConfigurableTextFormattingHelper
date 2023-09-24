/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	using ExprValue = Expressions.Value;

	/// <summary>
	/// This helper class contains methods for replacing placeholders in strings.
	/// </summary>
	internal static class StringExpansion
	{
		private static readonly Regex PlaceholderPattern = new(@"\[\[(?<token>[A-Za-z0-9-_]+)(?:\.(?<indexNumber>[-+]?[0-9]+))?(?<operation>#|\|)?\]\]");

		public static string Expand(this string str, IReadOnlyDictionary<string, ExprValue> arguments)
		{
			ArgumentNullException.ThrowIfNull(str);
			ArgumentNullException.ThrowIfNull(arguments);

			string ReplaceMatch(Match match)
			{
				var tokenGroup = match.Groups["token"];
				var indexNumberGroup = match.Groups["indexNumber"];
				var operationGroup = match.Groups["operation"];

				if (arguments.TryGetValue(tokenGroup.Value, out var val))
				{
					return (operationGroup.Success, operationGroup.Value) switch
					{
						(true, "|") => val.AsStringValue.Value.Length.ToString(InvariantCulture),
						_ => val.AsStringValue.Value
					};
				}

				// TODO: consider other operations?
				/*
				if (arguments.TryGetValue(tokenGroup.Value, out var val))
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
									return val.Length.ToString(InvariantCulture);
								default:
									throw new NotImplementedException("A compiler error should be returned here.");
							}
						}
						else
						{
							return values[index];
						}
					}
				}*/

				return "";
			}

			return PlaceholderPattern.Replace(str, ReplaceMatch);
		}
	}
}
