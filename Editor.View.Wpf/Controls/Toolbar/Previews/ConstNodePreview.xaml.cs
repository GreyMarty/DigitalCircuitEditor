using System.Windows.Controls;
using Editor.ViewModel;

namespace Editor.View.Wpf.Controls.Toolbar.Previews;

public partial class ConstNodePreview : UserControl, IPreview
{
    public string Label { get; } 
    
    
    public ConstNodePreview(bool value = false)
    {
        Label = value ? "1" : "0";
        DataContext = this;
        InitializeComponent();
    }
}