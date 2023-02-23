using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Azure.LogicApps.NET.Converters;

public class WorkflowActionJsonConverter : JsonConverterFactory
{
	public override bool CanConvert(Type typeToConvert)
	{
		if (typeof(WorkflowActionBase).IsAssignableFrom(typeToConvert))
		{
			return true;
		}

		return false;
	}

	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver
			{
				Modifiers =
					{
						static jsonTypeInfo =>
						{
							const string actionIdentifierPropertyName = "actionIdentifier";

							foreach (JsonPropertyInfo propertyInfo in jsonTypeInfo.Properties)
							{
								propertyInfo.ShouldSerialize = (parent, value) => propertyInfo switch
								{
									{ Name: actionIdentifierPropertyName }  => false,
									_ => true
								};

								if(propertyInfo.Name == actionIdentifierPropertyName)
								{
									propertyInfo.IsRequired = false;
							}
							}

							if (jsonTypeInfo.Type != typeof(WorkflowActionBase))
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
									new JsonDerivedType(typeof(InitializeVariable), ActionType.InitializeVariable),
									new JsonDerivedType(typeof(SetVariable), ActionType.SetVariable),
									new JsonDerivedType(typeof(IfCondition), ActionType.If),
									new JsonDerivedType(typeof(SwitchCondition), ActionType.Switch),
									new JsonDerivedType(typeof(Until), ActionType.Until),
									new JsonDerivedType(typeof(Http), ActionType.Http),
									new JsonDerivedType(typeof(HttpWebhook), ActionType.HttpWebhook),
								}
							};
						}
					}
			},
			Converters =
			{
				new WorkflowActionsDictionaryJsonConverter()
			}
		};

		return new WorkflowActionJsonConverterInternal(jsonSerializerOptions);
	}

	private class WorkflowActionJsonConverterInternal : JsonConverter<WorkflowActionBase>
	{
		private readonly JsonSerializerOptions _options;

		public WorkflowActionJsonConverterInternal(JsonSerializerOptions options)
		{
			_options = options;
		}

		public override WorkflowActionBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var result = (WorkflowActionBase)JsonSerializer.Deserialize(ref reader, typeToConvert, _options);

			return result;
		}

		public override void Write(Utf8JsonWriter writer, WorkflowActionBase value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, _options);
		}
	}
}