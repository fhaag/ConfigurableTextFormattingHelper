using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	internal class ElementDef
	{
		public string? Id { get; set; }

		public List<string>? Start { get; set; }

		public List<string>? End { get; set; }

		public List<string>? Token { get; set; }

		public Syntax.ElementDef CreateElement()
		{
			return (Token, Start, End) switch
			{
				({ }, null, null) => CreateCommand(),
				(null, { }, { }) => CreateSpan(),
				_ => throw new NotSupportedException($"Failed to recognize syntax element type on element '{Id}'.")
			};
		}

		private SpanDef CreateSpan()
		{
			if (Id == null)
			{
				throw new InvalidOperationException("No id assigned.");
			}
			if (Start == null)
			{
				throw new InvalidOperationException($"No start patterns assigned on syntax element '{Id}'.");
			}
			if (End == null)
			{
				throw new InvalidOperationException($"No start patterns assigned on syntax element '{Id}'.");
			}

			return new(Id, Start, End);
		}

		private CommandDef CreateCommand()
		{
			if (Id == null)
			{
				throw new InvalidOperationException("No id assigned.");
			}
			if (Token == null)
			{
				throw new InvalidOperationException($"No patterns assigned on syntax element '{Id}'.");
			}

			return new(Id, Token);
		}
	}
}
