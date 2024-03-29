﻿/*
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

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;
	using ExprValue = Infrastructure.Expressions.Value;

	public sealed class LevelPolicy
	{
		public LevelPolicy()
		{
		}

		public LevelPolicy(string fromParameter)
		{
			ArgumentNullException.ThrowIfNull(fromParameter);

			FromParameter = fromParameter;
		}

		public LevelPolicy(Int32 value)
		{
			ArgumentNullException.ThrowIfNull(value);

			Value = value;
		}

		public string? FromParameter { get; private set; }

		public int? Value { get; private set; }

		internal int DetermineLevel(int? enclosingLevel, IReadOnlyDictionary<string, ExprValue> arguments)
		{
			ArgumentNullException.ThrowIfNull(arguments);

			if (Value.HasValue)
			{
				return Value.Value;
			}

			if (FromParameter != null)
			{
				var rawVal = $"[[{FromParameter}]]".Expand(arguments);
				if (int.TryParse(rawVal, System.Globalization.NumberStyles.Integer, InvariantCulture, out var val))
				{
					return val;
				}
			}

			if (enclosingLevel.HasValue)
			{
				return enclosingLevel.Value + 1;
			}

			return 1;
		}
	}
}
