namespace ConfigurableTextFormattingHelper.Defs
{
	internal sealed class DefinedContext
	{
		public DefinedContext()
		{
		}

		private readonly DefinedContext? parentContext;

		public TokenInfo? RecognizeToken(string str, int charIndex)
		{
			var recognizedTokens = new List<TokenInfo>();
			var indexInToken = 0;
			var candidates = tokens.ToList();
			while (candidates.Count > 0)
			{
				var ch = str[charIndex + indexInToken];
				for (var i = candidates.Count - 1; i >= 0; i--)
				{
					var candidate = candidates[i];
					if (candidate.Token[indexInToken] == ch)
					{
						if (indexInToken == candidate.Token.Length - 1)
						{
							recognizedTokens.Add(candidate.Info);
							candidates.RemoveAt(i);
						}
					}
					else
					{
						candidates.RemoveAt(i);
					}
				}

				indexInToken++;
			}

			return candidates.Count > 0 ? candidates[0].Info : null;
		}

		private readonly List<(string Token, TokenInfo Info)> tokens = new();
	}
}
