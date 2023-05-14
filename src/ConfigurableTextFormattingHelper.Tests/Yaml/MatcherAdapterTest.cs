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

using ConfigurableTextFormattingHelper.Infrastructure.Yaml;
using YamlDotNet.Serialization;

namespace ConfigurableTextFormattingHelper.Tests.Yaml
{
	public class MatcherAdapterTest
	{
		private sealed class ObjectWithMatchers
		{
			public List<RawMatchSettings>? Match { get; set; }

			public string? Text { get; set; }
		}

		private static IDeserializer BuildDeserializer() => DeserializerProvider.Build();

		private void CheckDeserialization(string yaml, Action<ObjectWithMatchers>? checkFunc = null)
		{
			var deserializer = BuildDeserializer();

			var obj = deserializer.Deserialize<ObjectWithMatchers>(yaml);

			obj.Should().NotBeNull();

			if (checkFunc != null)
			{
				checkFunc(obj);
			}
		}

		[Fact]
		public void TestNoMatch()
		{
			CheckDeserialization(@"--- # test
text: null", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().BeNullOrEmpty();
			});
		}

		[Fact]
		public void TestSingleSimpleMatch()
		{
			CheckDeserialization(@"--- # test
match: abc", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestSingleListSimpleMatch()
		{
			CheckDeserialization(@"--- # test
match:
  - abc", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestMultiListSimpleMatch()
		{
			CheckDeserialization(@"--- # test
match:
  - abc
  - def", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(2);
			});
		}

		[Fact]
		public void TestSingleObjectMatch()
		{
			CheckDeserialization(@"--- # test
match:
  pattern: abc", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestSingleListObjectMatch()
		{
			CheckDeserialization(@"--- # test
match:
  - pattern: abc", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestMultiListObjectMatch()
		{
			CheckDeserialization(@"--- # test
match:
  - pattern: abc
  - pattern: def", obj =>
			{
				obj.Text.Should().BeNull();
				obj.Match.Should().HaveCount(2);
			});
		}

		[Fact]
		public void TestNoMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().BeNullOrEmpty();
			});
		}

		[Fact]
		public void TestSingleSimpleMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match: abc
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestSingleListSimpleMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match:
  - abc
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestMultiListSimpleMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match:
  - abc
  - def
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(2);
			});
		}

		[Fact]
		public void TestSingleObjectMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match:
  pattern: abc
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestSingleListObjectMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match:
  - pattern: abc
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(1);
			});
		}

		[Fact]
		public void TestMultiListObjectMatchBeforeOtherAttribute()
		{
			CheckDeserialization(@"--- # test
match:
  - pattern: abc
  - pattern: def
text: xyz", obj =>
			{
				obj.Text.Should().Be("xyz");
				obj.Match.Should().HaveCount(2);
			});
		}
	}
}
