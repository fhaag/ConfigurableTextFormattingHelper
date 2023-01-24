namespace ConfigurableTextFormattingHelper.Projects
{
	/// <summary>
	/// Represents a project, describing the source input for a document.
	/// </summary>
	internal sealed class Project
	{
		public string? Title { get; set; }

		public string? Description { get; set; }

		public IList<string>? Syntax { get; set; }

		public IList<string>? Semantics { get; set; }

		[YamlDotNet.Serialization.YamlMember(Alias = "content")]
		public IList<string>? Sources { get; set; }
	}
}
