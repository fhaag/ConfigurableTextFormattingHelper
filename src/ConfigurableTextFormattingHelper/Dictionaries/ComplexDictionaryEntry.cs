namespace ConfigurableTextFormattingHelper.Dictionaries
{
	internal sealed class ComplexDictionaryEntry : DictionaryEntry
	{
		public ComplexDictionaryEntry(IEnumerable<DictionaryEntryVariant> variants)
		{
			ArgumentNullException.ThrowIfNull(variants);

			Variants = variants.ToArray();
		}

		public IReadOnlyList<DictionaryEntryVariant> Variants { get; }

		private sealed record VariantMatch(DictionaryEntryVariant Variant, int MatchingFlagCount, int AdditionalFlagCount);

		public override string? Resolve(FlagMatchingStrategy strategy, IReadOnlySet<string> flags)
		{
			VariantMatch? bestCandidate = null;

			Func<DictionaryEntryVariant, bool> eligibleVariantFilter = strategy switch
			{
				FlagMatchingStrategy.ExactFlags => v => v.Flags.Count == flags.Count,
				FlagMatchingStrategy.AllFlags => v => v.Flags.Count >= flags.Count,
				_ => v => true
			};

			foreach (var variant in Variants.Where(eligibleVariantFilter))
			{
				var matchingFlags = variant.Flags.Intersect(flags).ToArray();

				switch (strategy)
				{
					case FlagMatchingStrategy.AllFlags:
					case FlagMatchingStrategy.ExactFlags:
						if (matchingFlags.Length < flags.Count)
						{
							continue;
						}
						break;
				}

				var newMatch = new VariantMatch(variant, matchingFlags.Length, variant.Flags.Count - matchingFlags.Length);

				if ((bestCandidate == null)
					|| (newMatch.MatchingFlagCount > bestCandidate.MatchingFlagCount)
					|| (newMatch.MatchingFlagCount == bestCandidate.MatchingFlagCount && newMatch.AdditionalFlagCount < bestCandidate.AdditionalFlagCount))
				{
					bestCandidate = newMatch;
				}
			}

			return bestCandidate?.Variant.Text;
		}
	}
}
