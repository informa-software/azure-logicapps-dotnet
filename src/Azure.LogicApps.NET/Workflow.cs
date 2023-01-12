using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Extensions;
using Azure.LogicApps.NET.Triggers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET;

public class Workflow
{
	public WorkflowDefinition Definition { get; set; }

	public WorkflowKind Kind { get; set; } = WorkflowKind.Stateful;

	public Workflow(WorkflowKind workflowKind)
	{
		Kind = workflowKind;
		Definition = new WorkflowDefinition();
	}

	public Workflow AddTrigger(WorkflowTriggerBase trigger)
	{
		Definition.Triggers = new WorkflowTriggers
		{
			Manual = trigger
		};

		return this;
	}

	public Workflow AddAction(WorkflowActionBase action)
	{
		Definition.Actions ??= new Dictionary<string, WorkflowActionBase>();

		Definition.Actions.Add(action.ActionIdentifier, action);

		return this;
	}

	public Workflow AddAction(string previousActionIdentifier, WorkflowActionBase action)
	{
		action.RunAfter = new RunAfter
		{
			Actions = new Dictionary<string, List<string>>
			{
				{ previousActionIdentifier, new List<string> { "Succeeded" } }
			}
		};
		Definition.Actions.Add(action.ActionIdentifier, action);
		return this;
	}

	public JsonNode ToWorkflowTemplate()
	{
		var actions = new JsonObject();

		foreach (KeyValuePair<string, WorkflowActionBase> keyValuePair in Definition.Actions)
		{
			actions.Add(keyValuePair.Key, keyValuePair.Value.ToWorkflowTemplate());
		}

		var jsonObject = new JsonObject()
		{
			["definition"] = new JsonObject
			{
				["$schema"] = Definition.Schema,
				["actions"] = actions,
				["triggers"] = new JsonObject
				{
					["manual"] = Definition.Triggers.Manual.ToWorkflowTemplate()
				},
				["contentVersion"] = Definition.ContentVersion,
				["outputs"] = Definition.Outputs
			},
			["kind"] = Kind.ToString()
		};

		return jsonObject;
	}

	public string ToWorkflowTemplateJsonString()
	{
		var jsonSerializerOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		return ToWorkflowTemplateJsonString(jsonSerializerOptions);
	}

	public string ToWorkflowTemplateJsonString(JsonSerializerOptions jsonSerializerOptions)
	{
		JsonNode workflowTemplate = ToWorkflowTemplate();
		string jsonString = JsonSerializer.Serialize(workflowTemplate, jsonSerializerOptions);
		return jsonString;
	}

	public static Workflow Parse(string jsonString)
	{
		JsonNode jsonNode = JsonNode.Parse(jsonString)!;

		List<WorkflowActionBase> action = jsonNode["definition"]["actions"].AsObject()
			.Select<KeyValuePair<string, JsonNode>, WorkflowActionBase>(x =>
			{
				return x.Value.AsObject()["type"].GetValue<string>() switch
				{
					ActionType.InitializeVariable => new InitializeVariable
					{
						ActionIdentifier = x.Key,
						Name = x.Value.AsObject()["inputs"]["variables"].AsArray().First().AsObject()["name"].GetValue<string>(),
						Type = x.Value.AsObject()["inputs"]["variables"].AsArray().First().AsObject()["type"].GetValue<string>(),
						Value = x.Value.AsObject()["inputs"]["variables"].AsArray().First().AsObject()["value"]
							.GetValueAs(x.Value.AsObject()["inputs"]["variables"].AsArray().First().AsObject()["type"].GetValue<string>()),
						RunAfter = new RunAfter
						{
							Actions = x.Value.AsObject()["runAfter"]?.AsObject()
								?.ToDictionary(ss => ss.Key, ss => ss.Value.AsArray().Select(s => s.GetValue<string>()).ToList())
						}
					},
					ActionType.SetVariable => new SetVariable
					{
						ActionIdentifier = x.Key,
						Name = x.Value.AsObject()["inputs"]["name"].GetValue<string>(),
						Value = x.Value.AsObject()["inputs"]["value"].GetValueAs(),
						RunAfter = new RunAfter
						{
							Actions = x.Value.AsObject()["runAfter"]?.AsObject()
								?.ToDictionary(ss => ss.Key, ss => ss.Value.AsArray().Select(s => s.GetValue<string>()).ToList())
						}
					},
					_ => throw new NotImplementedException()
				};
			})
			.ToList();

		RequestTrigger requestTrigger = null;
		if (jsonNode["definition"]["triggers"]["manual"]["type"].GetValue<string>() == "Request")
		{
			requestTrigger = new RequestTrigger
			{
				Inputs = jsonNode["definition"]["triggers"]["manual"]["inputs"].AsObject()
			};
		}

		Workflow workflow = new Workflow(jsonNode["kind"].GetValue<WorkflowKind>())
		{
			Definition = new WorkflowDefinition
			{
				Actions = action.ToDictionary(x => x.ActionIdentifier, x => x),
				Triggers = new WorkflowTriggers
				{
					Manual = requestTrigger
				},
				Schema = jsonNode["definition"]["schema"].GetValue<string>(),
				ContentVersion = jsonNode["definition"]["contentVersion"].GetValue<string>(),
				Outputs = jsonNode["definition"]["outputs"]?.AsObject()
			},
		};

		return workflow;
	}
}
