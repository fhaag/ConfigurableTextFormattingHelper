namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	internal class ContentDef
	{
		public string? Id { get; set; }

		public string? Mode { get; set; }

		public Syntax.ContentDef CreateContentDef()
		{
			return new()
			{
				Id = Id ?? "",
				Mode = Mode?.ToLowerInvariant() switch
				{
					"once" => ContentMode.Once,
					"multi" => ContentMode.Multi,
					_ => ContentMode.Append
				}
			};
		}
	}
}
