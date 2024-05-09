namespace Editor.DecisionDiagrams.Operations;

public class Or : IBinaryOperation
{
    public bool Compute(bool a, bool b) => a || b;
}