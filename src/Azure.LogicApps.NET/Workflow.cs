using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET;

public class Workflow : IWorkflowItem<Workflow>
{
	public WorkflowDefinition Definition { get; set; }

	public WorkflowKind Kind { get; set; }

	public static Workflow FromWorkflowJsonString(string jsonString)
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true,
			Converters =
			{
				new JsonStringEnumConverter(),
				new WorkflowTriggerJsonConverter(),
				new WorkflowActionsDictionaryJsonConverter()
			}
		};

		return JsonSerializer.Deserialize<Workflow>(jsonString, jsonSerializerOptions);
	}

	public virtual string ToWorkflowJsonString()
	{
		JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters =
			{
				new JsonStringEnumConverter(),
				new WorkflowTriggerJsonConverter(),
				new WorkflowActionsDictionaryJsonConverter()
			},
		};

		return JsonSerializer.Serialize(this, jsonSerializerOptions);
	}
}
