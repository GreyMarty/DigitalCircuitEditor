namespace Editor.DecisionDiagrams.Operations;

public class Nand : IBinaryOperation
{
    public bool Compute(bool a, bool b) => !(a && b);
}