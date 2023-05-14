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

using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal sealed class ListAdapter<T> : IYamlTypeConverter
	{
		public bool Accepts(Type type) => typeof(IEnumerable<T>).IsAssignableFrom(type);

		private static object? CreateReturnValue(Type type, IEnumerable<T> items)
		{
			if (type == typeof(T))
			{
				return items.FirstOrDefault();
			}

			if (type.IsArray)
			{
				return items.ToArray();
			}

			if (type.IsAssignableTo(typeof(List<T>)) || type.IsAssignableTo(typeof(IList<T>)))
			{
				return items.ToList();
			}

			throw new NotSupportedException($"Unable to create a value of type {type}.");
		}

		public object? ReadYaml(IParser parser, Type type)
		{
			var deserializer = DeserializerProvider.Build(skipConvertersForTypes: new[] { typeof(IEnumerable<T>) });

			if (parser.TryConsume<SequenceStart>(out _))
			{
				var items = new List<T>();
				while (!parser.TryConsume<SequenceEnd>(out _))
				{
					if (parser.TryConsume<Comment>(out _))
					{
						continue;
					}

					var item = deserializer.Deserialize<T>(parser);
					items.Add(item);
				}

				return CreateReturnValue(type, items);
			}

			var singleValue = deserializer.Deserialize<T>(parser);
			if (singleValue == null)
			{
				return null;
			}

			return CreateReturnValue(type, new[] { singleValue });
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type)
		{
			throw new NotImplementedException();
		}
	}
}
