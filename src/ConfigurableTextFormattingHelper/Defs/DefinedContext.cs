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
