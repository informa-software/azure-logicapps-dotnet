using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Triggers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Azure.LogicApps.NET.Converters;

public class WorkflowTriggerJsonConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		if (typeof(WorkflowTriggerBase).IsAssignableFrom(typeToConvert))
		{
			return true;
		}

		return false;
	}

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver
			{
				Modifiers =
					{
						static jsonTypeInfo =>
						{
							if (jsonTypeInfo.Type != typeof(WorkflowTriggerBase))
							{
								return;
							}

							jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
							{
								TypeDiscriminatorPropertyName = "type",
								IgnoreUnrecognizedTypeDiscriminators = true,
								UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
								DerivedTypes =
								{
									new JsonDerivedType(typeof(RequestTrigger), "Request"),
									new JsonDerivedType(typeof(RecurrenceTrigger), "Recurrence")
								}
							};
						}
					}
			}
		};

		return new WorkflowTriggerJsonConverterInternal(jsonSerializerOptions);
	}

	private class WorkflowTriggerJsonConverterInternal : JsonConverter<WorkflowTriggerBase>
	{
		private readonly JsonSerializerOptions _options;

		public WorkflowTriggerJsonConverterInternal(JsonSerializerOptions options)
		{
			_options = options;
		}

		public override WorkflowTriggerBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var result = (WorkflowTriggerBase)JsonSerializer.Deserialize(ref reader, typeToConvert, _options);

			if (result is RequestTrigger requestTrigger)
			{
				if (requestTrigger.ExtensionData?.Any(x => x.Key == "operationOptions" && x.Value?.ToString() == "EnableSchemaValidation") == true)
				{
					requestTrigger.EnableSchemaValidation = true;
					return requestTrigger;
				}
			}

			return result;
		}

		public override void Write(Utf8JsonWriter writer, WorkflowTriggerBase value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, _options);
		}
	}
}
