using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	internal sealed class SyntaxEnvironment
	{
		public SyntaxEnvironment(SyntaxDef syntax)
		{
			ArgumentNullException.ThrowIfNull(syntax);

			Syntax = syntax;
		}

		public SyntaxEnvironment(SyntaxEnvironment parent, SyntaxDef syntax, SpanDef span) : this(syntax)
		{
			ArgumentNullException.ThrowIfNull(parent);
			ArgumentNullException.ThrowIfNull(span);

			Parent = parent;
			DefiningSpan = span;
		}

		public SyntaxEnvironment? Parent { get; }

		public SyntaxDef Syntax { get; }

		public SpanDef? DefiningSpan { get; }
	}
}
