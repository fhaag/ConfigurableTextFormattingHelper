using ConfigurableTextFormattingHelper.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Semantics
{
	/// <summary>
	/// Includes the contents of another file.
	/// </summary>
	internal sealed class IncludeOutput : Output
	{
		public override IEnumerable<TextElement> Generate(IReadOnlyDictionary<string, string[]> arguments)
		{
			throw new NotImplementedException();
		}
	}
}
