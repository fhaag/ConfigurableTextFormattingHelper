namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
{
	internal abstract class Output
	{
		public abstract IEnumerable<Documents.TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments);
	}
}
