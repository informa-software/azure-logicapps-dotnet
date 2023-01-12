using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET.Actions;

public class SetVariable : WorkflowActionBase
{
	public string Name { get; set; }

	public string Type { get; set; }

	public object Value { get; set; }

	public override JsonNode ToWorkflowTemplate()
	{
		JsonObject jsonObject = new JsonObject
		{
			["type"] = ActionType.SetVariable,
			["inputs"] = new JsonObject
			{
				["name"] = Name,
				["value"] = Type switch
				{
					VariableDataType.String => Value.ToString(),
					VariableDataType.Integer => int.Parse(Value.ToString()),
					VariableDataType.Boolean => bool.Parse(Value.ToString()),
					VariableDataType.Float => float.Parse(Value.ToString()),
					VariableDataType.Object or VariableDataType.Array => JsonValue.Parse(JsonSerializer.Serialize(Value)),
					_ => throw new ArgumentOutOfRangeException()
				}
			},
			["runAfter"] = new JsonObject()
		};

		if (RunAfter?.Actions.Any() == true)
		{
			foreach (var item in RunAfter.Actions)
			{
				jsonObject["runAfter"][item.Key] = JsonNode.Parse(JsonSerializer.Serialize(item.Value));
			}
		}

		return jsonObject;
	}
}