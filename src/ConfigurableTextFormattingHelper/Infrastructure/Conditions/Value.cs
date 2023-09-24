namespace ConfigurableTextFormattingHelper.Infrastructure.Expressions
{
	internal abstract class Value
	{
		protected Value(string id)
		{
			Id = id;
		}

		public string Id { get; }

		public abstract StringValue AsStringValue { get; }

		public abstract IEnumerable<IntegerValue> IntegerValues { get; }

		public abstract Value Clone();

		public static Value CreateFromStrings(string id, string value, bool useTypeHint)
		{
			var effectiveId = id;
			if (useTypeHint)
			{
				var typeHintIndex = id.LastIndexOf('_');
				if (typeHintIndex >= 0)
				{
					effectiveId = id.Substring(0, typeHintIndex);
					var typeHint = id.Substring(typeHintIndex + 1);
					switch (typeHint.ToLowerInvariant())
					{
						case "integer":
							int.TryParse(value, out var intValue);
							return new IntegerValue(effectiveId) { Value = intValue };
					}
				}
			}

			return new StringValue(effectiveId) { Value = value };
		}
	}
}
