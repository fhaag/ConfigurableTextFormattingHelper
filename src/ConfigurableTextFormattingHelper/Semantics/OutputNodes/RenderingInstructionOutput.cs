/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

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
