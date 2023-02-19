using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET.Actions;

public class SetVariable : WorkflowActionBase
{
	public override string Type => ActionType.SetVariable;

	public Variable Inputs { get; set; }

	public class Variable
	{
		public string Name { get; set; }

		public object Value { get; set; }
	}
}
