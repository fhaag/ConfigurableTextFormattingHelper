using YamlDotNet.RepresentationModel;

namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	using OutputNodes;
	using Infrastructure;

	internal class OutputDef
	{
		#region settings

		public string? Type { get; set; }

		public string? RenderingInstruction { get; set; }

		public string? DictKey { get; set; }

		public string? Verbatim { get; set; }

		public string? Context { get; set; }

		public YamlNode? Arguments { get; set; }

		#endregion

		public Output CreateOutput(SemanticsProcessingManager processingManager)
		{
			return Type?.ToLowerInvariant() switch
			{
				"verbatim" => CreateVerbatim(processingManager),
				"renderinginstruction" => CreateRenderingInstruction(processingManager),
				"dictionary" => CreateDictionary(processingManager),
				"spancontent" => CreateSpanContent(processingManager),
				_ => GuessOutput(processingManager)
			};
		}

		private VerbatimOutput CreateVerbatim(SemanticsProcessingManager processingManager)
		{
			if (Verbatim == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(100), null));
			}

			return new(Verbatim ?? "");
		}

		private RenderingInstructionOutput CreateRenderingInstruction(SemanticsProcessingManager processingManager)
		{
			if (RenderingInstruction == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(110), null));
			}

			return new(RenderingInstruction);
		}

		private DictionaryOutput CreateDictionary(SemanticsProcessingManager processingManager)
		{
			// TODO: complete
			return new();
		}

		private SpanContentOutput CreateSpanContent(SemanticsProcessingManager processingManager)
		{
			return new()
			{
				ContextId = Context
			};
		}

		private Output GuessOutput(SemanticsProcessingManager processingManager)
		{
			if (!string.IsNullOrEmpty(RenderingInstruction))
			{
				return CreateRenderingInstruction(processingManager);
			}

			if (!string.IsNullOrEmpty(Verbatim))
			{
				return CreateVerbatim(processingManager);
			}

			if (!string.IsNullOrEmpty(DictKey))
			{
				return CreateDictionary(processingManager);
			}

			throw new InvalidOperationException("Cannot determine output type.");
		}
	}
}
