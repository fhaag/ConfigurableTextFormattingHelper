namespace ConfigurableTextFormattingHelper.Semantics
{
	internal sealed class SemanticsDef
	{
		public IList<ContextDef> Contexts { get; } = new List<ContextDef>();

		public void Append(SemanticsDef other)
		{
			ArgumentNullException.ThrowIfNull(other);

			var nextInsertionIndex = 0;
			foreach (var ctx in other.Contexts)
			{
				var existingContext = Contexts.FirstOrDefault(c => c.Id == ctx.Id);
				if (existingContext != null)
				{
					existingContext.Append(ctx);
				}
				else
				{
					Contexts.Insert(nextInsertionIndex, ctx);
					nextInsertionIndex++;
				}
			}
		}
	}
}
