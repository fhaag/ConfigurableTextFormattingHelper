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

namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	using Infrastructure.Yaml;

	internal class ElementDef
	{
		public string? RuleId { get; set; }

		private string FormattedId => RuleId ?? (ElementId != null ? $":{ElementId}:" : "<unnamed>");

		public string? ElementId { get; set; }

		public List<RawMatchSettings>? Start { get; set; }

		public List<RawMatchSettings>? End { get; set; }

		public List<RawMatchSettings>? Match { get; set; }

		public LevelPolicy? Level { get; set; }

		public string? InitialContent { get; set; }

		public List<ContentSwitchDef>? ContentSwitches { get; set; }

		public List<ContentDef>? ContentSettings { get; set; }

		public Syntax.ElementDef CreateElement()
		{
			if (ElementId == null)
			{
				throw new InvalidOperationException($"No element ID specified by rule '{FormattedId}'.");
			}

			return (Match, Start) switch
			{
				({ }, null) => CreateCommand(),
				(null, { }) => CreateSpan(),
				_ => throw new NotSupportedException($"Failed to recognize syntax element type on element '{FormattedId}'.")
			};
		}

		private SpanDef CreateSpan()
		{
			if (ElementId == null)
			{
				throw new InvalidOperationException("No element id assigned.");
			}
			if (Start == null)
			{
				throw new InvalidOperationException($"No start patterns assigned on syntax element '{FormattedId}'.");
			}

			return new(ElementId, Start.Select(m => m.CreateMatchSettings()), (End ?? new()).Select(m => m.CreateMatchSettings()), ContentSettings, InitialContent, ContentSwitches, Level?.CreateLevelPolicy())
			{
				RuleId = RuleId
			};
		}

		private CommandDef CreateCommand()
		{
			if (ElementId == null)
			{
				throw new InvalidOperationException("No element id assigned.");
			}
			if (Match == null)
			{
				throw new InvalidOperationException($"No patterns assigned on syntax element '{FormattedId}'.");
			}

			return new(ElementId, Match.Select(m => m.CreateMatchSettings()))
			{
				RuleId = RuleId
			};
		}
	}
}
