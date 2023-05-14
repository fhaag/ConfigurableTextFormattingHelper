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
