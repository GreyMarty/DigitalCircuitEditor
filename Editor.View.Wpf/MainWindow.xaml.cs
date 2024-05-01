using Editor.View.Wpf.DesignData;
using MahApps.Metro.Controls;

namespace Editor.View.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        ToolBar.DataContext = new ToolBarDesignData();
    }
}