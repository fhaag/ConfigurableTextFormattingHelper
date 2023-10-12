using ConfigurableTextFormattingHelper.Dictionaries;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Xunit;

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

		#region TestSimpleEntry

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

		#endregion

		#region TestComplexEntry

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

		#endregion

		#region TestExpansionFromRaw

		public static IEnumerable<object?[]> GetExpansionFromRawData()
		{
			IEnumerable<(Dictionaries.Raw.DictionaryEntry rawEntry, Dictionaries.DictionaryEntry importedEntry)> GetItems()
			{
				yield return (new(), new SimpleDictionaryEntry(""));

				yield return (new()
				{
					Text = "abc"
				}, new SimpleDictionaryEntry("abc"));

				yield return (new()
				{
					Text = "efgh",
					Variants =
					{
						{"x", new()
						{
							Text = "xyz"
						}
						}
					}
				}, new ComplexDictionaryEntry(new[] {
					new DictionaryEntryVariant(new[] {"x" }.ToHashSet(), "xyz"),
					new DictionaryEntryVariant(new HashSet<string>(), "efgh")
				}));

				yield return (new()
				{
					Text = "F",
					Variants =
					{
						{ "G", new()
						{
							Variants = {
								{ "J", new()
								{
									Text = "K"
								} }, { "L", new()
								{
									Text = "M"
								}
								}
							}
						}
						}, { "H", new()
						{
							Text = "I",
							Variants =
							{
								{ "N", new()
								{
									Text = "O"
								}
								}, { "P", new()
								{
									Text = "Q"
								}
								}
							}
						}
						}
					}
				}, new ComplexDictionaryEntry(new[]
				{
					new DictionaryEntryVariant(new[] {"G", "J" }.ToHashSet(), "K"),
					new DictionaryEntryVariant(new[] {"G", "L" }.ToHashSet(), "M"),
					new DictionaryEntryVariant(new[] {"H", "N" }.ToHashSet(), "O"),
					new DictionaryEntryVariant(new[] {"H", "P" }.ToHashSet(), "Q"),
					new DictionaryEntryVariant(new[] {"H" }.ToHashSet(), "I"),
					new DictionaryEntryVariant(new HashSet<string>(), "F")
				}));
			}

			return GetItems().Select(t => TupleToObjects(t));
		}

		[Theory]
		[MemberData(nameof(GetExpansionFromRawData))]
		internal void TestExpansionFromRaw(Dictionaries.Raw.DictionaryEntry rawEntry, Dictionaries.DictionaryEntry importedEntry)
		{
			var expanded = rawEntry.ToDictionaryEntry();

			expanded.Should().BeOfType(importedEntry.GetType());

			switch (expanded, importedEntry)
			{
				case (SimpleDictionaryEntry simple, SimpleDictionaryEntry expectedSimple):
					simple.Text.Should().Be(expectedSimple.Text);
					break;
				case (ComplexDictionaryEntry complex, ComplexDictionaryEntry expectedComplex):
					complex.Variants.Should().HaveSameCount(expectedComplex.Variants);

					for (var i = 0; i < complex.Variants.Count; i++)
					{
						var variant = complex.Variants[i];
						var expectedVariant = expectedComplex.Variants[i];

						variant.Text.Should().Be(expectedVariant.Text);
						variant.Flags.Should().BeEquivalentTo(expectedVariant.Flags);
					}
					break;
				default:
					throw new InvalidOperationException($"This case should never occur: Expected {importedEntry.GetType()}, but found {expanded.GetType()}.");
			}
		}

		#endregion
	}
}
