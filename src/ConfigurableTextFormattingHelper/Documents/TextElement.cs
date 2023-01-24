namespace ConfigurableTextFormattingHelper.Documents
{
	/// <summary>
	/// The base class for all elements in a document.
	/// </summary>
	internal abstract class TextElement
	{
		public Span? Parent { get; internal set; }

		internal virtual void Render(Rendering.IRenderer renderer) => throw new NotSupportedException($"A text element of type {GetType().FullName} must be substituted before rendering.");

		public abstract TextElement CloneDeep();
	}
}
