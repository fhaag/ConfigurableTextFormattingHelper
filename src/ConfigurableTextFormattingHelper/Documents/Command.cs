namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class Command : TextElement, IDefinedTextElement
	{
		public Command(Syntax.CommandDef elementDef) : base()
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
		}

		public Syntax.CommandDef ElementDef { get; }

		Syntax.ElementDef IDefinedTextElement.ElementDef => ElementDef;

		private readonly Dictionary<string, string[]> arguments = new();

		public IDictionary<string, string[]> Arguments => arguments;

		IReadOnlyDictionary<string, string[]> IDefinedTextElement.Arguments => arguments;

		public override TextElement CloneDeep()
		{
			var result = new Command(ElementDef);
			foreach (var arg in arguments)
			{
				result.Arguments[arg.Key] = arg.Value;
			}
			return result;
		}
	}
}
