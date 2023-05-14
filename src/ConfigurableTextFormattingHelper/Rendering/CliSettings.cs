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

using CommandLine;

namespace ConfigurableTextFormattingHelper.Rendering
{
	public class CliSettings
	{
		[Value(0, HelpText = "Specifies the path of the project to process.", MetaName = "project", Required = true)]
		public string? ProjectPath { get; set; }

		public string GetDefaultOutputName(string extension)
		{
			if (ProjectPath == null)
			{
				throw new InvalidOperationException("No input project path set.");
			}

			return Path.Combine(Path.GetDirectoryName(ProjectPath) ?? Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(ProjectPath) + extension);
		}
	}
}
