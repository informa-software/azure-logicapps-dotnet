using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Actions;

public class Until : WorkflowActionBase
{
	public Dictionary<string, WorkflowActionBase> Actions { get; set; }

	public string Expression { get; set; }

	public UntilLimit Limit { get; set; }

	public class UntilLimit
	{
		public int Count { get; set; }

		public string Timeout { get; set; }
	}
}