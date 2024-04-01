using System.Numerics;
using Editor.Component;
using Editor.Core.Components;
using Editor.Core.Components.Triggers;
using Editor.Core.Wpf.Views;

namespace Editor.Core.Wpf.Prefabs;

public static class IfDiagramNodePrefab
{
    public static void Instantiate(EditorWorld world)
    {
        var root = Entity.CreateBuilder()
            .AddComponent<Position>()
            .AddComponent<Hoverable>()
            .AddComponent<Selectable>()
            .AddComponent<SelectOnClickTrigger>()
            .AddComponent<DragOnMouseMoveTrigger>()
            .AddComponent<ViewComponent<DiagramNode>>();

        var rootEntity = world.Instantiate(root);

        for (var i = 0; i < 3; i++)
        {
            var x = -2 + 2 * i;
            
            var spawner = Entity.CreateBuilder()
                .AddComponent(new Position { Local = new Vector2(3 * i - 3, 3)})
                .AddComponent(new ChildOf { Parent = rootEntity })
                .AddComponent<Hoverable>()
                .AddComponent<ViewComponent<GhostDiagramNode>>();

            var spawnerEntity = world.Instantiate(spawner);
            
            var connection = Entity.CreateBuilder()
                .AddComponent<Position>()
                .AddComponent(new ChildOf { Parent = rootEntity })
                .AddComponent(new IfDiagramConnection
                {
                    Target = spawnerEntity,
                    Type = i switch
                    {
                        0 => IfDiagramConnectionType.If,
                        1 => IfDiagramConnectionType.True,
                        2 => IfDiagramConnectionType.False,
                    }
                })
                .AddComponent<ViewComponent<ConnectionLine>>();

            world.Instantiate(connection);
        }
    }
}