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

			return new(ElementId, Start, End ?? new(), Level)
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

			return new(ElementId, Match)
			{
				RuleId = RuleId
			};
		}
	}
}
