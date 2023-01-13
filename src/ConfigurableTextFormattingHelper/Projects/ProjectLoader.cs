namespace ConfigurableTextFormattingHelper.Projects
{
	using Infrastructure;

	internal sealed class ProjectLoader : SettingsLoaderBase<Project>
	{
		private readonly Lazy<YamlDotNet.Serialization.IDeserializer> deserializer
			= new(() => Infrastructure.Yaml.DeserializerProvider.Build());

		protected override SettingsFormat DetermineFormat(string path, SettingsFormat? format)
		{
			var result = base.DetermineFormat(path, format);
			if (result == SettingsFormat.Unknown)
			{
				if (Path.GetExtension(path) == Constants.ProjectExtension)
				{
					return SettingsFormat.Yaml;
				}
			}
			return result;
		}

		protected override bool CanProcessFormat(SettingsFormat format) => format == SettingsFormat.Yaml;

		public override Project Load(string rawData, SettingsFormat format)
		{
			ArgumentNullException.ThrowIfNull(rawData);

			return deserializer.Value.Deserialize<Project>(rawData);
		}
	}
}
