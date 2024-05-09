namespace Editor.DecisionDiagrams.Operations;

public class Not : IUnaryOperation
{
    public bool Compute(bool a) => !a;
}