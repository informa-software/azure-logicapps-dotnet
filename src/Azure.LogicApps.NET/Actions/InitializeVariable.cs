using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Actions;

public class InitializeVariable : WorkflowActionBase
{
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