using System.Text.Json.Nodes;
using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Triggers;

public class RequestTrigger : WorkflowTriggerBase
{
	public new string Kind { get; } = "Http";

	public new string Type { get; } = "Request";

	public JsonObject Inputs { get; set; }

	public override JsonNode ToWorkflowTemplate()
	{
		JsonObject jsonObject = new JsonObject
		{
			["kind"] = Kind,
			["type"] = Type,
			["inputs"] = new JsonObject
			{
				["schema"] = new JsonObject()
			}
		};

		return jsonObject;
	}
}
