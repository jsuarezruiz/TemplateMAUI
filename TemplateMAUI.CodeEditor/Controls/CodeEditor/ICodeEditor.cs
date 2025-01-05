namespace TemplateMAUI.CodeEditor
{
    public interface ICodeEditor : IDisposable
    {
        Task SetTextAsync(string text);

        Task SetLanguageAsync(string languageId);

        Task SetThemeAsync(CodeEditorTheme theme);

        Task SetThemeAsync(string theme);

        void SetIsMiniMapVisible(bool isMiniMapVisible);

        void SetIsReadOnly(bool status);

        Task SetIsContextMenuEnabled(bool status = true);

        void ScrollTo(int lineNumber);
    }
}