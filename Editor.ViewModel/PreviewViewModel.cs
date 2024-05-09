using Editor.Core.Prefabs.Factories;
using Editor.Core.Prefabs.Spawners;
using Editor.Core.Prefabs.Spawners.Circuits;
using Editor.DecisionDiagrams.Circuits;

namespace Editor.ViewModel;

public class PreviewViewModel : ViewModel
{
    public EditorViewModel Editor { get; set; } = new();


    public void SpawnCircuit(ICircuitElement circuit)
    {
        var spawner = Editor.Context!.Instantiate(new InstantSpawnerFactory<CircuitSpawner>()
            .Create()
            .ConfigureComponent<CircuitSpawner>(x =>
            {
                x.Root = circuit;
            })
        );

        spawner.GetRequiredComponent<Spawner>().Component!.Spawn();
    }
}