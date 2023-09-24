/*
MIT License

Copyright (c) 2023 The Configurable Text Formatting Helper Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.ComponentModel;
using ConfigurableTextFormattingHelper.Documents;
using ConfigurableTextFormattingHelper.Infrastructure.Expressions;

namespace ConfigurableTextFormattingHelper.Semantics.OutputNodes
{
	/// <summary>
	/// Output verbatim text.
	/// </summary>
	internal sealed class SetValueOutput : Output
	{
		public enum SetValueMode
		{
			Assign,
			Increase,
			Decrease
		}

		public SetValueOutput(SetValueMode mode, String id, EvaluatableExpression expression)
		{
			ArgumentNullException.ThrowIfNull(id);
			ArgumentNullException.ThrowIfNull(expression);

			switch (mode)
			{
				case SetValueMode.Assign:
				case SetValueMode.Increase:
				case SetValueMode.Decrease:
					Mode = mode;
					break;
				default:
					throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(SetValueMode));
			}

			Id = id;
			Expression = expression;
		}

		public SetValueMode Mode { get; }

		public String Id { get; }

		public EvaluatableExpression Expression { get; }

		public override IEnumerable<TextElement> Generate(ISubstitutionProcess process, IValueProvider context)
		{
			if (context.Values.TryGetValue(Id, out var val))
			{
				switch (val, Mode)
				{
					case (IntegerValue intVal, SetValueMode.Assign):
						intVal.Value = Expression.EvaluateToInteger(context);
						break;
					case (IntegerValue intVal, SetValueMode.Increase):
						intVal.Value += Expression.EvaluateToInteger(context);
						break;
					case (IntegerValue intVal, SetValueMode.Decrease):
						intVal.Value -= Expression.EvaluateToInteger(context);
						break;
					case (StringValue strVal, SetValueMode.Assign):
						break;
					default:
						throw new InvalidOperationException($"Invalid mode {Mode} for value type {val.GetType().Name}.");
				}
			}
			else
			{
				throw new InvalidOperationException($"Cannot find value {Id}.");
			}
			yield break;
		}
	}
}
