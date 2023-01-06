namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class Command : ActiveTextElement
	{
		public Command(Syntax.CommandDef elementDef) : base(elementDef)
		{
		}

		public new Syntax.CommandDef ElementDef => (Syntax.CommandDef)base.ElementDef;
	}
}
