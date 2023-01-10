using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Renderers.Plain.CommonMark
{
	internal sealed class CommonMarkRenderer : IRenderer
	{
		internal CommonMarkRenderer(TextWriter destination)
		{
			this.destination = destination;
		}

		private readonly TextWriter destination;

		// the reduced set of characters should be sufficient, while not cluttering the source too much with escape symbols
		private const string charsToEscape = @"[]()#*_\>-`"; // @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

		private static readonly Regex EscapePattern = new("[" + Regex.Escape(charsToEscape) + "]");

		public void AppendLiteral(string literal)
		{
			var escapedLiteral = EscapePattern.Replace(literal, @"\$0");
			destination.Write(escapedLiteral);
		}

		public void AppendRenderingInstruction(string instruction)
		{
			switch (instruction)
			{
				case "linebreak":
					destination.WriteLine("  ");
					break;
				case "parbreak":
					destination.WriteLine();
					destination.WriteLine();
					break;
				default:
					throw new NotSupportedException($"Unsupported rendering instruction: {instruction}");
			}
		}
	}
}
