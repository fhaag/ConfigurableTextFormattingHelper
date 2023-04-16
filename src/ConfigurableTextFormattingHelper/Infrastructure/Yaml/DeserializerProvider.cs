using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal static class DeserializerProvider
	{
		public static IDeserializer Build(Func<DeserializerBuilder, DeserializerBuilder>? modifyBuilder = null)
		{
			var builder = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.WithTypeConverter(new SingleStringValueListAdapter())
				.WithTypeConverter(new MatcherAdapter());

			if (modifyBuilder != null)
			{
				builder = modifyBuilder(builder);
			}

			return builder.Build();
		}
	}
}
