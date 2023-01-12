using Azure.LogicApps.NET.Base;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET;

public class WorkflowDefinition
{
	public string Schema { get; set; } = "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#";

	public Dictionary<string, WorkflowActionBase> Actions { get; set; }

	public WorkflowTriggers Triggers { get; set; }

	public string ContentVersion { get; set; } = "1.0.0.0";

	public JsonObject Outputs { get; set; } = new JsonObject();
}
