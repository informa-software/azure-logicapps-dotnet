using Azure.LogicApps.NET.Converters;
using System.Text.Json;

namespace Azure.LogicApps.NET.Base;

public abstract class WorkflowTriggerBase : IWorkflowItem<WorkflowTriggerBase>
{
	public static WorkflowTriggerBase FromWorkflowJsonString(string jsonString)
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			Converters =
			{
				new WorkflowTriggerJsonConverter()
			}
		};

		return JsonSerializer.Deserialize<WorkflowTriggerBase>(jsonString, jsonSerializerOptions);
	}

	public virtual string ToWorkflowJsonString()
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters =
			{
				new WorkflowTriggerJsonConverter()
			}
		};

		return JsonSerializer.Serialize(this, jsonSerializerOptions);
	}
}
