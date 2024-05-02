using Editor.View.Wpf.Controls.Toolbar.Previews;
using Editor.ViewModel;

namespace Editor.View.Wpf.DesignData;

public class ToolBarDesignData : ToolBarViewModel
{
    public ToolBarDesignData()
    {
        Items = DefaultItems;
    }


    public static ToolBarItemViewModel[] DefaultItems =>
    [
        new ToolBarItemViewModel
        {
            Hotkey = '1',
            Preview = new DiagramNodePreview()
        },
        new ToolBarItemViewModel
        {
            Hotkey = '2',
            Preview = new ConstNodePreview()
        },
        new ToolBarItemViewModel
        {
            Hotkey = '3',
            Preview = new LabelPreview()
        }
    ];
}