using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
{
	internal abstract class Value
	{
		protected Value(string id)
		{
			Id = id;
		}

		public string Id { get; }

		public abstract IEnumerable<IntegerValue> IntegerValues { get; }

		public abstract Value Clone();
	}
}
