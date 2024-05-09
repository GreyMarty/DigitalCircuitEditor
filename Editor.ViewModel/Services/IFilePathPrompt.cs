namespace Editor.ViewModel.Services;

public interface IFilePathPrompt
{
    public string? GetSaveFilePath();
    public string? GetOpenFilePath();
}