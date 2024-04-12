using Editor.Component;
using Editor.Core.Behaviors.Triggers.Args;
using TinyMessenger;

namespace Editor.Core.Behaviors.Triggers;

public class EventTrigger<TEvent> : TriggerBase<EditorContext, EventTriggerArgs<TEvent>> 
    where TEvent : class, ITinyMessage
{
    protected override void OnInit()
    {
        Events.Subscribe<TEvent>(x => OnFired(new EventTriggerArgs<TEvent>(x)));
    }

    protected override void OnDestroy()
    {
        Events.Unsubscribe<TEvent>();
    }
}