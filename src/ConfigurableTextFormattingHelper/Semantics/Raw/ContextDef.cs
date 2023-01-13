﻿namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	internal sealed class ContextDef
	{
		public string? Id { get; set; }

		public List<ElementRuleDef>? Elements { get; set; }

		public Semantics.ContextDef ToRuntimeContextDef(SemanticsProcessingManager processingManager)
		{
			var result = new Semantics.ContextDef(Id ?? "");

			if (Elements != null)
			{
				foreach (var rawEl in Elements)
				{
					var el = rawEl.ToRuntimeElementRuleDef(processingManager);
					result.Elements[el.Id] = el;
				}
			}

			return result;
		}
	}
}
