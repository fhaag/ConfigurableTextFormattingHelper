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
	internal sealed class ProcessingManager
	{
		public ProcessingManager(Syntax.SyntaxDef syntax)
		{
			ArgumentNullException.ThrowIfNull(syntax);

			SyntaxEnvironment = new(syntax);
		}

		public Syntax.SyntaxEnvironment SyntaxEnvironment { get; set; }

		public void LeaveSyntaxEnvironment()
		{
			if (SyntaxEnvironment.Parent == null)
			{
				throw new InvalidOperationException("The current syntax environment is the root environment.");
			}

			SyntaxEnvironment = SyntaxEnvironment.Parent;
		}

		public Syntax.SyntaxDef Syntax => SyntaxEnvironment.Syntax;

		public void AddMessage(ProcessingMessage message) => messages.Add(message);

		private readonly List<ProcessingMessage> messages = new();

		public IReadOnlyList<ProcessingMessage> Messages => messages;
	}
}
