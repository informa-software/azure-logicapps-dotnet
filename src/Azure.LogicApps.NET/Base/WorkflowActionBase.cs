using Azure.LogicApps.NET.Converters;
using System.Text.Json;

namespace Azure.LogicApps.NET.Base;

public abstract class WorkflowActionBase : IWorkflowItem<WorkflowActionBase>
{
	public required string ActionIdentifier { get; set; }

	public Dictionary<string, List<string>> RunAfter { get; set; } = new Dictionary<string, List<string>>();

	public static WorkflowActionBase FromWorkflowJsonString(string jsonString)
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters =
			{
				new WorkflowActionJsonConverter()
			}
		};

		return JsonSerializer.Deserialize<WorkflowActionBase>(jsonString, jsonSerializerOptions);
	}

	public virtual string ToWorkflowJsonString()
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters =
			{
				new WorkflowActionJsonConverter()
			}
		};

		return JsonSerializer.Serialize(this, jsonSerializerOptions);
	}
}
