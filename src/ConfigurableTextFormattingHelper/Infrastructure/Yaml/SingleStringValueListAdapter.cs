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

using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal sealed class SingleStringValueListAdapter : IYamlTypeConverter
	{
		public bool Accepts(Type type) => typeof(IEnumerable<string>).IsAssignableFrom(type);

		private static object CreateEnumerable(Type type, IEnumerable<string> items)
		{
			if (type.IsArray)
			{
				return items.ToArray();
			}

			if (type.IsAssignableTo(typeof(List<string>)) || type.IsAssignableTo(typeof(IList<string>)))
			{
				return items.ToList();
			}

			throw new NotSupportedException($"Unable to create a string list of type {type}.");
		}

		public object? ReadYaml(IParser parser, Type type)
		{
			Scalar? scalar;

			if (parser.TryConsume<Scalar>(out scalar))
			{
				return CreateEnumerable(type, new[] { scalar.Value });
			}
			
			if (parser.TryConsume<SequenceStart>(out _))
			{
				var items = new List<string>();
				while (!parser.Accept<SequenceEnd>(out _))
				{
					if (parser.Accept<Scalar>(out scalar))
					{
						items.Add(scalar.Value);
					}
					else if (parser.Accept<Comment>(out _))
					{
						// do nothing
					}
					else
					{
						throw new NotSupportedException(string.Format(InvariantCulture,
							"Unexpected YAML token at line {0}, position {1}.",
							parser.Current!.Start.Line, parser.Current.Start.Column));
					}

					if (!parser.MoveNext())
					{
						break;
					}
				}

				parser.MoveNext();
				return CreateEnumerable(type, items);
			}

			throw new NotSupportedException(string.Format(InvariantCulture,
				"Unexpected YAML token at line {0}, position {1}.",
				parser.Current!.Start.Line, parser.Current.Start.Column));
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type)
		{
			throw new NotImplementedException();
		}
	}
}
