namespace ConfigurableTextFormattingHelper.Renderers.Debugging
{
	using Rendering;

	internal sealed class DebugRenderer : IRenderer, IDisposable
	{
		internal DebugRenderer(TextWriter? destination, DebuggingCliSettings settings)
		{
			this.destination = destination;
			this.settings = settings;
		}

		private readonly TextWriter? destination;

		private readonly DebuggingCliSettings settings;

		#region colors

		private const ConsoleColor TextElementBoundaryColor = ConsoleColor.DarkGreen;

		private const ConsoleColor TextElementColor = ConsoleColor.Green;

		private const ConsoleColor LiteralColor = ConsoleColor.DarkGray;

		private const ConsoleColor TextElementParamColor = ConsoleColor.Yellow;

		#endregion

		private void Write(ConsoleColor cl, string text, bool appendNewLine = false)
		{
			if (destination != null)
			{
				destination.Write(text);
				if (appendNewLine)
				{
					destination.WriteLine();
				}
				return;
			}

			var origCl = Console.ForegroundColor;
			Console.ForegroundColor = cl;
			Console.Write(text);
			Console.ForegroundColor = origCl;
			if (appendNewLine)
			{
				Console.WriteLine();
			}
		}

		public void AppendLiteral(string literal)
		{
			Write(TextElementBoundaryColor, "[");
			Write(TextElementColor, "LITERAL");
			if (settings.IncludeContent)
			{
				Write(TextElementBoundaryColor, "|");
				Write(LiteralColor, literal);
			}
			Write(TextElementBoundaryColor, "]", true);
		}

		public void AppendRenderingInstruction(string instruction)
		{
			Write(TextElementBoundaryColor, "[");
			Write(TextElementColor, "RDINSTRUCTION");
			if (settings.IncludeContent)
			{
				Write(TextElementBoundaryColor, "|");
				Write(TextElementParamColor, instruction);
			}
			Write(TextElementBoundaryColor, "]", true);
		}

		public void Dispose()
		{
			destination?.Dispose();
		}
	}
}
