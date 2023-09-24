using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Expressions
{
	internal sealed class StringValue : Value
	{
		public StringValue(string id) : base(id)
		{
		}

		public string Value { get; set; } = "";

		public override Value Clone() => new StringValue(Id)
		{
			Value = Value
		};

		public override StringValue AsStringValue => this;

		public override IEnumerable<IntegerValue> IntegerValues
		{
			get
			{
				yield break;
			}
		}
	}
}
