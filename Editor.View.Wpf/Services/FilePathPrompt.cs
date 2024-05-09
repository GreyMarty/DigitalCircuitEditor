using System.IO;
using Editor.ViewModel.Services;
using Microsoft.Win32;

namespace Editor.View.Wpf.Services;

public class FilePathPrompt : IFilePathPrompt
{
    public string? GetSaveFilePath(string? defaultPath = null)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "YAML Files (*.yaml;*.yml)|*.yaml;*.yml|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            AddExtension = true,
            DefaultExt = ".yml",
        };

        if (!string.IsNullOrEmpty(defaultPath))
        {
            dialog.DefaultDirectory = Path.GetDirectoryName(defaultPath);
            dialog.FileName = Path.GetFileName(defaultPath);
        }
        
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? GetOpenFilePath()
    {
        var dialog = new OpenFileDialog()
        {
            Filter = "YAML Files (*.yaml;*.yml)|*.yaml;*.yml|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}