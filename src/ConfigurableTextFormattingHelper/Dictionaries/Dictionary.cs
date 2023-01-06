using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Dictionaries
{
	internal sealed class Dictionary
	{
		private readonly Dictionary<string, DictionaryEntry> items = new();

		public DictionaryEntry? FindEntry(string key, params string[] variantFlags)
		{
			if (items.TryGetValue(key, out var entry))
			{

			}

			return null;
		}
	}
}
