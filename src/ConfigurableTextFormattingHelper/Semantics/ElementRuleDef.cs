namespace ConfigurableTextFormattingHelper.Semantics
{
	internal sealed class ElementRuleDef
	{
		public ElementRuleDef(string id)
		{
			ArgumentNullException.ThrowIfNull(id);

			Id = id;
		}

		public string Id { get; }

		public IList<Output> Output { get; } = new List<Output>();
	}
}
