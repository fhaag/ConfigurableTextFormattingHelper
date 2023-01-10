namespace ConfigurableTextFormattingHelper.Rendering
{
	public interface IRendererFactoryWithCliSettings : IRendererFactory
	{
		CliSettings? Settings { get; set; }

		Type SettingsType { get; }
	}

	public interface IRendererFactoryWithCliSettings<TSettings> : IRendererFactoryWithCliSettings
		where TSettings : CliSettings, new()
	{
		CliSettings? IRendererFactoryWithCliSettings.Settings
		{
			get => Settings;
			set => Settings = value as TSettings;
		}

		Type IRendererFactoryWithCliSettings.SettingsType => typeof(TSettings);

		new TSettings? Settings { get; set; }
	}
}
