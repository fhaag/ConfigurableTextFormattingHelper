﻿using System.Text.RegularExpressions;
using SD = ConfigurableTextFormattingHelper.Syntax;
using Doc = ConfigurableTextFormattingHelper.Documents;

namespace ConfigurableTextFormattingHelper.Tests.Parser
{
	using Infrastructure.Yaml;

	public sealed class ParserTest
	{
		private void CheckParseResult(SD.SyntaxDef syntax, string source, Action<Doc.Span> checkResult)
		{
			var parser = new ConfigurableTextFormattingHelper.Syntax.Parser(new ProcessingManager());

			var span = parser.Parse(syntax, source);

			checkResult(span);
		}

		#region syntax defs

		private static SD.SyntaxDef CreateSimpleSyntax()
		{
			var result = new SD.SyntaxDef();
			result.AddEscape("`");
			result.AddElement(new SD.CommandDef("a", new[] { new RawMatchSettings("_AA_") }));
			result.AddElement(new SD.CommandDef("b", new[] { new RawMatchSettings("%BBB%") }));
			return result;
		}

		private static SD.SyntaxDef CreateBasicSyntaxWithSpans()
		{
			var result = new SD.SyntaxDef();
			result.AddEscape("--");
			result.AddElement(new SD.SpanDef("s1", new[] { new RawMatchSettings(Regex.Escape("<<")) }, new[] { new RawMatchSettings(Regex.Escape(">>")) }, null));
			result.AddElement(new SD.SpanDef("s2", new[] { new RawMatchSettings("§(?<testval>[A-Z]{3,6})§") }, new[] { new RawMatchSettings(":§") }, null));
			return result;
		}

		#endregion

		[Fact]
		public void TestEmpty()
		{
			CheckParseResult(CreateSimpleSyntax(), "", span =>
			{
				span.GetDefaultContent().Should().BeEmpty();
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
				var content = span.GetDefaultContent();
				content.Should().HaveCount(1);
				var literal = content.First().Should().BeOfType<Doc.Literal>().Which;
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
				var content = span.GetDefaultContent();
				content.Should().HaveCount(1);
				var literal = content.First().Should().BeOfType<Doc.Literal>().Which;
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
				var content = span.GetDefaultContent();
				content.Should().HaveCount(commandIds.Length);
				for (var i = 0; i < commandIds.Length; i++)
				{
					var cmd = content[i].Should().BeOfType<Doc.Command>().Which;
					cmd.Parent.Should().Be(span);
					cmd.ElementDef.ElementId.Should().Be(commandIds[i]);
				}
			});
		}

		[Fact]
		public void TestCommandAtStart()
		{
			CheckParseResult(CreateSimpleSyntax(), "_AA_AAbcd", span =>
			{
				var content = span.GetDefaultContent();
				content.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);
				content[0].Should().BeOfType<Doc.Command>().Which.ElementDef.ElementId.Should().Be("a");
				content[1].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("AAbcd");
			});
		}

		[Fact]
		public void TestCommandAtEnd()
		{
			CheckParseResult(CreateSimpleSyntax(), "xxzxxz %BBB%", span =>
			{
				var content = span.GetDefaultContent();
				content.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);
				content[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("xxzxxz ");
				content[1].Should().BeOfType<Doc.Command>().Which.ElementDef.ElementId.Should().Be("b");
			});
		}

		[Fact]
		public void TestCommandBetween()
		{
			CheckParseResult(CreateSimpleSyntax(), "1234__AA_ABC", span =>
			{
				var content = span.GetDefaultContent();
				content.Should().HaveCount(3).And.OnlyContain(e => e.Parent == span);
				content[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("1234_");
				content[1].Should().BeOfType<Doc.Command>().Which.ElementDef.ElementId.Should().Be("a");
				content[2].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("ABC");
			});
		}

		[Fact]
		public void TestSpansOnly()
		{
			CheckParseResult(CreateBasicSyntaxWithSpans(), "<<§XXXX§<<>>:§<<>>>><<>>", span =>
			{
				var content = span.GetDefaultContent();
				content.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span);

				var span1 = content[0].Should().BeOfType<Doc.DefinedSpan>().Which;
				span1.ElementDef.ElementId.Should().Be("s1");
				var content1 = span1.GetDefaultContent();
				content1.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span1);

				var span1Child1 = content1[0].Should().BeOfType<Doc.DefinedSpan>().Which;
				span1Child1.ElementDef.ElementId.Should().Be("s2");
				span1Child1.Arguments.Should().ContainKey("testval").WhoseValue.Should().BeEquivalentTo(new[] { "XXXX" });
				var content1_1 = span1Child1.GetDefaultContent();
				content1_1.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span1Child1);

				var innermost = content1_1[0].Should().BeOfType<Doc.DefinedSpan>().Which;
				innermost.ElementDef.ElementId.Should().Be("s1");
				innermost.GetDefaultContent().Should().BeEmpty();

				var span1Child2 = content1[1].Should().BeOfType<Doc.DefinedSpan>().Which;
				span1Child2.ElementDef.ElementId.Should().Be("s1");
				var content1_2 = span1Child2.GetDefaultContent();
				content1_2.Should().BeEmpty();

				var span2 = content[1].Should().BeOfType<Doc.DefinedSpan>().Which;
				span2.ElementDef.ElementId.Should().Be("s1");
				var content2 = span2.GetDefaultContent();
				content2.Should().BeEmpty();
			});
		}

		[Fact]
		public void TestSpansWithLiterals()
		{
			CheckParseResult(CreateBasicSyntaxWithSpans(), "ab<<xyz>>def§AB§ABC§1234<< >>:§x", span =>
			{
				var content = span.GetDefaultContent();
				content.Should().HaveCount(5).And.OnlyContain(e => e.Parent == span);

				content[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("ab");

				var span1 = content[1].Should().BeOfType<Doc.DefinedSpan>().Which;
				span1.ElementDef.ElementId.Should().Be("s1");
				var content1 = span1.GetDefaultContent();
				content1.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span1);
				content1[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("xyz");

				content[2].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("def§AB");

				var span2 = content[3].Should().BeOfType<Doc.DefinedSpan>().Which;
				span2.ElementDef.ElementId.Should().Be("s2");
				span2.Arguments.Should().ContainKey("testval").WhoseValue.Should().BeEquivalentTo(new[] { "ABC" });
				var content2 = span2.GetDefaultContent();
				content2.Should().HaveCount(2).And.OnlyContain(e => e.Parent == span2);
				content2[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("1234");
				
				var span2Child1 = content2[1].Should().BeOfType<Doc.DefinedSpan>().Which;
				span2Child1.ElementDef.ElementId.Should().Be("s1");
				var content2_1 = span2Child1.GetDefaultContent();
				content2_1.Should().HaveCount(1).And.OnlyContain(e => e.Parent == span2Child1);
				content2_1[0].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be(" ");

				content[4].Should().BeOfType<Doc.Literal>().Which.Text.Should().Be("x");
			});
		}
	}
}
