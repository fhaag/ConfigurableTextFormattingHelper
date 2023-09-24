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

namespace ConfigurableTextFormattingHelper.Documents
{
	using ExprValue = Infrastructure.Expressions.Value;

	internal sealed class DefinedSpan : Span, IDefinedTextElement
	{
		public DefinedSpan(Syntax.SpanDef elementDef, int level) : base()
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
			Level = level;

			SwitchToContent(elementDef.InitialContent);
		}

		public Syntax.SpanDef ElementDef { get; }

		public int Level { get; }

		Syntax.ElementDef IDefinedTextElement.ElementDef => ElementDef;

		private readonly Dictionary<string, ExprValue> arguments = new();

		public IDictionary<string, ExprValue> Arguments => arguments;

		IReadOnlyDictionary<string, ExprValue> IDefinedTextElement.Arguments => arguments;

		public override TextElement CloneDeep()
		{
			var result = new DefinedSpan(ElementDef, Level);

			CloneContent(result);

			return result;
		}

		public override DefinedSpan? FindEnclosingSpan(string spanId)
		{
			if (ElementDef.ElementId == spanId)
			{
				return this;
			}

			return base.FindEnclosingSpan(spanId);
		}

		public override void SwitchToContent(string contentId)
		{
			var contentDef = ElementDef.GetContentDefinition(contentId);
			SwitchToContent(contentId, contentDef.Mode);
		}

		protected override string InitialContentId => ElementDef.InitialContent;

		protected override string GetDebugInfo()
		{
			return ElementDef.ElementId + "[" + Level.ToString(InvariantCulture) + "]" + base.GetDebugInfo();
		}
	}
}
