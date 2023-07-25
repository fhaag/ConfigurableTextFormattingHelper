using xfm = xFunc.Maths;

namespace ConfigurableTextFormattingHelper.Infrastructure.Conditions
{
	internal sealed class Condition
	{
		public Condition(string? expression)
		{
			var proc = new xfm.Processor();
			executableExpression = proc.Parse(expression ?? "1==1");
		}

		private readonly xfm.Expressions.IExpression executableExpression;

		public bool Evaluate(IValueProvider runtimeState)
		{
			ArgumentNullException.ThrowIfNull(runtimeState);

			var intParams = runtimeState.Values.SelectMany(v => v.Value.IntegerValues.Select<IntegerValue, xfm.Expressions.Parameters.Parameter>(iv => new(iv.Id, new xfm.Expressions.Parameters.ParameterValue(iv.Value))));
			var exprParams = new xfm.Expressions.Parameters.ExpressionParameters(intParams);

			var result = executableExpression.Execute(exprParams);
			return result != null;
		}
	}
}
