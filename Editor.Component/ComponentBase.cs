using System.ComponentModel;
using System.Runtime.CompilerServices;
using Editor.Component.Events;

namespace Editor.Component;

public abstract class ComponentBase : INotifyPropertyChanged
{
    public IEntity Entity { get; private set; } = default!;
    public IEventBusSubscriber Events { get; private set; } = default!;

    public IContext Context => Entity.Context;
    
    public bool Initialized { get; private set; }

    
    public event PropertyChangedEventHandler? PropertyChanged;
    

    internal void Init(IEntity entity)
    {
        if (Initialized)
        {
            return;
        }

        Entity = entity;
        Events = Context.EventBus.Subscribe(() => Entity is { Initialized: true, Alive: true, Active: true });
        
        OnInit();
        Initialized = true;
    }

    internal void Destroy()
    {
        if (!Initialized)
        {
            return;
        }
        
        OnDestroy();
        Initialized = false;
    }
    
    protected virtual void OnInit() { }
    protected virtual void OnDestroy() { }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public abstract class ComponentBase<TContext> : ComponentBase 
    where TContext : IContext
{
    public new TContext Context => (TContext)base.Context;
}