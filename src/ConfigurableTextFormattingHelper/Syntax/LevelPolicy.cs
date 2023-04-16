namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	internal class LevelPolicy
	{
		public string? FromParameter { get; set; }

		public int? Value { get; set; }

		public int DetermineLevel(int? enclosingLevel, IReadOnlyDictionary<string, string[]> arguments)
		{
			ArgumentNullException.ThrowIfNull(arguments);

			if (Value.HasValue)
			{
				return Value.Value;
			}

			if (FromParameter != null)
			{
				var rawVal = $"[[{FromParameter}]]".Expand(arguments);
				if (int.TryParse(rawVal, System.Globalization.NumberStyles.Integer, InvariantCulture, out var val))
				{
					return val;
				}
			}

			if (enclosingLevel.HasValue)
			{
				return enclosingLevel.Value + 1;
			}

			return 1;
		}
	}
}
