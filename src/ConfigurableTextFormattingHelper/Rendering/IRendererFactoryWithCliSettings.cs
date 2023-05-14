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
