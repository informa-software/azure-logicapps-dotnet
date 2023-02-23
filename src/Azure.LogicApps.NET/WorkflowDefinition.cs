using Azure.LogicApps.NET.Base;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET;

public class WorkflowDefinition
{
	[JsonPropertyName("$schema")]
	public string Schema { get; set; } = "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#";

	public Dictionary<string, WorkflowActionBase> Actions { get; set; }

	public Dictionary<string, WorkflowTriggerBase> Triggers { get; set; }

	public string ContentVersion { get; set; } = "1.0.0.0";

	public JsonObject Outputs { get; set; }
}
