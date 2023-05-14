using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal sealed class MatcherAdapter : IYamlTypeConverter
	{
		public bool Accepts(Type type) => typeof(RawMatchSettings).IsAssignableFrom(type);

		public object? ReadYaml(IParser parser, Type type)
		{
			var deserializer = DeserializerProvider.Build(skipConvertersForTypes: new[] { typeof(RawMatchSettings) });

			if (parser.TryConsume<Scalar>(out var scalar))
			{
				return new RawMatchSettings(scalar.Value);
			}

			return deserializer.Deserialize<RawMatchSettings>(parser);
		}

		public void WriteYaml(IEmitter emitter, object? value, Type type)
		{
			throw new NotImplementedException();
		}
	}
}
