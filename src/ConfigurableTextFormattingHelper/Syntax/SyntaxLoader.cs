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

namespace ConfigurableTextFormattingHelper.Syntax
{
	using Infrastructure;

	internal sealed class SyntaxLoader : SettingsLoaderBase<SyntaxDef>
	{
		private readonly Lazy<YamlDotNet.Serialization.IDeserializer> deserializer
			= new(() => Infrastructure.Yaml.DeserializerProvider.Build());

		protected override SettingsFormat DetermineFormat(string path, SettingsFormat? format)
		{
			var result = base.DetermineFormat(path, format);
			if (result == SettingsFormat.Unknown)
			{
				if (Path.GetExtension(path) == Constants.SyntaxExtension)
				{
					return SettingsFormat.Yaml;
				}
			}
			return result;
		}

		protected override bool CanProcessFormat(SettingsFormat format) => format == SettingsFormat.Yaml;

		public override SyntaxDef Load(string rawData, SettingsFormat format)
		{
			ArgumentNullException.ThrowIfNull(rawData);
			
			var rawDef = deserializer.Value.Deserialize<Raw.SyntaxDef>(rawData);

			var result = new SyntaxDef();
			rawDef?.Populate(result);
			return result;
		}
	}
}
