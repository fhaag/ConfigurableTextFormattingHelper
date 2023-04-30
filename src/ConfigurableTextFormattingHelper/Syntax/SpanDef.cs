using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;
	using Infrastructure.Yaml;

	/// <summary>
	/// A syntax element whose start and end can be recognized, and that encloses other document content.
	/// </summary>
	internal sealed class SpanDef : ElementDef
	{
		public const string DefaultContentId = "_default";

		public SpanDef(string elementId, IEnumerable<RawMatchSettings> startPatterns, IEnumerable<RawMatchSettings> endPatterns, IEnumerable<Raw.ContentDef>? contentSettings = null, string? initialContent = null, IEnumerable<Raw.ContentSwitchDef>? contentSwitches = null, Raw.LevelPolicy? level = null) : base(elementId)
		{
			this.startPatterns.AddRange(startPatterns.Select(ms => ms.CreateMatchSettings()));
			this.endPatterns.AddRange(endPatterns.Select(ms => ms.CreateMatchSettings()));

			if (contentSwitches != null)
			{
				this.contentSwitches.AddRange(contentSwitches.Select(cs => cs.CreateContentSwitch()));
			}

			if (contentSettings != null)
			{
				foreach (var rawCs in contentSettings)
				{
					var cs = rawCs.CreateContentDef();
					this.contentSettings[cs.Id] = cs;
				}
			}

			this.level = level?.CreateLevelPolicy() ?? new();
			InitialContent = initialContent ?? DefaultContentId;
		}

		private readonly List<MatchSettings> startPatterns = new();

		private readonly List<MatchSettings> endPatterns = new();

		private readonly LevelPolicy level;

		public override Match? FindInText(string text, int charIndex) => startPatterns.FindMatch(text, charIndex);

		public Match? FindEndInText(string text, int charIndex) => endPatterns.FindMatch(text, charIndex);

		public int DetermineLevel(int? enclosingLevel, IReadOnlyDictionary<string, string[]> arguments) => level.DetermineLevel(enclosingLevel, arguments);

		private readonly List<ContentSwitchDef> contentSwitches = new();

		private readonly Dictionary<string, ContentDef> contentSettings = new();

		public string InitialContent { get; }

		public (Match Match, string NewContentId)? FindContentSwitchInText(string text, int charIndex, string currentContentId)
		{
			var matchingResult = contentSwitches.Select(cs => new
			{
				SwitchDef = cs,
				Match = cs.FindInText(text, charIndex, currentContentId)
			}).FirstOrDefault(csInfo => csInfo.Match != null);

			if (matchingResult != null)
			{
				return (matchingResult.Match!, matchingResult.SwitchDef.To);
			}

			return null;
		}

		public ContentDef GetContentDefinition(string contentId)
		{
			contentSettings.TryGetValue(contentId, out var contentDef);
			return contentDef ?? new ContentDef
			{
				Id = contentId,
				Mode = ContentMode.Append
			};
		}
	}
}
