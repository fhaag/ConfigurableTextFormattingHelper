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
	using ConfigurableTextFormattingHelper.Infrastructure.Expressions;

	internal class OutputDef
	{
		#region settings

		public string? Type { get; set; }

		public string? RenderingInstruction { get; set; }

		public string? DictKey { get; set; }

		public string? Verbatim { get; set; }

		public string? Content { get; set; }

		public string? Id { get; set; }

		public string? Value { get; set; }

		public string? Level { get; set; }

		public string? Title { get; set; }

		//public YamlNode? Arguments { get; set; }

		public Dictionary<string, string>? Arguments { get; set; }

		#endregion

		public Output CreateOutput(SemanticsProcessingManager processingManager)
		{
			return Type?.ToLowerInvariant() switch
			{
				"verbatim" => CreateVerbatim(processingManager),
				"renderinginstruction" => CreateRenderingInstruction(processingManager),
				"dictionary" => CreateDictionary(processingManager),
				"spancontent" => CreateSpanContent(processingManager),
				"setvalue" => CreateSetValueOutput(processingManager),
				"inc" => CreateIncDecValueOutput(processingManager, true),
				"dec" => CreateIncDecValueOutput(processingManager, false),
				{ } => throw new InvalidOperationException($"Unknown output type: {Type}"),
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

			var result = new RenderingInstructionOutput(RenderingInstruction);
			
			if (Arguments != null)
			{
				foreach (var pair in Arguments)
				{
					result.Arguments[pair.Key] = new[] { pair.Value };
				}
			}

			return result;
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

		private SetValueOutput CreateSetValueOutput(SemanticsProcessingManager processingManager)
		{
			if (Id == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(120), null));
				throw new InvalidOperationException("Error detected.");
			}

			return new(SetValueOutput.SetValueMode.Assign, Id, new EvaluatableExpression(Value));
		}

		private SetValueOutput CreateIncDecValueOutput(SemanticsProcessingManager processingManager, bool increase)
		{
			if (Id == null)
			{
				processingManager.AddMessage(new(ProcessingStage.Initialization, MessageSeverity.Error, processingManager.Messages.CreateMessage(120), null));
				throw new InvalidOperationException("Error detected.");
			}

			return new(increase
				? SetValueOutput.SetValueMode.Increase
				: SetValueOutput.SetValueMode.Decrease, Id, new EvaluatableExpression(Value ?? "1"));
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
