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
