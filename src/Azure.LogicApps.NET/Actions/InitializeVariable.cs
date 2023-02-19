using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET.Actions;

public class InitializeVariable : WorkflowActionBase
{
	public override string Type => ActionType.InitializeVariable;

	public Input Inputs { get; set; }

	public class Input
	{
		public List<Variable> Variables { get; set; }
	}

	public class Variable
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public object Value { get; set; }
	}
}