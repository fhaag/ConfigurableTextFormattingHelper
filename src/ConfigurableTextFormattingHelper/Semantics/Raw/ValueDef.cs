namespace ConfigurableTextFormattingHelper.Semantics.Raw
{
	internal sealed class ValueDef
	{
		public string? Id { get; set; }

		public string? Type { get; set; }

		public string? Value { get; set; }

		public Infrastructure.Conditions.Value CreateValue()
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
						return new Infrastructure.Conditions.IntegerValue(Id)
						{
							Value = intValue
						};
					}
					else
					{
						throw new InvalidOperationException();
					}
				case "string":
					return new Infrastructure.Conditions.StringValue(Id)
					{
						Value = Value ?? ""
					};
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
