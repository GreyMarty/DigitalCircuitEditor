using Editor.ViewModel;

namespace Editor.View.Wpf.DesignData;

public class MainWindowDesignData : MainViewModel
{
    public MainWindowDesignData()
    {
        ToolBar.Items = ToolBarDesignData.DefaultItems;
    }
}