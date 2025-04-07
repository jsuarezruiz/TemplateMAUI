#nullable disable
using System.Web;

namespace TemplateMAUI.CodeEditor
{
    public class CodeEditor : TemplatedView, ICodeEditor
    {
        const string ElementMonaco = "PART_Monaco";

        WebView _monacoEditor;

        bool _loaded;

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CodeEditor), null);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                OnPropertyChanged();

                _ = SetTextAsync(value);
            }
        }

        public static readonly BindableProperty LanguageProperty =
            BindableProperty.Create(nameof(Language), typeof(string), typeof(CodeEditor), null);

        public string Language
        {
            get
            {
                object editorLanguageProperty = GetValue(LanguageProperty);
                return editorLanguageProperty == null ? "csharp" : (string)editorLanguageProperty;
            }
            set
            {
                SetValue(LanguageProperty, value);
                OnPropertyChanged();

                _ = SetLanguageAsync(value);
            }
        }

        public static readonly BindableProperty ThemeProperty =
            BindableProperty.Create(nameof(Theme), typeof(CodeEditorTheme), typeof(CodeEditor), null);

        public CodeEditorTheme Theme
        {
            get
            {
                object editorThemeProperty = GetValue(ThemeProperty);
                return editorThemeProperty == null ? CodeEditorTheme.VisualStudioLight : (CodeEditorTheme)GetValue(ThemeProperty);
            }
            set
            {
                SetValue(ThemeProperty, value);
                OnPropertyChanged();

                _ = SetThemeAsync(value);
            }
        }

        public static readonly BindableProperty IsMiniMapVisibleProperty =
            BindableProperty.Create(nameof(IsMiniMapVisible), typeof(bool), typeof(CodeEditor), false);

        public bool IsMiniMapVisible
        {
            get => (bool)GetValue(IsMiniMapVisibleProperty);
            set
            {
                SetValue(IsMiniMapVisibleProperty, value);
                OnPropertyChanged();

                SetIsMiniMapVisible(value);
            }
        }

        public static readonly BindableProperty IsReadOnlyProperty =
            BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(CodeEditor), false);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set
            {
                SetValue(IsReadOnlyProperty, value);
                OnPropertyChanged();

                SetIsReadOnly(value);
            }
        }

        public static readonly BindableProperty IsContextMenuEnabledProperty =
             BindableProperty.Create(nameof(IsContextMenuEnabled), typeof(bool), typeof(CodeEditor), null);

        public bool IsContextMenuEnabled
        {
            get
            {
                object editorContextMenuEnabled = GetValue(IsContextMenuEnabledProperty);
                return IsContextMenuEnabledProperty == null ? false : (bool)editorContextMenuEnabled;
            }
            set
            {
                SetValue(IsContextMenuEnabledProperty, value);
                OnPropertyChanged();

                SetIsContextMenuEnabled(value);
            }
        }

        public delegate void LoadedEventHandler(object sender, object args);
        public new event LoadedEventHandler Loaded;

        protected override void OnApplyTemplate()
        {
            if(_monacoEditor is not null)
            {
                _monacoEditor.Loaded -= OnMonacoEditorLoaded;
                _monacoEditor.Navigated -= OnMonacoEditorWebViewNavigated;
            }

            base.OnApplyTemplate();

            _monacoEditor = GetTemplateChild(ElementMonaco) as WebView;

            _monacoEditor.Loaded += OnMonacoEditorLoaded;
            _monacoEditor.Navigated += OnMonacoEditorWebViewNavigated;
        }

        public void Dispose()
        {
            if (_monacoEditor is not null)
            {
                _monacoEditor.Loaded -= OnMonacoEditorLoaded;
                _monacoEditor.Navigated -= OnMonacoEditorWebViewNavigated;
            }
        }

        public async Task SetTextAsync(string text)
        {
            string ensuredText = HttpUtility.JavaScriptStringEncode(text);

            string command = $"editor.setValue('{ensuredText}');";

            await _monacoEditor.EvaluateJavaScriptAsync(command);
        }

        public async Task SetLanguageAsync(string language)
        {
            string command = $"editor.setModel(monaco.editor.createModel(editor.getValue(), '{language}'));";

            await _monacoEditor.EvaluateJavaScriptAsync(command);
        }

        public async Task SetThemeAsync(CodeEditorTheme theme)
        {
            string themeValue = "vs-dark";

            switch (theme)
            {
                case CodeEditorTheme.VisualStudioLight:
                    {
                        themeValue = "vs-light";
                    }
                    break;
                case CodeEditorTheme.VisualStudioDark:
                    {
                        themeValue = "vs-dark";
                    }
                    break;
                case CodeEditorTheme.HighContrastDark:
                    {
                        themeValue = "hc-black";
                    }
                    break;
            }

            await SetThemeAsync(themeValue);
        }

        public async Task SetThemeAsync(string theme)
        {
            string command = $"editor._themeService.setTheme('{theme}');";

            await _monacoEditor.EvaluateJavaScriptAsync(command);
        }

        public void SetIsMiniMapVisible(bool isMiniMapVisible = true)
        {
            if (isMiniMapVisible)
            {
                _ = _monacoEditor.EvaluateJavaScriptAsync($"editor.updateOptions({{ minimap: {{ enabled: true }} }});");
            }
            else
            {
                _ = _monacoEditor.EvaluateJavaScriptAsync($"editor.updateOptions({{ minimap: {{ enabled: false }} }});");
            }
        }

        public void SetIsReadOnly(bool isReadOnly = false)
        {
            if (isReadOnly)
            {
                _ = _monacoEditor.EvaluateJavaScriptAsync($"editor.updateOptions({{readOnly: true}});");
            }
            else
            {
                _ = _monacoEditor.EvaluateJavaScriptAsync($"editor.updateOptions({{readOnly: false}});");
            }
        }

        public Task SetIsContextMenuEnabled(bool status = true)
        {
            string command = string.Empty;

            if (status)
                command = $"editor.updateOptions({{ contextmenu: true }});";
            else
                command = $"editor.updateOptions({{ contextmenu: false }});";

            return _monacoEditor.EvaluateJavaScriptAsync(command);
        }

        public void ScrollTo(int lineNumber)
        {
            string command = $"editor.revealLine({lineNumber}); editor.setPosition({{lineNumber: {lineNumber}, column: 0 }});";
            _monacoEditor.EvaluateJavaScriptAsync(command);
        }

        void OnMonacoEditorLoaded(object sender, object args)
        {
            string path = "file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html").Replace("\\", "/");
            _monacoEditor.Source = new Uri(path);

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        void OnMonacoEditorWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (!_loaded)
            {
                _ = SetThemeAsync(Theme);
                _ = SetLanguageAsync(Language);
                _ = SetTextAsync(Text);

                _loaded = true;
            }
        }
    }
}