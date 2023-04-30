namespace ConfigurableTextFormattingHelper.Syntax
{
	internal enum ContentMode
	{
		/// <summary>
		/// Elements for a given content ID are appended to the single content with this ID.
		/// </summary>
		Append = 1,

		/// <summary>
		/// Content with a given ID can only be started once.
		/// </summary>
		Once = 2,

		/// <summary>
		/// Each time content with a given ID is started, a new content item is added.
		/// </summary>
		Multi = 3
	}
}
