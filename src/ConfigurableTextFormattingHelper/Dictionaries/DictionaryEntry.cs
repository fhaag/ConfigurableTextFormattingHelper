using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Dictionaries
{
	internal sealed class DictionaryEntry
	{
		public string? Text { get; set; }

		public Dictionary<string, DictionaryEntry> Variants { get; } = new();
	}
}
