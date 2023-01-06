namespace ConfigurableTextFormattingHelper.Documents
{
	/// <summary>
	/// The base class for all elements in a document that are subject to processing by the application.
	/// </summary>
	internal abstract class ActiveTextElement : TextElement
	{
		protected ActiveTextElement(Syntax.ElementDef elementDef)
		{
			ArgumentNullException.ThrowIfNull(elementDef);

			ElementDef = elementDef;
		}

		public Syntax.ElementDef ElementDef { get; }

		/// <summary>
		/// The ID that identifies the syntax element.
		/// </summary>
		public string Id => ElementDef.Id;

		public IDictionary<string, string[]> Arguments = new Dictionary<string, string[]>();
	}
}
