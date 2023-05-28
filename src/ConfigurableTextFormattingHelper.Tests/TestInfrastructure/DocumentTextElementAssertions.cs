using static System.Globalization.CultureInfo;
using FluentAssertions.Primitives;
using ConfigurableTextFormattingHelper.Documents;
using FluentAssertions.Execution;

namespace ConfigurableTextFormattingHelper.Tests.TestInfrastructure
{
	internal sealed class DocumentTextElementAssertions : ReferenceTypeAssertions<TextElement, DocumentTextElementAssertions>
	{
		public DocumentTextElementAssertions(TextElement instance) : base(instance) { }

		protected override string Identifier => "text element";

		public AndConstraint<DocumentTextElementAssertions> BeSameDocumentAs(ExpectedNode expected, string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.ForCondition(expected != null)
				.FailWith("The expected tree cannot be null.");

			CheckSubtree(expected!, Subject, "/");

			void CheckSubtree(ExpectedNode expected, TextElement actual, string path)
			{
				void CheckChildren(IReadOnlyList<ExpectedNode>? expected, IReadOnlyList<TextElement> actual)
				{
					Execute.Assertion
						.BecauseOf(because, becauseArgs)
						.ForCondition((expected?.Count ?? 0) == actual.Count)
						.FailWith("Expected {context:text element} at path {2} to have {0} child(ren){reason}, but found {1}:" + string.Join("", actual.Select(ac => "\n  " + ac.ToString())),
							expected?.Count, actual.Count, path);

					if (expected != null)
					{
						for (var i = 0; i < expected.Count; i++)
						{
							CheckSubtree(expected[i], actual[i], path + i.ToString(InvariantCulture) + "/");
						}
					}
				}

				Execute.Assertion
					.BecauseOf(because, becauseArgs)
					.ForCondition(expected.NodeType == actual.GetType())
					.FailWith("Expected {context:text element} at path {2} to be of type {0}{reason}, but found {1}.",
						expected.NodeType, actual.ToString(), path);

				switch (actual)
				{
					case DefinedSpan ds:
						Execute.Assertion
							.BecauseOf(because, becauseArgs)
							.ForCondition(expected.ElementId == ds.ElementDef.ElementId)
							.FailWith("Expected {context:text element} at path {2} to be defined by element ID {0}{reaosn}, but found {1}.",
								expected.ElementId, ds.ElementDef.ElementId, path)
							.Then
							.ForCondition(expected.Level == ds.Level)
							.FailWith("Expected {context:text element} at path {2} to be on nesting level {0}{reason}, but found {1}.",
								expected.Level, ds.Level, path);

						CheckChildren(expected.DefaultContent, ds.GetDefaultContent());
						break;
					case Span s:
						CheckChildren(expected.DefaultContent, s.GetDefaultContent());
						break;
					case Literal l:
						Execute.Assertion
							.BecauseOf(because, becauseArgs)
							.ForCondition(l.Text == expected.Text)
							.FailWith("Expected literal text of {context:text element} at path {2} to be '{0}'{reason}, but found '{1}'.",
								expected.Text, l.Text, path);
						break;
				}
			}

			return new(this);
		}
	}
}
