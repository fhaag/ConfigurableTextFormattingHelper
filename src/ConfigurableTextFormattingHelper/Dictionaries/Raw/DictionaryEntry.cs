﻿/*
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

namespace ConfigurableTextFormattingHelper.Dictionaries.Raw
{
	internal sealed class DictionaryEntry
	{
		public string? Text { get; set; }

		public Dictionary<string, DictionaryEntry> Variants { get; } = new();

		public Dictionaries.DictionaryEntry ToDictionaryEntry()
		{
			if (Variants.Count <= 0)
			{
				return new SimpleDictionaryEntry(Text ?? "");
			}

			var variants = new List<DictionaryEntryVariant>();

			void CollectVariants(DictionaryEntry root, string[] flags)
			{
				foreach (var pair in root.Variants)
				{
					CollectVariants(pair.Value, flags.Append(pair.Key).ToArray());
				}

				if (root.Text != null)
				{
					variants.Add(new(flags.ToHashSet(), root.Text));
				}
			}

			CollectVariants(this, Array.Empty<string>());

			return new ComplexDictionaryEntry(variants);
		}
	}
}