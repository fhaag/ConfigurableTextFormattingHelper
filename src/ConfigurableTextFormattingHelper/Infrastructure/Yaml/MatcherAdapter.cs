using System.Diagnostics.Tracing;
using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal sealed class MatcherAdapter : IYamlTypeConverter
	{
		public bool Accepts(Type type) => typeof(RawMatchSettings).IsAssignableFrom(type) || typeof(IEnumerable<RawMatchSettings>).IsAssignableFrom(type);

		private static object? CreateReturnValue(Type type, IEnumerable<RawMatchSettings> items)
		{
			if (type == typeof(RawMatchSettings))
			{
				return items.FirstOrDefault();
			}

			if (type.IsArray)
			{
				return items.ToArray();
			}

			if (type.IsAssignableTo(typeof(List<RawMatchSettings>)) || type.IsAssignableTo(typeof(IList<RawMatchSettings>)))
			{
				return items.ToList();
			}

			throw new NotSupportedException($"Unable to create a RawMatcher-based value of type {type}.");
		}

		public object? ReadYaml(IParser parser, Type type)
		{
			var deserializer = DeserializerProvider.Build();

			Scalar? scalar;
			
			if (parser.TryConsume<Scalar>(out scalar))
			{
				return CreateReturnValue(type, new[] { new RawMatchSettings(scalar.Value) });
			}

			if (parser.Accept<MappingStart>(out _))
			{
				return CreateReturnValue(type, new[] { deserializer.Deserialize<RawMatchSettings>(parser) });
			}

			if (parser.TryConsume<SequenceStart>(out _))
			{
				var values = new List<RawMatchSettings>();
				do
				{
					if (parser.TryConsume<SequenceEnd>(out _))
					{
						break;
					}

					if (!parser.Accept<Comment>(out _))
					{
						var rms = deserializer.Deserialize<RawMatchSettings>(parser);
						values.Add(rms);
					}
				}
				while (parser.MoveNext());

				return CreateReturnValue(type, values);

				//parser.Accept<YamlDotNet.Core.Events.>
//				return CreateReturnValue(type, deserializer.Deserialize<RawMatchSettings[]>(parser));
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
