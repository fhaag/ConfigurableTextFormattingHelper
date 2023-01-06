namespace ConfigurableTextFormattingHelper.Infrastructure
{
	internal record InputOrigin(string FilePath, int Line, int Column, InputOrigin? LoadedFrom = null);
}
