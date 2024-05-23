namespace Editor.ViewModel.Services;

public interface IClipboardService
{
    public void Copy(string text);
    public string? Paste();
}