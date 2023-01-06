using System.Text.RegularExpressions;
using SD = ConfigurableTextFormattingHelper.Syntax;
using Doc = ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Tests.Parser
{
	public sealed class ParserTest
	{
		private void CheckParseResult(SD.SyntaxDef syntax, string source, Action<Doc.Span> checkResult)
		{
			var parser = new ConfigurableTextFormattingHelper.Parser(new ProcessingManager(syntax));

			var span = parser.Parse(source);

			checkResult(span);
		}

		#region syntax defs

		private static SD.SyntaxDef CreateSimpleSyntax()
		{
			var result = new SD.SyntaxDef();
			result.AddEscape("`");
			result.AddElement(new SD.CommandDef("a", new[] { "_AA_" }));
			result.AddElement(new SD.CommandDef("b", new[] { "%BBB%" }));
			return result;
		}

		private static SD.SyntaxDef CreateBasicSyntaxWithSpans()
		{
			var result = new SD.SyntaxDef();
			result.AddEscape("--");
			result.AddElement(new SD.SpanDef("s1", new[] { Regex.Escape("<<") }, new[] { Regex.Escape(">>") }));
			result.AddElement(new SD.SpanDef("s2", new[] { "§(?<testval>[A-Z]{3,6})§" }, new[] { ":§" }));
			return result;
		}

		#endregion

		[Fact]
		public void TestEmpty()
		{
			CheckParseResult(CreateSimpleSyntax(), "", span =>
			{
				span.Elements.Should().BeEmpty();
			});
		}

		[Theory]
		[InlineData(" ")]
		[InlineData("xyz")]
		[InlineData("aa_aa_aa")]
		[InlineData("123_AA4567")]
		[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla cursus ut neque nec venenatis. Praesent rutrum augue vitae dolor tempor, sit amet placerat elit placerat. Donec gravida sollicitudin massa. Etiam mauris risus, commodo non ullamcorper nec, venenatis nec ipsum. Praesent purus est, scelerisque ac ante non, venenatis efficitur elit.")]
		public void TestLiteralOnly(string sourceText)
		{
			CheckParseResult(CreateSimpleSyntax(), sourceText, span =>
			{
				span.Elements.Should().HaveCount(1);
				var literal = span.Elements.First().Should().BeOfType<Doc.Literal>().Which;
				literal.Parent.Should().Be(span);
				literal.Text.Should().Be(sourceText);
			});
		}

		[Theory]
		[InlineData("` ", " ")]
		[InlineData("a`b", "ab")]
		[InlineData("abAB`A", "abABA")]
		[InlineData("12345``678", "12345`678")]
		[InlineData("xx`yxy`z", "xxyxyz")]
		[InlineData("ab`", "ab")]
		[InlineData("def`_AA_", "def_AA_")]
		public void TestLiteralWithEscape(string sourceText, string resultText)
		{
			CheckParseResult(CreateSimpleSyntax(), sourceText, span =>
			{
				span.Elements.Should().HaveCount(1);
				var literal = span.Elements.First().Should().BeOfType<Doc.Literal>().Which;
				literal.Parent.Should().Be(span);
				literal.Text.Should().Be(resultText);
			});
		}

		[Theory]
		[InlineData("_AA_", new[] {"a"})]
		[InlineData("%BBB%", new[] {"b"})]
		[InlineData("_AA__AA__AA__AA_", new[] {"a", "a", "a", "a"})]
		[InlineData("%BBB%_AA_%BBB%", new[] {"b", "a", "b"})]
		public void TestCommandsOnly(string sourceText, string[] commandIds)
		{
			CheckParseResult(CreateSimpleSyntax(), sourceText, span =>
			{
				span.Elements.Should().HaveCount(commandIds.Length);
				for (var i = 0; i < commandIds.Length; i++)
				{
					var cmd = span.Elements[i].Should().BeOfType<Doc.Command>().Which;
					cmd.Parent.Should().Be(span);
					cmd.ElementDef.Id.Should().Be(commandIds[i]);
				}
			});
		}

		[Fact]
		public void TestCommandAtStart()
		{
			CheckParseResult(CreateSimpleSyntax(), "_AA_AAbcd", span =>
			{
				span.Elements.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);
				span.Elements[0].Should().BeOfType<Doc.Command>().Which.ElementDef.Id.Should().Be("a");
				span.Elements[1].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("AAbcd");
			});
		}

		[Fact]
		public void TestCommandAtEnd()
		{
			CheckParseResult(CreateSimpleSyntax(), "xxzxxz %BBB%", span =>
			{
				span.Elements.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);
				span.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("xxzxxz ");
				span.Elements[1].Should().BeOfType<Doc.Command>().Which.ElementDef.Id.Should().Be("b");
			});
		}

		[Fact]
		public void TestCommandBetween()
		{
			CheckParseResult(CreateSimpleSyntax(), "1234__AA_ABC", span =>
			{
				span.Elements.Should().HaveCount(3).And.OnlyContain(e => e.Parent == span);
				span.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("1234_");
				span.Elements[1].Should().BeOfType<Doc.Command>().Which.ElementDef.Id.Should().Be("a");
				span.Elements[2].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("ABC");
			});
		}

		[Fact]
		public void TestSpansOnly()
		{
			CheckParseResult(CreateBasicSyntaxWithSpans(), "<<§XXXX§<<>>:§<<>>>><<>>", span =>
			{
				span.Elements.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);

				var span1 = span.Elements[0].Should().BeOfType<Doc.Span>().Which;
				span1.ElementDef.Id.Should().Be("s1");
				span1.Elements.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span1);

				var span1Child1 = span1.Elements[0].Should().BeOfType<Doc.Span>().Which;
				span1Child1.ElementDef.Id.Should().Be("s2");
				span1Child1.Arguments.Should().ContainKey("testval").WhoseValue.Should().BeEquivalentTo(new[] { "XXXX" });
				span1Child1.Elements.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span1Child1);

				var innermost = span1Child1.Elements[0].Should().BeOfType<Doc.Span>().Which;
				innermost.ElementDef.Id.Should().Be("s1");
				innermost.Elements.Should().BeEmpty();

				var span1Child2 = span1.Elements[1].Should().BeOfType<Doc.Span>().Which;
				span1Child2.ElementDef.Id.Should().Be("s1");
				span1Child2.Elements.Should().BeEmpty();

				var span2 = span.Elements[1].Should().BeOfType<Doc.Span>().Which;
				span2.ElementDef.Id.Should().Be("s1");
				span2.Elements.Should().BeEmpty();
			});
		}

		[Fact]
		public void TestSpansWithLiterals()
		{
			CheckParseResult(CreateBasicSyntaxWithSpans(), "ab<<xyz>>def§AB§ABC§1234<< >>:§x", span =>
			{
				span.Elements.Should().HaveCount(5).And.OnlyContain(e => e.Parent == span);

				span.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("ab");

				var span1 = span.Elements[1].Should().BeOfType<Doc.Span>().Which;
				span1.ElementDef.Id.Should().Be("s1");
				span1.Elements.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span1);
				span1.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("xyz");

				span.Elements[2].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("def§AB");

				var span2 = span.Elements[3].Should().BeOfType<Doc.Span>().Which;
				span2.ElementDef.Id.Should().Be("s2");
				span2.Arguments.Should().ContainKey("testval").WhoseValue.Should().BeEquivalentTo(new[] { "ABC" });
				span2.Elements.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span2);
				span2.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("1234");
				
				var span2Child1 = span2.Elements[1].Should().BeOfType<Doc.Span>().Which;
				span2Child1.ElementDef.Id.Should().Be("s1");
				span2Child1.Elements.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span2Child1);
				span2Child1.Elements[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be(" ");

				span.Elements[4].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("x");
			});
		}
	}
}
