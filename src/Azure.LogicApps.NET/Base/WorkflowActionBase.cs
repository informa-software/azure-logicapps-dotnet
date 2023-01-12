using Azure.LogicApps.NET.Actions;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET.Base;

[JsonDerivedType(typeof(InitializeVariable))]
[JsonDerivedType(typeof(SetVariable))]
public abstract class WorkflowActionBase
{
	public required string ActionIdentifier { get; set; }

	public RunAfter RunAfter { get; set; }

	public abstract JsonNode ToWorkflowTemplate();

	public string ToWorkflowTemplateJsonString()
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		return ToWorkflowTemplateJsonString(jsonSerializerOptions);
	}

	public string ToWorkflowTemplateJsonString(JsonSerializerOptions jsonSerializerOptions)
	{
		JsonNode workflowTemplate = ToWorkflowTemplate();
		string jsonString = JsonSerializer.Serialize(workflowTemplate, jsonSerializerOptions);
		return jsonString;
	}
}
