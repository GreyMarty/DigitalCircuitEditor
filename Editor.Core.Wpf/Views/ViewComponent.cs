using Editor.Component;
using Editor.Core.Components;
using Editor.Core.ViewModels;

namespace Editor.Core.Wpf.Views;

public interface IViewComponent 
{
    public EditorElement View { get; }
}

public interface IViewComponent<out T> : IViewComponent where T : EditorElement
{
    public new T View { get; }
    EditorElement IViewComponent.View => View;
}

public sealed class ViewComponent<T> : EditorComponentBase, IViewComponent<T> where T : EditorElement, new()
{
    public T View { get; } = new();

    
    public override void Init(EditorWorld world, IEntity entity)
    {
        var viewModel = View.DataContext as EditorElementViewModel;
        viewModel?.Init(world, entity);
    }

    public override void Dispose()
    {
        var viewModel = View.DataContext as EditorElementViewModel;
        viewModel?.Dispose();
        
        base.Dispose();
    }
}