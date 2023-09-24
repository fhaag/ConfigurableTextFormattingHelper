using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableTextFormattingHelper.Infrastructure.Expressions
{
	internal interface IValueProvider
	{
		IReadOnlyDictionary<string, Value> Values { get; }
	}
}
