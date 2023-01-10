namespace ConfigurableTextFormattingHelper.App.Cli
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var plugins = Infrastructure.PluginLoader.LoadPlugins();

			if (args.Length >= 2)
			{
				switch (args[1].ToLowerInvariant())
				{
					case "help":
						// TODO: help text
						Console.WriteLine("TODO Help");
						break;
					case string targetId:
						{
							var runner = new Runner(plugins);
							runner.Execute(targetId, args.Skip(2).ToArray());
						}
						break;
				}
			}
		}
	}
}