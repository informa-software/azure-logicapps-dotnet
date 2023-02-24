using Azure.LogicApps.NET.Base;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET.Actions;

public class IfCondition : WorkflowActionBase
{
	public Dictionary<string, WorkflowActionBase> Actions { get; set; }

	public ElseStatement Else { get; set; }

	public JsonObject Expression { get; set; }

	public class ElseStatement
	{
		public Dictionary<string, WorkflowActionBase> Actions { get; set; }
	}
}
