namespace Editor.DecisionDiagrams.Operations;

public interface IBinaryOperation : IOperation
{
    public bool Compute(bool a, bool b);
}