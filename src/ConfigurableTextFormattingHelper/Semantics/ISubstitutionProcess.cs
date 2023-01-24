namespace ConfigurableTextFormattingHelper.Semantics
{
	internal interface ISubstitutionProcess
	{
		Documents.TextElement CurrentElement { get; }

		IEnumerable<Documents.TextElement> Digest(IEnumerable<Documents.TextElement> elements);

		void SwitchContext(string id);

		void SwitchContextBack();
	}
}
