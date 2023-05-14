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

namespace ConfigurableTextFormattingHelper.Rendering
{
	/// <summary>
	/// This component transfers the content of <see cref="Documents.Span">spans</see> to a renderer.
	/// </summary>
	internal sealed class RenderingController
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="renderer">The target renderer.</param>
		/// <exception cref="ArgumentNullException"><paramref name="renderer"/> is <see langword="null"/>.</exception>
		public RenderingController(IRenderer renderer)
		{
			ArgumentNullException.ThrowIfNull(renderer);

			this.renderer = renderer;
		}

		private readonly IRenderer renderer;

		public void Render(params Documents.Span[] spans)
		{
			ArgumentNullException.ThrowIfNull(spans);

			foreach (var span in spans)
			{
				span.Render(renderer);
			}
		}
	}
}
