namespace ConfigurableTextFormattingHelper.Semantics
{
	internal sealed class ContextDef
	{
		public ContextDef(string id)
		{
			ArgumentNullException.ThrowIfNull(id);

			Id = id;
		}

		public string Id { get; }

		public IDictionary<string, ElementRuleDef> Elements { get; } = new Dictionary<string, ElementRuleDef>();

		public void Append(ContextDef other)
		{
			ArgumentNullException.ThrowIfNull(other);

			foreach (var el in other.Elements.Values)
			{
				Elements[el.Id] = el;
			}
		}
	}
}
