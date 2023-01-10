namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class RenderingInstruction : TextElement
	{
		public RenderingInstruction(string instruction)
		{
			ArgumentNullException.ThrowIfNull(instruction);

			Instruction = instruction;
		}

		public string Instruction { get; }
	}
}
