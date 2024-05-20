namespace Editor.DecisionDiagrams.Operations;

public class Xnor : IBinaryOperation
{
    public bool Compute(bool a, bool b) => !(a ^ b);
}