namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal abstract class SettingsLoaderBase<TSettings>
	{
		protected abstract bool CanProcessFormat(SettingsFormat format);

		protected virtual SettingsFormat DetermineFormat(string path, SettingsFormat? format)
		{
			if (format.HasValue)
			{
				return format.Value;
			}

			return Path.GetExtension(path).ToLowerInvariant() switch
			{
				".yaml" => SettingsFormat.Yaml,
				_ => SettingsFormat.Unknown
			};
		}

		public TSettings LoadFromFile(string path, SettingsFormat? format = null)
		{
			ArgumentNullException.ThrowIfNull(path);

			var actualFormat = DetermineFormat(path, format);

			if (!CanProcessFormat(actualFormat))
			{
				throw new ArgumentException($"Cannot load file {path} as {typeof(TSettings)} with format {actualFormat}, as this format is not supported.");
			}

			var fileContent = File.ReadAllText(path);
			return Load(fileContent, actualFormat);
		}

		public TSettings LoadFromStream(Stream stream, SettingsFormat format)
		{
			ArgumentNullException.ThrowIfNull(stream);

			if (!CanProcessFormat(format))
			{
				throw new ArgumentException($"Cannot load stream as {typeof(TSettings)} with format {format}, as this format is not supported.");
			}

			using var sr = new StreamReader(stream);
			var streamContent = sr.ReadToEnd();
			return Load(streamContent, format);
		}

		public abstract TSettings Load(string rawData, SettingsFormat format);
	}
}
