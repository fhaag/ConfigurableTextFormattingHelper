namespace ConfigurableTextFormattingHelper.Semantics
{
	using Infrastructure;

	internal class SemanticsLoader : SettingsLoaderBase<SemanticsDef>
	{
		public SemanticsLoader(SemanticsProcessingManager processingManager)
		{
			ArgumentNullException.ThrowIfNull(processingManager);

			this.processingManager = processingManager;
		}

		private readonly SemanticsProcessingManager processingManager;

		private readonly Lazy<YamlDotNet.Serialization.IDeserializer> deserializer
			= new(() => Infrastructure.Yaml.DeserializerProvider.Build());

		protected override SettingsFormat DetermineFormat(string path, SettingsFormat? format)
		{
			var result = base.DetermineFormat(path, format);
			if (result == SettingsFormat.Unknown)
			{
				if (Path.GetExtension(path) == Constants.SemanticsExtension)
				{
					return SettingsFormat.Yaml;
				}
			}
			return result;
		}

		protected override bool CanProcessFormat(SettingsFormat format) => format == SettingsFormat.Yaml;

		public override SemanticsDef Load(string rawData, SettingsFormat format)
		{
			ArgumentNullException.ThrowIfNull(rawData);

			var rawDef = deserializer.Value.Deserialize<Raw.SemanticsDef>(rawData);

			return rawDef.ToRuntimeSemanticsDef(processingManager);
		}
	}
}
