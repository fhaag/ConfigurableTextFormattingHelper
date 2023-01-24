using ConfigurableTextFormattingHelper.Rendering;

namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class Literal : TextElement
	{
		public Literal(string text)
		{
			ArgumentNullException.ThrowIfNull(text);

			Text = text;
		}

		public string Text { get; }

		internal override void Render(IRenderer renderer) => renderer.AppendLiteral(Text);

		public override TextElement CloneDeep() => new Literal(Text);
	}
}
