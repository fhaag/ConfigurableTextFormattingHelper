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
