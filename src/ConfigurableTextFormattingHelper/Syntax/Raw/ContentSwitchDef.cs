using ConfigurableTextFormattingHelper.Infrastructure.Yaml;

namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	internal sealed class ContentSwitchDef
	{
		public List<string>? From { get; set; }

		public string? To { get; set; }

		public List<RawMatchSettings>? Match { get; set; }

		public Syntax.ContentSwitchDef CreateContentSwitch()
		{
			if (To == null)
			{
				throw new InvalidOperationException("No target content defined.");
			}

			if (Match == null)
			{
				throw new InvalidOperationException("No match rules defined.");
			}

			return new(Match, To, From);
		}
	}
}
