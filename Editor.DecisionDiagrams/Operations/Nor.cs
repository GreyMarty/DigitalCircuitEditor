namespace Editor.DecisionDiagrams.Operations;

public class Nor : IBinaryOperation
{
    public bool Compute(bool a, bool b) => !(a || b);
}