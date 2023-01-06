namespace ConfigurableTextFormattingHelper.Syntax.Raw
{
	/// <summary>
	/// The raw syntax definition as loaded from the file.
	/// </summary>
	internal sealed class SyntaxDef
	{
		public List<string>? Escape { get; set; }

		public List<ElementDef>? Elements { get; set; }

		public void Populate(Syntax.SyntaxDef syntax)
		{
			ArgumentNullException.ThrowIfNull(syntax);

			if (Escape != null)
			{
				foreach (var ep in Escape)
				{
					syntax.AddEscape(ep);
				}
			}

			if (Elements != null)
			{
				foreach (var elDef in Elements)
				{
					syntax.AddElement(elDef.CreateElement());
				}
			}
		}
	}
}
