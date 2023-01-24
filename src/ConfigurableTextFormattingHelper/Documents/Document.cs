namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class Document
	{
		/// <summary>
		/// The root span of the document that contains the entire contents.
		/// </summary>
		// TODO: check initialization of dummy root span
		public Span Root { get; } = new();
	}
}
