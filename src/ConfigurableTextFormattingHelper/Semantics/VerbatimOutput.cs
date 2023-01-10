using ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Semantics
{
	/// <summary>
	/// Output verbatim text.
	/// </summary>
	internal sealed class VerbatimOutput : Output
	{
		public VerbatimOutput(string text)
		{
			ArgumentNullException.ThrowIfNull(text);

			Text = text;
		}

		public string Text { get; }

		public override IEnumerable<TextElement> Generate(IReadOnlyDictionary<string, string[]> arguments)
		{
			yield return new Documents.Literal(Text);
		}
	}
}
