using ConfigurableTextFormattingHelper.Rendering;

namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class RenderingInstruction : TextElement
	{
		public RenderingInstruction(string instruction, IEnumerable<KeyValuePair<string, string[]>> arguments)
		{
			ArgumentNullException.ThrowIfNull(instruction);
			ArgumentNullException.ThrowIfNull(arguments);

			Instruction = instruction;
			Arguments = new Dictionary<string, string[]>(arguments);
		}

		public string Instruction { get; }

		public IReadOnlyDictionary<string, string[]> Arguments { get; }

		internal override void Render(IRenderer renderer) => renderer.AppendRenderingInstruction(Instruction, Arguments);

		public override TextElement CloneDeep() => new RenderingInstruction(Instruction, Arguments);
	}
}
