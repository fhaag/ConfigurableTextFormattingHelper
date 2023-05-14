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

		public void AppendRenderingInstruction(string instruction, IReadOnlyDictionary<string, string[]> arguments)
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
