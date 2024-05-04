using Editor.DecisionDiagrams.Circuits.Gates;

namespace Editor.DecisionDiagrams.Circuits;

public static class DiagramToCircuitConverter
{
    public static ICircuitElement ToCircuit(this INode root)
    {
        var id = 0;
        return root.ToCircuit([], [], ref id);
    }

    private static ICircuitElement ToCircuit(this INode root, Dictionary<int, ICircuitElement> cache, Dictionary<int, Input> inputs, ref int id)
    {
        // Constant
        if (root is TerminalNode terminalNode)
        {
            var localElement = new Constant(id++, terminalNode.Value);
            cache[root.Id] = localElement;
            return localElement;
        }

        var branchNode = (BranchNode)root;
        var terminalTrue = branchNode.True as TerminalNode;
        var terminalFalse = branchNode.False as TerminalNode;

        if (!inputs.TryGetValue(branchNode.VariableId, out var input))
        {
            input = new Input(id++, branchNode.VariableId);
            inputs[input.InputId] = input;
        }

        ICircuitElement element = (terminalTrue?.Value, terminalFalse?.Value) switch
        {
            (true, false) => input,
            (false, true) => new NotGate(id++, input),
            (true, _) => new OrGate(id++, input, branchNode.False.ToCircuit(cache, inputs, ref id)),
            (_, false) => new AndGate(id++, branchNode.True.ToCircuit(cache, inputs, ref id), input),
            _ => new MuxGate(
                id++,
                branchNode.False.ToCircuit(cache, inputs, ref id),
                branchNode.True.ToCircuit(cache, inputs, ref id),
                input
            )
        };

        cache[root.Id] = element;
        return element;
    }
}