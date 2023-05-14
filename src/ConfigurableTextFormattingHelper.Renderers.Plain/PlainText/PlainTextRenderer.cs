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
