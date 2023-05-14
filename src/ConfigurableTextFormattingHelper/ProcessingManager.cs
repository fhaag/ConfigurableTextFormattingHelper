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

namespace ConfigurableTextFormattingHelper
{
	using Infrastructure;

	/// <summary>
	/// Represents a process of processing a set of input files and generating output.
	/// </summary>
	/// <remarks>
	/// <para>Each instance of this class represents the process of processing a set of input files and generating output.
	///   It stores intermediate information and the final results of the operation.</para>
	/// </remarks>
	internal sealed class ProcessingManager : IProcessingMessageList
	{
		public void AddMessage(ProcessingMessage message) => messages.Add(message);

		private readonly List<ProcessingMessage> messages = new();

		public IReadOnlyList<ProcessingMessage> Messages => messages;

		/// <summary>
		/// Gets or sets the current project.
		/// </summary>
		public Projects.Project? Project { get; set; }

		public string? ProjectPath { get; set; }

		private IEnumerable<string> GetSearchPaths()
		{
			if (ProjectPath != null)
			{
				var pjDir = Path.GetDirectoryName(ProjectPath);
				if (!string.IsNullOrEmpty(pjDir))
				{
					yield return pjDir;
				}
			}

			var asmDir = Path.GetDirectoryName(typeof(ProcessingManager).Assembly.Location);
			if (!string.IsNullOrEmpty(asmDir))
			{
				yield return Path.Join(asmDir, "config");
			}
		}

		internal string? FindFile(params string[] pathCandidates)
		{
			var splitPathCandidates = pathCandidates.Select(pc => pc.Split('/')).ToArray();

			string? FindInBasePath(string basePath)
			{
				foreach (var pc in splitPathCandidates)
				{
					var path = Path.Join(new[] { basePath }.Concat(pc).ToArray());
					try
					{
						if (File.Exists(path))
						{
							return path;
						}
					}
					catch { }
				}
				return null;
			}

			foreach (var bp in GetSearchPaths())
			{
				var foundPath = FindInBasePath(bp);
				if (foundPath != null)
				{
					return foundPath;
				}
			}

			return null;
		}
	}
}
