using ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Semantics
{
	/// <summary>
	/// Emits an instruction for the renderer.
	/// </summary>
	/// <remarks>
	/// <para>This output node emits an instruction for the renderer.
	///   Consult the renderer's documentation to learn which rendering instructions are supported.
	///   Commonly supported instructions are:</para>
	/// <list type="table">
	/// <listheader>
	/// <term>Rendering Instruction</term>
	/// <description>Meaning</description>
	/// </listheader>
	/// <item>
	/// <term><c>linebreak</c></term>
	/// <description>Forces a hard line break.</description>
	/// </item>
	/// <item>
	/// <term><c>parbreak</c></term>
	/// <description>Forces a paragraph break.</description>
	/// </item>
	/// </list>
	/// </remarks>
	internal sealed class RenderingInstructionOutput : Output
	{
		public RenderingInstructionOutput(string? instruction)
		{
			ArgumentNullException.ThrowIfNull(instruction);

			Instruction = instruction;
		}

		public string Instruction { get; }

		public override IEnumerable<TextElement> Generate(IReadOnlyDictionary<string, string[]> arguments)
		{
			yield return new Documents.RenderingInstruction(Instruction);
		}
	}
}
