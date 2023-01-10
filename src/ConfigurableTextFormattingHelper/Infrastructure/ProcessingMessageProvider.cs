namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal abstract class ProcessingMessageProvider
	{
		protected virtual string GetMessageText(int messageId) => throw new ArgumentException("Unknown message ID: " + GetFullMessageId(messageId));

		protected abstract string MessageTypePrefix { get; }

		private string GetFullMessageId(int messageId) => string.Format(InvariantCulture,
			"{0}{1}",
			MessageTypePrefix, messageId);

		public MessageContent CreateMessage(int messageId, Dictionary<string, string>? arguments = null)
		{
			var text = GetMessageText(messageId);

			if (arguments != null)
			{
				foreach (var pair in arguments)
				{
					text = text.Replace($"[[{pair.Key}]]", pair.Value);
				}
			}

			return new(GetFullMessageId(messageId), text);
		}
	}
}
