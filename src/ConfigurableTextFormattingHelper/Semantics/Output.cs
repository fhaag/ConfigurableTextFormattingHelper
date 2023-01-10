namespace ConfigurableTextFormattingHelper.Semantics
{
	internal abstract class Output
	{
		public abstract IEnumerable<Documents.TextElement> Generate(IReadOnlyDictionary<string, string[]> arguments);
	}
}
