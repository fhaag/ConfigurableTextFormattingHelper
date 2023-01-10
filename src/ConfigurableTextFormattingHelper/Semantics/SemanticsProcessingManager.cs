namespace ConfigurableTextFormattingHelper.Semantics
{
	using Infrastructure;

	internal sealed class SemanticsProcessingManager : IProcessingMessageList
	{
		public SemanticsProcessingManager(ProcessingManager owner)
		{
			ArgumentNullException.ThrowIfNull(owner);

			this.owner = owner;
		}

		private readonly ProcessingManager owner;

		public void AddMessage(ProcessingMessage message) => owner.AddMessage(message);

		public ProcessingMessageProvider Messages { get; } = new SemanticsMessageProvider();
	}
}
