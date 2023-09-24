namespace ConfigurableTextFormattingHelper.Infrastructure.Expressions
{
	internal sealed class IntegerValue : Value
	{
		public IntegerValue(string id) : base(id)
		{
		}

		public int Value { get; set; }

		public override Value Clone() => new IntegerValue(Id)
		{
			Value = Value
		};

		public override StringValue AsStringValue => new StringValue(Id) { Value = Value.ToString(InvariantCulture) };

		public override IEnumerable<IntegerValue> IntegerValues
		{
			get
			{
				yield return this;
			}
		}
	}
}
