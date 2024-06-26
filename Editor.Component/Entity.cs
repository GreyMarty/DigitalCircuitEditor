﻿using System.ComponentModel;
using Editor.Component.Events;
using Editor.Component.Exceptions;

namespace Editor.Component;

public interface IEntity
{
    public IContext Context { get; }
    
    public bool Initialized { get; }
    public bool Alive { get; }
    public bool Active { get; set; }
    public ComponentBase[] Components { get; init; }


    public event PropertyChangedEventHandler? ComponentChanged;
    

    public void Init(IContext context);
    public void Destroy();
    public ComponentRef<T>? GetComponent<T>() where T : ComponentBase;
    public ComponentRef<T> GetRequiredComponent<T>() where T : ComponentBase;
}

public sealed class Entity : IEntity
{
    public Entity()
    {
        Components = [];
    }

    internal Entity(IEnumerable<ComponentBase> components)
    {
        Components = components.ToArray();

        foreach (var component in Components)
        {
            component.PropertyChanged += Component_OnPropertyChanged;
        }
    }

    ~Entity()
    {
        foreach (var component in Components)
        {
            component.PropertyChanged -= Component_OnPropertyChanged;
        }
    }
    
    
    public IContext Context { get; private set; }
    
    public bool Alive { get; private set; }
    public bool Initialized { get; private set; }
    public bool Active { get; set; } = true;
    
    public ComponentBase[] Components { get; init; }


    public event PropertyChangedEventHandler? ComponentChanged;


    public static IEntityBuilder CreateBuilder() => new EntityBuilder();
        
    public void Init(IContext context)
    {
        if (Initialized)
        {
            return;
        }

        Context = context;
        
        Initialized = true;
        Alive = true;
        
        foreach (var component in Components)
        {
            component.Init(this);
        }
    }

    public void Destroy()
    {
        foreach (var component in Components)
        {
            component.Destroy();
        }
        
        Initialized = false;
        Alive = false;
    }

    public ComponentRef<T>? GetComponent<T>() where T : ComponentBase
    {
        return Components.FirstOrDefault(x => x is T) is not T component 
            ? null 
            : new ComponentRef<T>(this, component);
    }

    public ComponentRef<T> GetRequiredComponent<T>() where T : ComponentBase
    {
        return GetComponent<T>() ?? throw new ComponentRequiredException(typeof(T));
    }

    private void Component_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ComponentChanged?.Invoke(sender, e);
    }
}