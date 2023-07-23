using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
{
	internal class ComparisonCondition : Condition
	{
		public IList<Value> Operands { get; } = new List<Value>();

		public Comparison Comparison { get; set; } = Comparison.Equal;
	}
}
