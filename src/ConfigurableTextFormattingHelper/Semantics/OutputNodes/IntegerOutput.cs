using ConfigurableTextFormattingHelper.Documents;
using ConfigurableTextFormattingHelper.Infrastructure.Expressions;

namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
{
	/// <summary>
	/// Output an integer number.
	/// </summary>
	internal sealed class IntegerOutput : Output
	{
		public IntegerOutput(EvaluatableExpression expression)
		{
			ArgumentNullException.ThrowIfNull(expression);

			Expression = expression;
		}

		public EvaluatableExpression Expression { get; }

		public override IEnumerable<TextElement> Generate(ISubstitutionProcess process, IValueProvider context)
		{
			yield return new Literal(Expression.EvaluateToInteger(context).ToString(InvariantCulture));
		}
	}
}
