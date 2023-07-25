using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
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

		public override IEnumerable<IntegerValue> IntegerValues
		{
			get
			{
				yield return this;
			}
		}
	}
}
