using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Actions;

public class SetVariable : WorkflowActionBase
{
	public Variable Inputs { get; set; }

	public class Variable
	{
		public string Name { get; set; }

		public object Value { get; set; }
	}
}
