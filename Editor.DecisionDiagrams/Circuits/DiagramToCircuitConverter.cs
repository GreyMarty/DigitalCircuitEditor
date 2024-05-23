using Editor.DecisionDiagrams.Circuits.Gates;

namespace Editor.DecisionDiagrams.Circuits;

public static class DiagramToCircuitConverter
{
    public static ICircuitElement ToCircuit(this INode root, bool reuseInputs = false)
    {
        var id = 0;
        return root.ToCircuit([], reuseInputs, [], ref id);
    }

    private static ICircuitElement ToCircuit(this INode root, Dictionary<int, ICircuitElement> cache, bool reuseInputs, Dictionary<int, Input> inputs, ref int id)
    {
        if (cache.TryGetValue(root.Id, out var element) && (element is not Input || reuseInputs))
        {
            return element;
        }
        
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

        var input = default(Input);
        var inverted = false;
        
        if (terminalTrue?.Value == false || terminalFalse?.Value == true)
        {
            if (!reuseInputs || !inputs.TryGetValue(-branchNode.VariableId, out input))
            {
                input = new Input(id++, branchNode.VariableId, true);
                inputs[-input.InputId] = input;
            }

            inverted = true;
        }
        else if (!reuseInputs || !inputs.TryGetValue(branchNode.VariableId, out input))
        {
            input = new Input(id++, branchNode.VariableId);
            inputs[input.InputId] = input;
        }

        element = (terminalTrue?.Value, terminalFalse?.Value, inverted) switch
        {
            (not null, not null, _) => input,
            (true, _, false) => new OrGate(id++, input, branchNode.False.ToCircuit(cache, reuseInputs, inputs, ref id)),
            (_, false, false) => new AndGate(id++, input, branchNode.True.ToCircuit(cache, reuseInputs, inputs, ref id)),
            (_, true, true) => new OrGate(id++, input, branchNode.True.ToCircuit(cache, reuseInputs, inputs, ref id)),
            (false, _, true) => new AndGate(id++, input, branchNode.False.ToCircuit(cache, reuseInputs, inputs, ref id)),
            _ => new MuxGate(
                id++,
                branchNode.False.ToCircuit(cache, reuseInputs, inputs, ref id),
                branchNode.True.ToCircuit(cache, reuseInputs, inputs, ref id),
                input
            )
        };

        cache[root.Id] = element;
        return element;
    }
}