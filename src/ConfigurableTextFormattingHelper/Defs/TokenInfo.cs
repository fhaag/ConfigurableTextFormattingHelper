namespace ConfigurableTextFormattingHelper.Defs
{
	internal sealed class TokenInfo
	{
		public TokenInfo(int length, SpanType span) : this(length, TokenKind.StartSpan)
		{
			Span = span;
		}

		public TokenInfo(int length, TokenKind kind)
		{
			Length = length;
			Kind = kind;
		}

		public int Length { get; }

		public TokenKind Kind { get; }

		public SpanType? Span { get; }
	}
}
