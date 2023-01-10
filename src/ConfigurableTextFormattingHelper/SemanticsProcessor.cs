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
			throw new NotImplementedException();
		}
	}
}
