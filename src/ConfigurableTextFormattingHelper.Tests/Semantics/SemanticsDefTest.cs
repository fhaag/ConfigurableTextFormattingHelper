using ConfigurableTextFormattingHelper.Documents;
using ConfigurableTextFormattingHelper.Infrastructure;
using ConfigurableTextFormattingHelper.Semantics;
using ConfigurableTextFormattingHelper.Tests.TestInfrastructure;
using Doc = ConfigurableTextFormattingHelper.Documents;
using OutputNodes = ConfigurableTextFormattingHelper.Semantics.OutputNodes;

namespace ConfigurableTextFormattingHelper.Tests.Semantics
{

	public sealed class SemanticsDefTest
	{
		private void CheckProcessingResult(SemanticsDef semantics, Documents.Span input, Action<Doc.Span> checkResult)
		{
			var processor = new SemanticsProcessor(new());

			var outputSpan = processor.Process(semantics, input);

			checkResult(outputSpan);
		}

		[Fact]
		public void TestVerbatimOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();
			input.AddElement(new Doc.Literal("abc"));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("abc")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToEmptyOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");

			var elRule = new ElementRuleDef("text1");
			defaultContext.Elements["text1"] = elRule;
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(Array.Empty<ExpectedNode>())
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToVerbatimOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			defaultContext.Elements["text1"] = elRule;
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("abc")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToTwoVerbatimOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");
			
			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			elRule.Output.Add(new OutputNodes.VerbatimOutput("def5"));
			defaultContext.Elements["text1"] = elRule;
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("abc"),
						ExpectedNode.Literal("def5")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToRenderingInstructionOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");
			
			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.RenderingInstructionOutput("test"));
			defaultContext.Elements["text1"] = elRule;
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.RenderingInstruction("test")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestSpanWithContentOutput()
		{
			var semantics = new SemanticsDef();
			var defaultContext = new ContextDef("default");

			var spanRule = new ElementRuleDef("sp1");
			spanRule.Output.Add(new OutputNodes.SpanContentOutput());
			spanRule.Output.Add(new OutputNodes.VerbatimOutput("ijkl"));
			spanRule.Output.Add(new OutputNodes.SpanContentOutput());
			defaultContext.Elements["sp1"] = spanRule;
			semantics.Contexts.Add(defaultContext);

			var input = new Doc.Span();

			var defSpan = new Doc.DefinedSpan(new ConfigurableTextFormattingHelper.Syntax.SpanDef("sp1", new[] { new MatchSettings(@"\(") }, new[] { new MatchSettings(@"\)") }), 1);
			defSpan.AddElement(new Literal("5uvw"));

			input.AddElement(defSpan);

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("5uvw"),
						ExpectedNode.Literal("ijkl"),
						ExpectedNode.Literal("5uvw")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}
	}
}
