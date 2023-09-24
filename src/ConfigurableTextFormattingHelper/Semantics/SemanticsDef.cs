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

namespace ConfigurableTextFormattingHelper.Semantics
{
	using Infrastructure.Expressions;

	internal sealed class SemanticsDef
	{
		public IDictionary<string, Value> Values { get; } = new Dictionary<string, Value>();

		public IDictionary<string, IReadOnlyList<ElementRuleDef>> Elements { get; } = new Dictionary<string, IReadOnlyList<ElementRuleDef>>();

		public void Append(SemanticsDef other)
		{
			ArgumentNullException.ThrowIfNull(other);

			foreach (var val in other.Values.Values)
			{
				Values[val.Id] = val;
			}

			var newElements = Elements.ToDictionary(p => p.Key, p => p.Value.ToList());
			foreach (var pair in other.Elements)
			{
				if (!newElements.TryGetValue(pair.Key, out var elRules))
				{
					elRules = new();
					newElements[pair.Key] = elRules;
				}

				for (var i = 0; i < pair.Value.Count; i++)
				{
					elRules.Insert(0, pair.Value[i]);
					i++;
				}
			}

			foreach (var pair in newElements)
			{
				Elements[pair.Key] = pair.Value.ToArray();
			}
		}
	}
}
