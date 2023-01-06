namespace ConfigurableTextFormattingHelper.Documents
{
	internal sealed class Span : ActiveTextElement
	{
		private sealed class ElementList : System.Collections.ObjectModel.Collection<TextElement>
		{
			public ElementList(Span owner)
			{
				this.owner = owner;
			}

			private readonly Span owner;

			protected override void InsertItem(int index, TextElement item)
			{
				if (item.Parent != null)
				{
					throw new ArgumentException("The text element is already a part of a span.");
				}

				base.InsertItem(index, item);

				item.Parent = owner;
			}

			protected override void RemoveItem(int index)
			{
				this[index].Parent = null;

				base.RemoveItem(index);
			}

			protected override void SetItem(int index, TextElement item)
			{
				if (item.Parent != null)
				{
					throw new ArgumentException("The text element to assign is already a part of a span.");
				}

				this[index].Parent = null;

				base.SetItem(index, item);

				item.Parent = owner;
			}

			protected override void ClearItems()
			{
				foreach (var item in this)
				{
					item.Parent = null;
				}

				base.ClearItems();
			}
		}

		public Span(Syntax.SpanDef elementDef) : base(elementDef)
		{
			Elements = new ElementList(this);
		}

		public new Syntax.SpanDef ElementDef => (Syntax.SpanDef)base.ElementDef;

		public IList<TextElement> Elements { get; }
	}
}
