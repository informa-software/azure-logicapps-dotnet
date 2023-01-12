using Azure.LogicApps.NET.Constants;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET.Extensions;

public static class JsonNodeExtensions
{
	public static object GetValueAs(this JsonNode jsonNode, string type)
	{
		if (type == string.Empty)
		{
			return jsonNode;
		}

		if (jsonNode is JsonObject)
		{
			return jsonNode.AsObject();
		}

		return type switch
		{
			VariableDataType.Boolean => jsonNode.GetValue<bool>(),
			VariableDataType.String => jsonNode.GetValue<string>(),
			VariableDataType.Float => jsonNode.GetValue<float>(),
			VariableDataType.Integer => jsonNode.GetValue<int>(),
			VariableDataType.Array => jsonNode.AsArray(),
			VariableDataType.Object => jsonNode.AsObject(),
			_ => throw new NotImplementedException(),
		};
	}

	public static object GetValueAs(this JsonNode jsonNode)
	{
		if (jsonNode is JsonObject)
		{
			return jsonNode.AsObject();
		}

		return jsonNode;
	}
}
