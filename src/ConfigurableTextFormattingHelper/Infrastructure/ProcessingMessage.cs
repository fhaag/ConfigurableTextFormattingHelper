namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal record ProcessingMessage(ProcessingStage ProcessingStage, MessageSeverity Severity, MessageContent Message, InputOrigin Origin);
}
