namespace ConfigurableTextFormattingHelper.Documents
{
	/// <summary>
	/// The base class for all elements in a document.
	/// </summary>
	internal abstract class TextElement
	{
		public Span? Parent { get; internal set; }
	}
}
