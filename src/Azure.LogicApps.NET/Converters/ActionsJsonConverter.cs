using Azure.LogicApps.NET.Base;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Azure.LogicApps.NET.Converters;

public class ActionsJsonConverter : JsonConverter<Dictionary<string, WorkflowActionBase>>
{
	public override Dictionary<string, WorkflowActionBase> Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
	{
		var result = (Dictionary<string, WorkflowActionBase>)JsonSerializer.Deserialize(ref reader, typeToConvert, options);

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
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver
			{
				Modifiers =
				{
					static typeInfo =>
					{
						if (typeInfo.Kind != JsonTypeInfoKind.Object)
						{
							return;
						}

						foreach (JsonPropertyInfo propertyInfo in typeInfo.Properties)
						{
							propertyInfo.ShouldSerialize = (parent, value) => propertyInfo switch
							{
								{ Name: "actionIdentifier" }  => false,
								{ Name: "type" } => ShouldTypeBeSerialized(parent),
								_ => true
							};
						}
					}
				}
			}
		};

		JsonSerializer.Serialize(writer, objectToWrite, jsonSerializerOptions);
	}

	private static bool ShouldTypeBeSerialized(object obj)
	{
		if (typeof(WorkflowActionBase).IsAssignableFrom(obj.GetType()))
		{
			return false;
		}

		return true;
	}
}
