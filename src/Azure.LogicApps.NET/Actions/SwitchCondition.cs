using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Actions;

public class SwitchCondition : WorkflowActionBase
{
	public string Expression { get; set; }

	public SwitchDefaultCaseStatement Default { get; set; }

	public Dictionary<string, SwitchCaseStatement> Cases { get; set; }

	public class SwitchDefaultCaseStatement
	{
		public Dictionary<string, WorkflowActionBase> Actions { get; set; }
	}

	public class SwitchCaseStatement : SwitchDefaultCaseStatement
	{
		public string Case { get; set; }
	}
}