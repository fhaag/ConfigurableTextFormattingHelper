namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	using Infrastructure;

	internal class OutputDef
	{
		public string? Type { get; set; }

		public string? RenderingInstruction { get; set; }

		public string? DictKey { get; set; }

		public string? Verbatim { get; set; }

		public string? Context { get; set; }

		public Semantics.Output CreateOutput(SemanticsProcessingManager processingManager)
		{
			return Type?.ToLowerInvariant() switch
			{
				"verbatim" => CreateVerbatim(processingManager),
				"renderingInstruction" => CreateRenderingInstruction(processingManager),
				_ => throw new NotImplementedException()
			};
		}

		private Semantics.VerbatimOutput CreateVerbatim(SemanticsProcessingManager processingManager)
		{
			if (Verbatim == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(100), null));
			}

			return new(Verbatim ?? "");
		}

		private Semantics.RenderingInstructionOutput CreateRenderingInstruction(SemanticsProcessingManager processingManager)
		{
			if (RenderingInstruction == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(110), null));
			}

			return new(RenderingInstruction);
		}
	}
}
