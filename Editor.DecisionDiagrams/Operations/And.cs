namespace Editor.DecisionDiagrams.Operations;

public class And : IBooleanOperation
{
    public bool Compute(bool a, bool b) => a && b;
}