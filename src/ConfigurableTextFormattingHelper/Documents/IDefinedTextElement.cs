namespace ConfigurableTextFormattingHelper.Documents
{
	internal interface IDefinedTextElement
	{
		Syntax.ElementDef ElementDef { get; }

		IReadOnlyDictionary<string, string[]> Arguments { get; }
	}
}
