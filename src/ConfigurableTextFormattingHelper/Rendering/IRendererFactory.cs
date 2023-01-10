namespace ConfigurableTextFormattingHelper.Rendering
{
	public interface IRendererFactory
	{
		string Identifier { get; }

		string DisplayName { get; }

		IRenderer CreateRenderer();
	}
}
