using Azure.LogicApps.NET.Base;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET.Converters;

public class WorkflowActionsDictionaryJsonConverter : JsonConverter<Dictionary<string, WorkflowActionBase>>
{
	public override Dictionary<string, WorkflowActionBase> Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters =
			{
				new WorkflowActionJsonConverter()
			}
		};

		var result = (Dictionary<string, WorkflowActionBase>)JsonSerializer.Deserialize(ref reader, typeToConvert, jsonSerializerOptions);

		foreach (KeyValuePair<string, WorkflowActionBase> kvp in result)
		{
			kvp.Value.ActionIdentifier = kvp.Key;
		}

		return result;
	}

	public override void Write(
		Utf8JsonWriter writer,
		Dictionary<string, WorkflowActionBase> objectToWrite,
		JsonSerializerOptions options)
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters =
			{
				new WorkflowActionJsonConverter()
			}
		};

		JsonSerializer.Serialize(writer, objectToWrite, jsonSerializerOptions);
	}
}
