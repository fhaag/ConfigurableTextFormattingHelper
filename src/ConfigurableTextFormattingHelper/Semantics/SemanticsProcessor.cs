namespace ConfigurableTextFormattingHelper.Semantics
{
	/// <summary>
	/// Transforms active elements in a <see cref="Documents.Document">structured document</see> into static, renderable content, based on a semantics definition.
	/// </summary>
	public sealed partial class SemanticsProcessor
	{
		internal SemanticsProcessor(ProcessingManager processingManager)
		{
			ArgumentNullException.ThrowIfNull(processingManager);

			this.processingManager = processingManager;
		}

		private readonly ProcessingManager processingManager;

		internal Documents.Span Process(SemanticsDef semantics, Documents.Span data)
		{
			ArgumentNullException.ThrowIfNull(data);
			ArgumentNullException.ThrowIfNull(semantics);

			var process = new SubstitutionProcess(semantics);

			var processedElements = process.Digest(new[] { data });
			var result = new Documents.Span();
			foreach (var el in processedElements)
			{
				result.Elements.Add(el);
			}
			return result;
		}
	}
}
