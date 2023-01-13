namespace ConfigurableTextFormattingHelper
{
	using Infrastructure;
	using Projects;
	using Rendering;

	/// <summary>
	/// Triggers the text processing.
	/// </summary>
	public sealed class Runner
	{
		public Runner()
		{
			plugins = PluginLoader.LoadPlugins();
		}

		public Runner(PluginDirectory plugins)
		{
			ArgumentNullException.ThrowIfNull(plugins);

			this.plugins = plugins;
		}

		private readonly PluginDirectory plugins;

		public void Execute(string rendererId, string[] args)
		{
			var rendererFactory = plugins.FindRendererById(rendererId);
			if (rendererFactory != null)
			{
				Execute(rendererFactory, args);
			}
			else
			{
				Console.WriteLine($"No renderer factory with ID {rendererId} found.");
			}
		}

		public void Execute(IRendererFactory rendererFactory, string[] args)
		{
			ArgumentNullException.ThrowIfNull(rendererFactory);

			var cfgRendererFactory = PrepareRenderer(rendererFactory);
			cfgRendererFactory.LoadSettings(new[] { "" }.Concat(args).ToArray());
			var renderer = cfgRendererFactory.RendererFactory.CreateRenderer();
			// TODO: actually execute conversion
		}

		private ConfiguredRendererFactory PrepareRenderer(IRendererFactory rendererFactory)
		{
			if (rendererFactory is IRendererFactoryWithCliSettings rendererFactoryWithSettings)
			{
				var cfgRendererType = typeof(TypedConfiguredRendererFactory<>).MakeGenericType(rendererFactoryWithSettings.SettingsType);
				var result = (ConfiguredRendererFactory?)Activator.CreateInstance(cfgRendererType, rendererFactory);

				if (result == null)
				{
					throw new InvalidOperationException($"Failed to initialize renderer factory of type {rendererFactory.GetType()}.");
				}

				return result;
			}

			return new SimpleConfiguredRendererFactory(rendererFactory);
		}

		private void Execute(CliSettings settings, IRenderer renderer)
		{
			var pjLoader = new ProjectLoader();
			var project = pjLoader.LoadFromFile(settings.ProjectPath);
			Execute(project, renderer);
		}

		private void Execute(Project project, IRenderer renderer)
		{
			ArgumentNullException.ThrowIfNull(project);
			ArgumentNullException.ThrowIfNull(renderer);

			// syntax
			var syntax = new Syntax.SyntaxDef();
			{
				var syntaxLoader = new Syntax.SyntaxLoader();

				foreach (var syntaxPath in project.Syntax)
				{
					var partialSyntax = syntaxLoader.LoadFromFile(syntaxPath);
					syntax.Append(partialSyntax);
				}
			}

			var mgr = new ProcessingManager(syntax);
			var semProc = new SemanticsProcessor(mgr);

			// semantics
			var semantics = new Semantics.SemanticsDef();
			{
				var semanticsLoader = new Semantics.SemanticsLoader(new(mgr));

				foreach (var semanticsPath in project.Semantics)
				{
					var partialSemantics = semanticsLoader.LoadFromFile(semanticsPath);
					semantics.Append(partialSemantics);
				}
			}

			var parser = new Parser(mgr);

			// TODO: parallelize this?
			foreach (var sourceFile in project.Sources)
			{
				var rawSpan = parser.Parse(sourceFile);
				var span = semProc.Process(rawSpan);

				// TODO: collect all resulting spans and processing messages
			}

			// TODO: invoke renderer
		}
	}
}
