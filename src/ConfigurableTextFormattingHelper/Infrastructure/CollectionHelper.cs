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

namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal static class CollectionHelper
	{
		/// <summary>
		/// Adds entries from one dictionary to another dictionary, overriding existing entries if keys are the same.
		/// </summary>
		/// <typeparam name="K">The key type.</typeparam>
		/// <typeparam name="V">The value type.</typeparam>
		/// <param name="dest">The destination dictionary.</param>
		/// <param name="items">The dictionary whose items are to be added.</param>
		public static void MergeEntries<K, V>(this IDictionary<K, V> dest, IEnumerable<KeyValuePair<K, V>> items)
		{
			ArgumentNullException.ThrowIfNull(dest);
			ArgumentNullException.ThrowIfNull(items);

			foreach (var item in items)
			{
				dest[item.Key] = item.Value;
			}
		}

		internal static IEnumerable<T> ItemsToEnumerable<T>(T firstItem, params T[] moreItems)
		{
			ArgumentNullException.ThrowIfNull(firstItem);
			ArgumentNullException.ThrowIfNull(moreItems);

			return new[] { firstItem }.Concat(moreItems);
		}
	}
}
