namespace ConfigurableTextFormattingHelper.Renderers.Plain.PlainText
{
	internal sealed class PlainTextRenderer : IRenderer, IDisposable
	{
		internal PlainTextRenderer(TextWriter destination)
		{
			this.destination = destination;
		}

		private readonly TextWriter destination;

		public void AppendLiteral(string literal)
		{
			destination.Write(literal);
		}

		public void AppendRenderingInstruction(string instruction, IReadOnlyDictionary<string, string[]> arguments)
		{
			switch (instruction)
			{
				case "linebreak":
					destination.WriteLine();
					break;
				case "parbreak":
					destination.WriteLine();
					destination.WriteLine();
					break;
				default:
					throw new NotSupportedException($"Unsupported rendering instruction: {instruction}");
			}
		}

		public void Dispose()
		{
			destination.Dispose();
		}
	}
}
