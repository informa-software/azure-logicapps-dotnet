using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET.Actions;

public class IfCondition : WorkflowActionBase
{
	public override string Type => ActionType.If;

	public Dictionary<string, WorkflowActionBase> Actions { get; set; }

	public ElseStatement Else { get; set; }

	public ConditionExpression Expression { get; set; }

	public class ElseStatement
	{
		public Dictionary<string, WorkflowActionBase> Actions { get; set; }
	}

	public class ConditionExpression : Dictionary<string, List<Dictionary<string, List<string>>>>
	{
	}
}