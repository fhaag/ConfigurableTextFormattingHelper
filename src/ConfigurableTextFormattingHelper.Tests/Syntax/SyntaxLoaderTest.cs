using ConfigurableTextFormattingHelper.Infrastructure;
using ConfigurableTextFormattingHelper.Syntax;
using SD = ConfigurableTextFormattingHelper.Syntax;

namespace ConfigurableTextFormattingHelper.Tests.Syntax
{
	public sealed class SyntaxLoaderTest
	{
		public SyntaxLoaderTest(ITestOutputHelper output)
		{
			this.output = output;
		}

		private readonly ITestOutputHelper output;

		private static string ExpectedRegexPattern(string pattern) => pattern.ToPreciseLocationStartRegex().ToString();

		private void CheckLoadedSyntax(string syntaxYaml, Action<SD.SyntaxDef> checkSyntax)
		{
			var loader = new SD.SyntaxLoader();
			var sd = loader.Load(syntaxYaml, Infrastructure.SettingsFormat.Yaml);
			checkSyntax(sd);
		}

		[Fact]
		public void TestEmptySyntax()
		{
			CheckLoadedSyntax(@"
--- # empty syntax
", sd =>
			{
				sd.EscapePatterns.Should().BeEmpty("because the empty syntax contains no escape patterns");
				sd.Elements.Should().BeEmpty("because the empty syntax contains no elements");
			});
		}

		[Fact]
		public void TestSingleEscape()
		{
			CheckLoadedSyntax(@"
--- # syntax with a single escape pattern
escape: ab\.x
", sd =>
			{
				sd.EscapePatterns.Should().HaveCount(1).And.ContainSingle(ex => ex.ToString() == ExpectedRegexPattern(@"ab\.x"));
				sd.Elements.Should().BeEmpty("because the syntax contains no elements");
			});
		}

		[Fact]
		public void TestMultipleEscapes()
		{
			CheckLoadedSyntax(@"
--- # syntax with multiple escape patterns
escape:
  - xxx
  - 123456
  - '\(R:'
", sd =>
			{
				sd.EscapePatterns.Should().HaveCount(3)
					.And.ContainSingle(ex => ex.ToString() == ExpectedRegexPattern(@"xxx"))
					.And.ContainSingle(ex => ex.ToString() == ExpectedRegexPattern(@"123456"))
					.And.ContainSingle(ex => ex.ToString() == ExpectedRegexPattern(@"\(R:"));
				sd.Elements.Should().BeEmpty("because the syntax contains no elements");
			});
		}

		[Fact]
		public void TestTokenWithPattern()
		{
			CheckLoadedSyntax(@"
--- # syntax with a single token defined only by a pattern
elements:
  - elementId: a
    ruleId: r1
    match: abc
", sd =>
			{
				sd.Elements.Should().HaveCount(1);

				var elDef = sd.Elements[0].Should().BeOfType<CommandDef>().Which;
				elDef.ElementId.Should().Be("a");
				elDef.RuleId.Should().Be("r1");
				elDef.FindInText("abc", 0).Should().Match<System.Text.RegularExpressions.Match>(m => m.Success && m.Value == "abc");
			});
		}

		[Fact]
		public void TestTokenWithPatternInObject()
		{
			CheckLoadedSyntax(@"
--- # syntax with a single token defined by a pattern in an object
elements:
  - elementId: a
    match:
      pattern: abc
", sd =>
			{
				sd.Elements.Should().HaveCount(1);

				var elDef = sd.Elements[0].Should().BeOfType<CommandDef>().Which;
				elDef.ElementId.Should().Be("a");
				elDef.RuleId.Should().BeNull();
				elDef.FindInText("abc", 0).Should().Match<System.Text.RegularExpressions.Match>(m => m.Success && m.Value == "abc");
			});
		}

		[Fact]
		public void TestTokenWithPatternInList()
		{
			CheckLoadedSyntax(@"
--- # syntax with a single token defined by a pattern in a list
elements:
  - elementId: a
    match:
      - abc
", sd =>
			{
				sd.Elements.Should().HaveCount(1);

				var elDef = sd.Elements[0].Should().BeOfType<CommandDef>().Which;
				elDef.ElementId.Should().Be("a");
				elDef.RuleId.Should().BeNull();
				elDef.FindInText("abc", 0).Should().Match<System.Text.RegularExpressions.Match>(m => m.Success && m.Value == "abc");
			});
		}

		[Fact]
		public void TestTokenWithPatternInObjectInList()
		{
			CheckLoadedSyntax(@"
--- # syntax with a single token defined by a pattern in an object in a list
elements:
  - elementId: a
    match:
      - pattern: abc
", sd =>
			{
				sd.Elements.Should().HaveCount(1);

				var elDef = sd.Elements[0].Should().BeOfType<CommandDef>().Which;
				elDef.ElementId.Should().Be("a");
				elDef.RuleId.Should().BeNull();
				elDef.FindInText("abc", 0).Should().Match<System.Text.RegularExpressions.Match>(m => m.Success && m.Value == "abc");
			});
		}
	}
}
