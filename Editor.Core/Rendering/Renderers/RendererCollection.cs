﻿using Editor.Component.Events;
using Editor.Core.Collections;
using Editor.Core.Events;
using SkiaSharp;
using TinyMessenger;

namespace Editor.Core.Rendering.Renderers;

public class RendererCollection : IDisposable
{
    private readonly OrderedCollection<Renderer> _preRenderers = new(x => x.ZIndex);
    private readonly OrderedCollection<Renderer> _renderers = new(x => x.ZIndex);
    private readonly OrderedCollection<Renderer> _postRenderers = new(x => x.ZIndex);

    private bool _initialized = false;
    private ITinyMessengerHub _eventBus = default!;
    private TinyMessageSubscriptionToken? _entityInstantiatedToken;
    private TinyMessageSubscriptionToken? _entityDestroyedToken;

    private Action _uiInvoker = default!;
    
    private bool _renderRequested = false;
    private Timer? _renderCooldownTimer;


    public Action<Action> Invoker { get; init; } = x => x.Invoke();
    
    
    public void Init(EditorContext context)
    {
        _eventBus = context.EventBus;
        _entityInstantiatedToken = context.EventBus.Subscribe<EntityInstantiated>(OnEntityInstantiated);
        _entityDestroyedToken = context.EventBus.Subscribe<EntityDestroyed>(OnEntityDestroyed);
        _initialized = true;
    }
    
    public void Dispose()
    {
        if (!_initialized)
        {
            return;
        }
        
        _renderCooldownTimer?.Dispose();
        
        _eventBus.Unsubscribe<EntityInstantiated>(_entityInstantiatedToken!);
        _eventBus.Unsubscribe<EntityDestroyed>(_entityDestroyedToken!);
        _entityInstantiatedToken?.Dispose();
        _entityDestroyedToken?.Dispose();
    }

    public void Render(Camera camera, SKCanvas canvas)
    {
        if (_renderCooldownTimer is not null)
        {
            _renderRequested = true;
            return;
        }

        ActualRender(camera, canvas);
        
        _renderCooldownTimer = new Timer(_ =>
        {
            _renderCooldownTimer?.Dispose();
            _renderCooldownTimer = null;
            
            if (!_renderRequested)
            {
                return;
            }
            
            Invoker.Invoke(() => _eventBus.Publish(new RenderRequested(this)));
            _renderRequested = false;
        }, null, 33, Timeout.Infinite);
    }

    private void ActualRender(Camera camera, SKCanvas canvas)
    {
        canvas.ResetMatrix();
        canvas.Clear();
        
        canvas.Translate(camera.SizePixels.X / 2, camera.SizePixels.Y / 2);
        canvas.Scale(camera.PixelsPerUnit);
        canvas.Save();
        
        foreach (var renderer in _preRenderers)
        {
            renderer.Render(camera, canvas);
        }
        
        canvas.Translate(camera.Position.X, camera.Position.Y);
        canvas.Scale(camera.Scale);
        
        foreach (var renderer in _renderers)
        {
            renderer.Render(camera, canvas);
        }

        canvas.Restore();
        
        foreach (var renderer in _postRenderers)
        {
            renderer.Render(camera, canvas);
        }
    }
    
    private OrderedCollection<Renderer>? ResolveCollection(RenderLayer? layer)
    {
        return layer switch
        {
            RenderLayer.PreRender => _preRenderers,
            RenderLayer.Default => _renderers,
            RenderLayer.PostRender => _postRenderers,
            _ => null
        };
    }
    
    private void OnEntityInstantiated(EntityInstantiated e)
    {
        var renderer = e.Entity.GetComponent<Renderer>()?.Component;
        ResolveCollection(renderer?.Layer)?.Add(renderer!);
        
        _eventBus.Publish(new RenderRequested(this));
    }
    
    private void OnEntityDestroyed(EntityDestroyed e)
    {
        _renderRequested = false;
        
        var renderer = e.Entity.GetComponent<Renderer>()?.Component;
        ResolveCollection(renderer?.Layer)?.Remove(renderer!);
        
        _eventBus.Publish(new RenderRequested(this));
    }
}