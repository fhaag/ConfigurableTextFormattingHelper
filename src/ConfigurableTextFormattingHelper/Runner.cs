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

			var mgr = new ProcessingManager();

			var cfgRendererFactory = PrepareRenderer(rendererFactory);
			cfgRendererFactory.LoadSettings(args);
			var renderer = cfgRendererFactory.RendererFactory.CreateRenderer();
			try
			{
				Execute(mgr, cfgRendererFactory.Settings, renderer);
			}
			finally
			{
				(renderer as IDisposable)?.Dispose();
			}
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

		private void Execute(ProcessingManager mgr, CliSettings settings, IRenderer renderer)
		{
			if (string.IsNullOrEmpty(settings.ProjectPath))
			{
				throw new InvalidOperationException("No project path specified.");
			}

			var pjLoader = new ProjectLoader();
			mgr.Project = pjLoader.LoadFromFile(settings.ProjectPath);
			mgr.ProjectPath = settings.ProjectPath;
			Execute(mgr, renderer);
		}

		private void Execute(ProcessingManager mgr, IRenderer renderer)
		{
			ArgumentNullException.ThrowIfNull(mgr);
			ArgumentNullException.ThrowIfNull(renderer);

			if (mgr.Project == null)
			{
				throw new InvalidOperationException("No project loaded.");
			}

			// syntax
			var syntax = new Syntax.SyntaxDef();
			if (mgr.Project.Syntax != null)
			{
				var syntaxLoader = new Syntax.SyntaxLoader();

				foreach (var syntaxPath in mgr.Project.Syntax)
				{
					var effectivePath = mgr.FindFile(syntaxPath, "syntax/" + syntaxPath, syntaxPath + Constants.SyntaxExtension, "syntax/" + syntaxPath + Constants.SyntaxExtension);
					if (effectivePath == null)
					{
						throw new InvalidOperationException($"Syntax file not found: {syntaxPath}");
					}

					var partialSyntax = syntaxLoader.LoadFromFile(effectivePath);
					syntax.Append(partialSyntax);
				}
			}

			// semantics
			var semantics = new Semantics.SemanticsDef();
			if (mgr.Project.Semantics != null)
			{
				var semanticsLoader = new Semantics.SemanticsLoader(new(mgr));

				foreach (var semanticsPath in mgr.Project.Semantics)
				{
					var effectivePath = mgr.FindFile(semanticsPath, "semantics/" + semanticsPath, semanticsPath + Constants.SemanticsExtension, "semantics/" + semanticsPath + Constants.SemanticsExtension);
					if (effectivePath == null)
					{
						throw new InvalidOperationException($"Semantics file not found: {semanticsPath}");
					}

					var partialSemantics = semanticsLoader.LoadFromFile(effectivePath);
					semantics.Append(partialSemantics);
				}
			}

			var parser = new Syntax.Parser(mgr);
			var semProc = new Semantics.SemanticsProcessor(mgr);

			var finalSpans = new List<Documents.Span>();

			if (mgr.Project.Sources != null)
			{
				// TODO: parallelize this?
				foreach (var sourceFile in mgr.Project.Sources)
				{
					var effectivePath = mgr.FindFile(sourceFile, sourceFile + Constants.SourceExtension);
					if (effectivePath == null)
					{
						throw new InvalidOperationException($"Content file not found: {sourceFile}");
					}

					var source = File.ReadAllText(effectivePath);

					var rawSpan = parser.Parse(syntax, source);
					var span = semProc.Process(semantics, rawSpan);

					finalSpans.Add(span);
				}
			}

			var renderingController = new Rendering.RenderingController(renderer);
			renderingController.Render(finalSpans.ToArray());
		}
	}
}
