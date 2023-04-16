using ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
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
	/// <item>
	/// <term><c>headline</c></term>
	/// <description>Inserts a headline.
	///   Usually accompanied by a <c>level</c> argument that indicates the nesting level (starting at <c>1</c>), as well as a <c>title</c> argument that specifies the headline text.</description>
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

		public override IEnumerable<TextElement> Generate(ISubstitutionProcess process, IReadOnlyDictionary<string, string[]> arguments)
		{
			yield return new RenderingInstruction(Instruction, arguments);
		}

		private readonly Dictionary<string, string[]> arguments = new();

		public IDictionary<string, string[]> Arguments => arguments;
	}
}
