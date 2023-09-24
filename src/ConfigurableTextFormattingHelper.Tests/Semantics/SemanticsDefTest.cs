using ConfigurableTextFormattingHelper.Documents;
using ConfigurableTextFormattingHelper.Infrastructure;
using ConfigurableTextFormattingHelper.Semantics;
using ConfigurableTextFormattingHelper.Tests.TestInfrastructure;
using Doc = ConfigurableTextFormattingHelper.Documents;
using OutputNodes = ConfigurableTextFormattingHelper.Semantics.OutputNodes;
using ConfigurableTextFormattingHelper.Infrastructure.Expressions;

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

			var elRule = new ElementRuleDef("text1");
			semantics.Elements["text1"] = new[] { elRule };

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

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			semantics.Elements["text1"] = new[] { elRule };

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
		public void TestCommandToSetValueOutput()
		{
			var semantics = new SemanticsDef();
			semantics.Values["v"] = new IntegerValue("v");

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			elRule.Output.Add(new OutputNodes.SetValueOutput(OutputNodes.SetValueOutput.SetValueMode.Assign, "v", new EvaluatableExpression("50")));
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			semantics.Elements["text1"] = new[] { elRule };

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("0"),
						ExpectedNode.Literal("50")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToIncValueOutput()
		{
			var semantics = new SemanticsDef();
			semantics.Values["v"] = new IntegerValue("v") { Value = 5 };

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			elRule.Output.Add(new OutputNodes.SetValueOutput(OutputNodes.SetValueOutput.SetValueMode.Increase, "v", new EvaluatableExpression("3")));
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			semantics.Elements["text1"] = new[] { elRule };

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("5"),
						ExpectedNode.Literal("8")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToDecValueOutput()
		{
			var semantics = new SemanticsDef();
			semantics.Values["v"] = new IntegerValue("v") { Value = 5 };

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			elRule.Output.Add(new OutputNodes.SetValueOutput(OutputNodes.SetValueOutput.SetValueMode.Decrease, "v", new EvaluatableExpression("30")));
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("v")));
			semantics.Elements["text1"] = new[] { elRule };

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("5"),
						ExpectedNode.Literal("-25")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToIntegerOutput()
		{
			var semantics = new SemanticsDef();
			semantics.Values["AA"] = new IntegerValue("AA") { Value = 20 };
			semantics.Values["BB"] = new IntegerValue("BB") { Value = 25 };
			semantics.Values["CC"] = new IntegerValue("CC") { Value = 1033 };

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("AA")));
			elRule.Output.Add(new OutputNodes.IntegerOutput(new EvaluatableExpression("BB + CC")));
			semantics.Elements["text1"] = new[] { elRule };

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("20"),
						ExpectedNode.Literal("1058")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToConditionalVerbatimOutput()
		{
			var semantics = new SemanticsDef();

			semantics.Values["x"] = new IntegerValue("x")
			{
				Value = 0
			};

			var elRule1 = new ElementRuleDef("text1");
			elRule1.Condition = new EvaluatableExpression("x > 5");
			elRule1.Output.Add(new OutputNodes.VerbatimOutput("A"));

			var elRule2 = new ElementRuleDef("text1");
			elRule2.Output.Add(new OutputNodes.VerbatimOutput("B"));
			elRule2.Output.Add(new OutputNodes.SetValueOutput(OutputNodes.SetValueOutput.SetValueMode.Assign, "x", new EvaluatableExpression("10")));

			semantics.Elements["text1"] = new[] { elRule1, elRule2 };

			var cmdDef = new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") });
			var input = new Doc.Span();
			input.AddElement(new Doc.Command(cmdDef));
			input.AddElement(new Doc.Command(cmdDef));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("B"),
						ExpectedNode.Literal("A")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestTwoCommandsToVerbatimOutput()
		{
			var semantics = new SemanticsDef();

			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			semantics.Elements["text1"] = new[] { elRule };

			elRule = new ElementRuleDef("text2");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("opqr"));
			semantics.Elements["text2"] = new[] { elRule };

			var input = new Doc.Span();
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text1", new[] { new MatchSettings(@"\[text1\]") })));
			input.AddElement(new Doc.Command(new ConfigurableTextFormattingHelper.Syntax.CommandDef("text2", new[] { new MatchSettings(@"\[text2\]") })));

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("abc"),
						ExpectedNode.Literal("opqr")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}

		[Fact]
		public void TestCommandToTwoVerbatimOutput()
		{
			var semantics = new SemanticsDef();
			
			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			elRule.Output.Add(new OutputNodes.VerbatimOutput("def5"));
			semantics.Elements["text1"] = new[] { elRule };

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
			
			var elRule = new ElementRuleDef("text1");
			elRule.Output.Add(new OutputNodes.RenderingInstructionOutput("test"));
			semantics.Elements["text1"] = new[] { elRule };

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

			var spanRule = new ElementRuleDef("sp1");
			spanRule.Output.Add(new OutputNodes.SpanContentOutput());
			spanRule.Output.Add(new OutputNodes.VerbatimOutput("ijkl"));
			spanRule.Output.Add(new OutputNodes.SpanContentOutput());
			semantics.Elements["sp1"] =	new[] { spanRule };

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

		[Fact]
		public void TestSpanWithDifferentContentOutput()
		{
			const string altContentId = "title";

			var semantics = new SemanticsDef();

			var spanRule = new ElementRuleDef("sp1");
			spanRule.Output.Add(new OutputNodes.VerbatimOutput("abc"));
			spanRule.Output.Add(new OutputNodes.SpanContentOutput
			{
				ContentId = altContentId
			});
			spanRule.Output.Add(new OutputNodes.SpanContentOutput());
			semantics.Elements["sp1"] = new[] { spanRule };

			var input = new Doc.Span();

			var defSpan = new Doc.DefinedSpan(new ConfigurableTextFormattingHelper.Syntax.SpanDef("sp1", new[] { new MatchSettings(@"\(") }, new[] { new MatchSettings(@"\)") }), 1);
			defSpan.AddElement(new Literal("klmno"));
			defSpan.SwitchToContent(altContentId);
			defSpan.AddElement(new Literal("pqrs"));

			input.AddElement(defSpan);

			CheckProcessingResult(semantics, input, actualOutput =>
			{
				var expectedOutput = ExpectedNode.Span(
					ExpectedNode.Span(
						ExpectedNode.Literal("abc"),
						ExpectedNode.Literal("pqrs"),
						ExpectedNode.Literal("klmno")
						)
					);

				actualOutput.Should().BeSameDocumentAs(expectedOutput);
			});
		}
	}
}
