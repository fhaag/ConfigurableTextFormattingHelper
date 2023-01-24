namespace ConfigurableTextFormattingHelper.Rendering
{
	/// <summary>
	/// This component transfers the content of <see cref="Documents.Span">spans</see> to a renderer.
	/// </summary>
	internal sealed class RenderingController
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="renderer">The target renderer.</param>
		/// <exception cref="ArgumentNullException"><paramref name="renderer"/> is <see langword="null"/>.</exception>
		public RenderingController(IRenderer renderer)
		{
			ArgumentNullException.ThrowIfNull(renderer);

			this.renderer = renderer;
		}

		private readonly IRenderer renderer;

		public void Render(params Documents.Span[] spans)
		{
			ArgumentNullException.ThrowIfNull(spans);

			foreach (var span in spans)
			{
				span.Render(renderer);
			}
		}
	}
}
