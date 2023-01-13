namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	internal sealed class SemanticsDef
	{
		public List<ContextDef>? Context { get; set; }

		public Semantics.SemanticsDef ToRuntimeSemanticsDef(SemanticsProcessingManager processingManager)
		{
			var result = new Semantics.SemanticsDef();

			if (Context != null)
			{
				foreach (var context in Context)
				{
					result.Contexts.Add(context.ToRuntimeContextDef(processingManager));
				}
			}

			return result;
		}
	}
}
