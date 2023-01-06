namespace ConfigurableTextFormattingHelper.Defs
{
	internal sealed class SpanType
	{
		public ISet<string> StartTokens { get; } = new HashSet<string>();

		public ISet<string> EndTokens { get; } = new HashSet<string>();
	}
}
