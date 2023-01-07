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

			if (type.IsAssignableTo(typeof(List<string>)))
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
