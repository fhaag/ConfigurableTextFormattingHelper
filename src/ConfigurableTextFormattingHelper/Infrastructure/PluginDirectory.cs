namespace ConfigurableTextFormattingHelper.Infrastructure
{
	/// <summary>
	/// Stores information about loaded plugins.
	/// </summary>
	public sealed class PluginDirectory
	{
		internal PluginDirectory(IEnumerable<Rendering.IRendererFactory> renderers)
		{
			ArgumentNullException.ThrowIfNull(renderers);

			Renderers = renderers.ToArray();
		}

		public IReadOnlyList<Rendering.IRendererFactory> Renderers { get; }

		public Rendering.IRendererFactory? FindRendererById(string id)
		{
			var normalizedId = id.ToLowerInvariant();
			return Renderers.FirstOrDefault(rf => rf.Identifier.ToLowerInvariant() == normalizedId);
		}
	}
}
