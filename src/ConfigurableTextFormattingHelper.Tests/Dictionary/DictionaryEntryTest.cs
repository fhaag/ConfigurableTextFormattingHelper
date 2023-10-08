using ConfigurableTextFormattingHelper.Dictionaries;
using System.Runtime.CompilerServices;

namespace ConfigurableTextFormattingHelper.Tests.Dictionary
{
	public class DictionaryEntryTest
	{
		private static object?[] TupleToObjects(ITuple src)
		{
			var result = new object?[src.Length];
			for (var i = 0; i < src.Length; i++)
			{
				result[i] = src[i];
			}
			return result;
		}

		public static IEnumerable<object?[]> GetSimpleEntryData()
		{
			IEnumerable<(FlagMatchingStrategy Strategy, IEnumerable<string> Flags, string? Expected)> GetItems()
			{
				yield return (FlagMatchingStrategy.AllFlags, Array.Empty<string>(), "aBC");
				yield return (FlagMatchingStrategy.ExactFlags, Array.Empty<string>(), "aBC");
				yield return (FlagMatchingStrategy.MaximumFlags, Array.Empty<string>(), "aBC");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "xx" }, null);
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "xx" }, null);
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "xx" }, "aBC");
			}

			return GetItems().Select(t => TupleToObjects(t));
		}

		[Theory]
		[MemberData(nameof(GetSimpleEntryData))]
		internal void TestSimpleEntry(FlagMatchingStrategy strategy, IEnumerable<string> flags, string? expected)
		{
			var resolveResult = new SimpleDictionaryEntry("aBC").Resolve(strategy, flags.ToHashSet());
			if (expected == null)
			{
				resolveResult.Should().BeNull();
			}
			else
			{
				resolveResult.Should().Be(expected);
			}
		}

		public static IEnumerable<object?[]> GetComplexEntryData()
		{
			IEnumerable<(FlagMatchingStrategy Strategy, IEnumerable<string> Flags, string? Expected)> GetItems()
			{
				yield return (FlagMatchingStrategy.AllFlags, Array.Empty<string>(), "D");
				yield return (FlagMatchingStrategy.ExactFlags, Array.Empty<string>(), "D");
				yield return (FlagMatchingStrategy.MaximumFlags, Array.Empty<string>(), "D");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "xx" }, null);
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "xx" }, null);
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "xx" }, "D");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "c" }, "B");
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "c" }, null);
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "c" }, "B");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "a" }, "C");
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "a" }, "C");
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "a" }, "C");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "a", "b" }, "A");
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "a", "b" }, "A");
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "a", "b" }, "A");

				yield return (FlagMatchingStrategy.AllFlags, new[] { "a", "x" }, null);
				yield return (FlagMatchingStrategy.ExactFlags, new[] { "a", "x" }, null);
				yield return (FlagMatchingStrategy.MaximumFlags, new[] { "a", "x" }, "C");
			}

			return GetItems().Select(t => TupleToObjects(t));
		}

		[Theory]
		[MemberData(nameof(GetComplexEntryData))]
		internal void TestComplexEntry(FlagMatchingStrategy strategy, IEnumerable<string> flags, string? expected)
		{
			var entry = new ComplexDictionaryEntry(new[]
			{
				new DictionaryEntryVariant(new[] { "a", "b" }.ToHashSet(), "A"),
				new DictionaryEntryVariant(new[] { "a", "c" }.ToHashSet(), "B"),
				new DictionaryEntryVariant(new[] { "a" }.ToHashSet(), "C"),
				new DictionaryEntryVariant(Array.Empty<string>().ToHashSet(), "D")
			});

			var resolveResult = entry.Resolve(strategy, flags.ToHashSet());

			if (expected == null)
			{
				resolveResult.Should().BeNull();
			}
			else
			{
				resolveResult.Should().Be(expected);
			}
		}
	}
}
