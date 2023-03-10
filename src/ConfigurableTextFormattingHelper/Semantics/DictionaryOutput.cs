using ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Semantics
{
	/// <summary>
	/// Looks up an entry from a dictionary and outputs the most suitable variant of that term.
	/// </summary>
	internal sealed class DictionaryOutput : Output
	{
		public string? Key { get; set; }

		public List<string> VariantFlags { get; } = new();

		public override IEnumerable<TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments)
		{
			throw new NotImplementedException();
		}
	}
}
