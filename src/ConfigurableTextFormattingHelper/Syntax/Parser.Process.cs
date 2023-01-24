namespace ConfigurableTextFormattingHelper.Syntax
{
	partial class Parser
	{
		private sealed class ParsingProcess
		{
			public ParsingProcess(SyntaxDef syntax)
			{
				ArgumentNullException.ThrowIfNull(syntax);

				SyntaxEnvironment = new(syntax);
			}

			public SyntaxEnvironment SyntaxEnvironment { get; set; }

			public void LeaveSyntaxEnvironment()
			{
				if (SyntaxEnvironment.Parent == null)
				{
					throw new InvalidOperationException("The current syntax environment is the root environment.");
				}

				SyntaxEnvironment = SyntaxEnvironment.Parent;
			}

			public SyntaxDef Syntax => SyntaxEnvironment.Syntax;
		}
	}
}
