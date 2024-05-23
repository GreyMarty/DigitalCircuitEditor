using System.Windows;
using Editor.ViewModel.Services;

namespace Editor.View.Wpf.Services;

public class ClipboardService : IClipboardService
{
    public void Copy(string text)
    {
        Clipboard.SetText(text);
    }

    public string? Paste()
    {
        return Clipboard.GetText();
    }
}