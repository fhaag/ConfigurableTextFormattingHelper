namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	internal class ElementRuleDef
	{
		public string? Id { get; set; }

		public List<OutputDef>? Output { get; set; }

		public Semantics.ElementRuleDef ToRuntimeElementRuleDef(SemanticsProcessingManager processingManager)
		{
			var result = new Semantics.ElementRuleDef(Id ?? "");

			if (Output != null)
			{
				foreach (var output in Output)
				{
					result.Output.Add(output.CreateOutput(processingManager));
				}
			}

			return result;
		}
	}
}
