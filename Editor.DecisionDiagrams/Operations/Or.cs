namespace Editor.DecisionDiagrams.Operations;

public class Or : IBooleanOperation
{
    public bool Compute(bool a, bool b) => a || b;
}