/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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

		public string? Content { get; set; }

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
				ContentId = Content
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
