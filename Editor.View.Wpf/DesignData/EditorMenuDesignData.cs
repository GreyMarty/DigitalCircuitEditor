using Editor.ViewModel;

namespace Editor.View.Wpf.DesignData;

public class EditorMenuDesignData : EditorMenuViewModel
{
    public EditorMenuDesignData()
    {
        Operations =
        [
            new OperationViewModel()
            {
                Label = "AND"
            },
            new OperationViewModel()
            {
                Label = "OR"
            },
        ];
    }
}