namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal sealed class RawMatchSettings
	{
		public RawMatchSettings()
		{
		}

		public RawMatchSettings(string? pattern)
		{
			Pattern = pattern;
		}

		/// <summary>
		/// The regular expression pattern to match.
		/// </summary>
		public string? Pattern { get; set; }

		/// <summary>
		/// Conditions that must apply to the values extracted from capturing groups in <see cref="Pattern"/>.
		/// </summary>
		public List<string>? Conditions { get; set; }

		public MatchSettings CreateMatchSettings()
		{
			if (Pattern == null)
			{
				throw new InvalidOperationException();
			}

			var pattern = Pattern.ToPreciseLocationStartRegex();

			return new MatchSettings(pattern);
		}
	}
}
