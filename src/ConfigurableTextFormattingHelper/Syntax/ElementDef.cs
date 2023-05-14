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

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	/// <summary>
	/// The base class for all syntax elements that can occur in a document.
	/// </summary>
	internal abstract class ElementDef
	{
		protected ElementDef(string elementId)
		{
			ElementId = elementId;
		}

		/// <summary>
		/// The generated element ID.
		/// </summary>
		public string ElementId { get; }

		/// <summary>
		/// Gets or sets the ID of the syntax rule.
		/// </summary>
		public string? RuleId { get; init; }

		/// <summary>
		/// Finds a match for the pattern that indicates the element's start in a source text.
		/// </summary>
		/// <param name="text">The source text.</param>
		/// <param name="charIndex">The character index in <paramref name="text"/>.</param>
		/// <returns>A detected match, or <see langword="null"/> if no match was found.</returns>
		public abstract Match? FindInText(string text, int charIndex);
	}
}
