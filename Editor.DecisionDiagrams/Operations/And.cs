namespace Editor.DecisionDiagrams.Operations;

public class And : IBinaryOperation
{
    public bool Compute(bool a, bool b) => a && b;
}