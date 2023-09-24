using xFunc.Maths.Expressions;
using xfm = xFunc.Maths;

namespace ConfigurableTextFormattingHelper.Infrastructure.Expressions
{
	internal sealed class EvaluatableExpression
	{
		public EvaluatableExpression(string? expression)
		{
			var proc = new xfm.Processor();
			executableExpression = proc.Parse(expression ?? "1==1");
		}

		private readonly xfm.Expressions.IExpression executableExpression;

		public bool EvaluateToBoolean(IValueProvider runtimeState)
		{
			ArgumentNullException.ThrowIfNull(runtimeState);

			var intParams = runtimeState.Values.SelectMany(v => v.Value.IntegerValues.Select<IntegerValue, xfm.Expressions.Parameters.Parameter>(iv => new(iv.Id, new xfm.Expressions.Parameters.ParameterValue(iv.Value))));
			var exprParams = new xfm.Expressions.Parameters.ExpressionParameters(intParams);

			var result = executableExpression.Execute(exprParams);

			if (result is bool boolResult)
			{
				return boolResult;
			}
			return false;
		}

		public int EvaluateToInteger(IValueProvider runtimeState)
		{
			ArgumentNullException.ThrowIfNull(runtimeState);

			var intParams = runtimeState.Values.SelectMany(v => v.Value.IntegerValues.Select<IntegerValue, xfm.Expressions.Parameters.Parameter>(iv => new(iv.Id, new xfm.Expressions.Parameters.ParameterValue(iv.Value))));
			var exprParams = new xfm.Expressions.Parameters.ExpressionParameters(intParams);

			var result = executableExpression.Execute(exprParams);
			
			if (result is NumberValue numResult)
			{
				return Convert.ToInt32(numResult.Number);
			}
			return 0;
		}
	}
}
