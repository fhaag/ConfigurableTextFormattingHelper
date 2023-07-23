using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
{
	internal class ValueLengthReference : ValueReference
	{
		public ValueLengthReference(string valueName): base(valueName) { }
	}
}
