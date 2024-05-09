namespace Editor.DecisionDiagrams.Operations;

public class Xor : IBinaryOperation
{
    public bool Compute(bool a, bool b) => a ^ b;
}