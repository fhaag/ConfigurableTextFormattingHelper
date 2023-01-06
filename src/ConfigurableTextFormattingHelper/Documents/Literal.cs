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
	}
}
