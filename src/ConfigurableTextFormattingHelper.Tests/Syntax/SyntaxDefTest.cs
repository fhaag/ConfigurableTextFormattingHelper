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

using ConfigurableTextFormattingHelper.Infrastructure;
using SD = ConfigurableTextFormattingHelper.Syntax;
using System.Text.RegularExpressions;

namespace ConfigurableTextFormattingHelper.Tests.Syntax
{
	public sealed class SyntaxDefTest
	{
		[Fact]
		public void TestSimpleMatch()
		{
			var baseSyntax = new SD.SyntaxDef();
			baseSyntax.AddElement(new SD.CommandDef("A", new[] { new MatchSettings(new Regex("aa")) })
			{
				RuleId= "A"
			});

			var matchResult = baseSyntax.MatchElement("aa", 0);
			matchResult.Should().NotBeNull("because a match should be found in 'aa'");
			matchResult!.Value.Element.RuleId.Should().Be("A");
		}

		[Fact]
		public void TestRulePrecedence()
		{
			var baseSyntax = new SD.SyntaxDef();
			baseSyntax.AddElement(new SD.CommandDef("A", new[] { new MatchSettings(new Regex("aa")), new MatchSettings(new Regex("bb")) })
			{
				RuleId = "A"
			});

			var additionalSyntax = new SD.SyntaxDef();
			additionalSyntax.AddElement(new SD.CommandDef("B", new[] { new MatchSettings(new Regex("a")) })
			{
				RuleId = "B"
			});

			baseSyntax.Append(additionalSyntax);

			var matchResult = baseSyntax.MatchElement("aa", 0);
			matchResult.Should().NotBeNull("because a match should be found in 'aa'");
			matchResult!.Value.Element.RuleId.Should().Be("B");

			matchResult = baseSyntax.MatchElement("bb", 0);
			matchResult.Should().NotBeNull("because a match should be found in 'bb'");
			matchResult!.Value.Element.RuleId.Should().Be("A");
		}

		[Fact]
		public void TestRuleReplacement()
		{
			var baseSyntax = new SD.SyntaxDef();
			baseSyntax.AddElement(new SD.CommandDef("A", new[] { new MatchSettings(new Regex("aa")) })
			{
				RuleId = "A"
			});

			var additionalSyntax = new SD.SyntaxDef();
			additionalSyntax.AddElement(new SD.CommandDef("A", new[] { new MatchSettings(new Regex("bb")) })
			{
				RuleId = "A"
			});

			baseSyntax.Append(additionalSyntax);

			var matchResult = baseSyntax.MatchElement("bb", 0);
			matchResult.Should().NotBeNull("because a match should be found in 'bb'");
			matchResult!.Value.Element.RuleId.Should().Be("A");

			matchResult = baseSyntax.MatchElement("aa", 0);
			matchResult.Should().BeNull("because the syntax element matching 'aa' has been overridden");
		}
	}
}
