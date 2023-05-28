using ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Tests.TestInfrastructure
{
	internal static class DocumentTextElementExtensions
	{
		public static DocumentTextElementAssertions Should(this TextElement instance)
		{
			return new DocumentTextElementAssertions(instance);
		}
	}
}
