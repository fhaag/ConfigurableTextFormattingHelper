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
