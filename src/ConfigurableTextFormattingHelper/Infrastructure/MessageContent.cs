namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal sealed class MessageContent
	{
		public MessageContent(string text)
		{
			ArgumentNullException.ThrowIfNull(text);

			Text = text;
		}

		public string Text { get; }

		private readonly Dictionary<string, string> parameters = new();

		public IDictionary<string, string> Parameters => parameters;

		public string this[string paramName]
		{
			get => parameters[paramName];
			set => parameters[paramName] = value;
		}
	}
}
