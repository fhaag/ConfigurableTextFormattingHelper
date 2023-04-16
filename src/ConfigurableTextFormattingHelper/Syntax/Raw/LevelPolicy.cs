namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	internal class LevelPolicy
	{
		public string? FromParameter { get; set; }

		public int? Value { get; set; }

		public Syntax.LevelPolicy CreateLevelPolicy()
		{
			return new()
			{
				FromParameter = FromParameter,
				Value = Value
			};
		}
	}
}
