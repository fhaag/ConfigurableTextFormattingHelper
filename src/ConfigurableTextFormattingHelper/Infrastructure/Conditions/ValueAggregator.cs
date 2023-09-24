using ConfigurableTextFormattingHelper.Infrastructure.Expressions;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
{
	/// <summary>
	/// A helper class that retrieves values across levels of fallback.
	/// </summary>
	internal sealed class ValueAggregator : IValueProvider
	{
		public ValueAggregator()
		{
			values = new(this);
		}

		private readonly List<IReadOnlyDictionary<string, Value>> sources = new();

		public void AddFallbackLevel(IValueProvider source)
		{
			ArgumentNullException.ThrowIfNull(source);

			sources.Add(source.Values);
		}

		public void AddFallbackLevel(IReadOnlyDictionary<string, Value> source)
		{
			ArgumentNullException.ThrowIfNull(source);

			sources.Add(source);
		}

		private sealed class FallbackDictionary : IReadOnlyDictionary<string, Value>
		{
			public FallbackDictionary(ValueAggregator owner)
			{
				ArgumentNullException.ThrowIfNull(owner);

				this.owner = owner;
			}

			private readonly ValueAggregator owner;

			public Value this[string key]
			{
				get
				{
					if (TryGetValue(key, out var val))
					{
						return val;
					}

					throw new ArgumentException($"There is no value with ID {key}.");
				}
			}

			public IEnumerable<string> Keys => owner.sources.SelectMany(src => src.Keys).Distinct();

			public IEnumerable<Value> Values => Keys.Select(k => this[k]);

			public int Count => Keys.Count();

			public bool ContainsKey(string key) => owner.sources.Any(src => src.ContainsKey(key));

			public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
			{
				var returnedKeys = new HashSet<string>();
				foreach (var src in owner.sources)
				{
					foreach (var pair in src)
					{
						if (!returnedKeys.Contains(pair.Key))
						{
							yield return pair;
							returnedKeys.Add(pair.Key);
						}
					}
				}
			}

			public bool TryGetValue(string key, [MaybeNullWhen(false)] out Value value)
			{
				foreach (var src in owner.sources)
				{
					if (src.TryGetValue(key, out var val))
					{
						value = val;
						return true;
					}
				}

				value = null;
				return false;
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		private readonly FallbackDictionary values;

		public IReadOnlyDictionary<string, Value> Values => values;
	}
}
