namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	internal sealed class ValueDef
	{
		public string? Id { get; set; }

		public string? Type { get; set; }

		public string? Value { get; set; }

		public Infrastructure.Expressions.Value CreateValue()
		{
			if (Id == null)
			{
				throw new InvalidOperationException();
			}

			switch (Type?.ToLowerInvariant())
			{
				case "integer":
					if (int.TryParse(Value, out var intValue))
					{
						return new Infrastructure.Expressions.IntegerValue(Id)
						{
							Value = intValue
						};
					}
					else
					{
						throw new InvalidOperationException();
					}
				case "string":
					return new Infrastructure.Expressions.StringValue(Id)
					{
						Value = Value ?? ""
					};
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
