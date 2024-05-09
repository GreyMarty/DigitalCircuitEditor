namespace Editor.ViewModel.Services;

public interface IFilePathPrompt
{
    public string? GetSaveFilePath(string? defaultPath = null);
    public string? GetOpenFilePath();
}