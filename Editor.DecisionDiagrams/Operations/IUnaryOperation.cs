namespace Editor.DecisionDiagrams.Operations;

public interface IUnaryOperation : IOperation
{
    public bool Compute(bool a);
}