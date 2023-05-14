using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
