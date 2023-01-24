using ConfigurableTextFormattingHelper.Rendering;

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

		internal override void Render(IRenderer renderer) => renderer.AppendRenderingInstruction(Instruction);

		public override TextElement CloneDeep() => new RenderingInstruction(Instruction);
	}
}
