using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ConfigurableTextFormattingHelper.Infrastructure.Yaml
{
	internal static class DeserializerProvider
	{
		private static readonly IYamlTypeConverter[] typeConverters = new IYamlTypeConverter[]
		{
			new SingleStringValueListAdapter(),
			new MatcherAdapter(),
			new ListAdapter<RawMatchSettings>(),
		};

		public static IDeserializer Build(Func<DeserializerBuilder, DeserializerBuilder>? modifyBuilder = null,
			Type[]? skipConvertersForTypes = null)
		{
			var builder = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance);

			foreach (var tc in typeConverters)
			{
				if (skipConvertersForTypes != null)
				{
					if (skipConvertersForTypes.Any(t => tc.Accepts(t)))
					{
						continue;
					}
				}

				builder = builder.WithTypeConverter(tc);
			}

			if (modifyBuilder != null)
			{
				builder = modifyBuilder(builder);
			}

			return builder.Build();
		}
	}
}
