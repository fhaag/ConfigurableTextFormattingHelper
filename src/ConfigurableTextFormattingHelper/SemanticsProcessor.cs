namespace ConfigurableTextFormattingHelper
{
	/// <summary>
	/// Transforms active elements in a <see cref="Documents.Document">structured document</see> into static, renderable content, based on a semantics definition.
	/// </summary>
	public sealed class SemanticsProcessor
	{
		internal SemanticsProcessor(ProcessingManager processingManager)
		{
			ArgumentNullException.ThrowIfNull(processingManager);

			this.processingManager = processingManager;
		}

		private readonly ProcessingManager processingManager;

		internal Documents.Span Process(Documents.Span data)
		{
			ArgumentNullException.ThrowIfNull(data);

			return ProcessSpan(data);
		}

		private Documents.Span ProcessSpan(Documents.Span span)
		{
			foreach (var child in span.Elements)
			{

			}
		}

		private IEnumerable<Documents.TextElement> ProcessElement(Documents.TextElement element)
		{
			if (element is Documents.ActiveTextElement activeElement)
			{
				
			}
			else
			{
				yield return element;
			}
		}
	}
}
