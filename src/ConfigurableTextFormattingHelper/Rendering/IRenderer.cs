namespace ConfigurableTextFormattingHelper.Rendering
{
	/// <summary>
	/// The common interface for objects that generate viewable output documents.
	/// </summary>
	public interface IRenderer
	{
		void AppendLiteral(string literal);

		void AppendRenderingInstruction(string instruction);
	}
}
