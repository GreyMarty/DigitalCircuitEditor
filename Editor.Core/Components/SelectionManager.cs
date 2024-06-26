﻿using Editor.Component;
using Editor.Component.Events;
using Editor.Core.Events;
using Editor.Core.Input;
using Editor.Core.Prefabs;
using Editor.Core.Prefabs.Factories;
using TinyMessenger;

namespace Editor.Core.Components;

public class SelectionManager : EditorComponentBase
{
    public IEntityBuilderFactory SelectionAreaFactory { get; set; } = new SelectionAreaFactory();
    
    
    protected override void OnInit()
    {
        Events.Subscribe<MouseButtonDown>(Context_OnMouseButtonDown);
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<MouseButtonDown>();
    }

    public void UnselectAll()
    {
        foreach (var entity in Context.Entities)
        {
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;

            if (selectableComponent is null)
            {
                continue;
            }

            selectableComponent.Selected = false;
        }
    }
    
    private void Context_OnMouseButtonDown(MouseButtonDown e)
    {
        if (e.Button != MouseButton.Left || Context.MouseLocked)
        {
            return;
        }

        Selectable? clickedOn = null;
        
        foreach (var renderer in Context.RenderingManager.Renderers)
        {
            var entity = renderer.Entity;
            
            if (!entity.Active)
            {
                continue;
            }
            
            var hoverableComponent = entity.GetComponent<Hoverable>()?.Component;
            var selectableComponent = entity.GetComponent<Selectable>()?.Component;

            if (hoverableComponent?.Hovered == true && selectableComponent is null && !e.ModKeys.HasFlag(ModKeys.Shift))
            {
                UnselectAll();
                return;
            }
            
            if (hoverableComponent?.Hovered != true || selectableComponent is null)
            {
                continue;
            }
            
            clickedOn = selectableComponent;
        }

        if (clickedOn?.Selected != true && !e.ModKeys.HasFlag(ModKeys.Shift))
        {
            UnselectAll();
        }

        if (clickedOn is not null)
        {
            clickedOn.Selected = true;
            return;
        }

        var builder = SelectionAreaFactory.Create()
            .ConfigureComponent<Position>(x =>
            {
                x.Value = e.PositionConverter.ScreenToWorldSpace(e.PositionPixels);
            });
        
        Context.Instantiate(builder);
    }
}