namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal static class CollectionHelper
	{
		public static void MergeEntries<K, V>(this IDictionary<K, V> dest, IEnumerable<KeyValuePair<K, V>> items)
		{
			ArgumentNullException.ThrowIfNull(dest);
			ArgumentNullException.ThrowIfNull(items);

			foreach (var item in items)
			{
				dest[item.Key] = item.Value;
			}
		}
	}
}
