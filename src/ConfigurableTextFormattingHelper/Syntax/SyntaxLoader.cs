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
