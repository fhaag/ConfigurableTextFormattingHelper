using ConfigurableTextFormattingHelper.Infrastructure.Yaml;
using YamlDotNet.Serialization.NamingConventions;
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

		/*
		 * This test case produces an infinite loop:
		 * 
		 * Der aktive Testlauf wurde abgebrochen. Grund: Der Testhostprozess ist abgestürzt. : Stack overflow.
   at YamlDotNet.Serialization.BuilderSkeleton`1[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]..ctor(YamlDotNet.Serialization.ITypeResolver)
   at YamlDotNet.Serialization.DeserializerBuilder..ctor()
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.DeserializerProvider.Build(System.Func`2<YamlDotNet.Serialization.DeserializerBuilder,YamlDotNet.Serialization.DeserializerBuilder>)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterNodeDeserializer.Deserialize(YamlDotNet.Core.IParser, System.Type, System.Func`3<YamlDotNet.Core.IParser,System.Type,System.Object>, System.Object ByRef)
   at YamlDotNet.Serialization.ValueDeserializers.NodeValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.ValueDeserializers.AliasValueDeserializer.DeserializeValue(YamlDotNet.Core.IParser, System.Type, YamlDotNet.Serialization.Utilities.SerializerState, YamlDotNet.Serialization.IValueDeserializer)
   at YamlDotNet.Serialization.Deserializer.Deserialize(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.Deserializer.Deserialize[[System.__Canon, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]](YamlDotNet.Core.IParser)
   at ConfigurableTextFormattingHelper.Infrastructure.Yaml.MatcherAdapter.ReadYaml(YamlDotNet.Core.IParser, System.Type)
   at YamlDotNet.Serialization.NodeDeserializers.TypeConverterN

Der Testlauf wurde mit dem Fehler System.Exception: One or more errors occurred.
 ---> System.Exception: Unable to read data from the transport connection: Eine vorhandene Verbindung wurde vom Remotehost geschlossen..
 ---> System.Exception: Eine vorhandene Verbindung wurde vom Remotehost geschlossen.
   at System.Net.Sockets.NetworkStream.Read(Span`1 buffer)
   --- End of inner exception stack trace ---
   at System.Net.Sockets.NetworkStream.Read(Span`1 buffer)
   at System.Net.Sockets.NetworkStream.ReadByte()
   at System.IO.BinaryReader.Read7BitEncodedInt()
   at System.IO.BinaryReader.ReadString()
   at Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.LengthPrefixCommunicationChannel.NotifyDataAvailable()
   at Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.TcpClientExtensions.MessageLoopAsync(TcpClient client, ICommunicationChannel channel, Action`1 errorHandler, CancellationToken cancellationToken)
   --- End of inner exception stack trace --- abgebrochen.
		 */
		// run with:
		// dotnet test ConfigurableTextFormattingHelper.Tests.dll -v n --filter "FullyQualifiedName~MatcherAdapterTest&FullyQualifiedName~TestMultiListObjectMatch"
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
