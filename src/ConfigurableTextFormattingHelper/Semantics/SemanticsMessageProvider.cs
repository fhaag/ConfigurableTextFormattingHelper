namespace ConfigurableTextFormattingHelper.Semantics
{
	using Infrastructure;

	internal sealed class SemanticsMessageProvider : ProcessingMessageProvider
	{
		protected override string MessageTypePrefix => "SEM";

		protected override string GetMessageText(int messageId) => messageId switch
		{
			100 => "No 'verbatim' attribute specified for verbatim output.",
			110 => "No 'renderingInstruction' attribute specified for rendering instruction output.",
			_ => base.GetMessageText(messageId)
		};
	}
}
