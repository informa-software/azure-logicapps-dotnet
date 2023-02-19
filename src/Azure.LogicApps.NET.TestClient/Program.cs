using Azure.LogicApps.NET;
using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Builders;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Triggers;
using System.Text.Json;
using System.Text.Json.Serialization;

IfCondition ifCondition = new IfCondition
{
	ActionIdentifier = "SomeCondition",
	Actions = new Dictionary<string, WorkflowActionBase>
	{
		{ "Set_Variable2", new SetVariable
			{
				ActionIdentifier = "Set_Variable2",
				Inputs = new SetVariable.Variable
				{
					Name = "name",
					Value = "Fred Bloggs"
				}
			}
		}
	},
	Expression = new IfCondition.ConditionExpression
	{
		{ "and", new List<Dictionary<string, List<string>>>
			{
				new Dictionary<string, List<string>>
				{
					{ "equals", new List<string>
						{
							"@variables('name')",
							"hello"
						}
					}
				}
			}
		}
	},
	Else = new IfCondition.ElseStatement
	{
		Actions = new Dictionary<string, WorkflowActionBase>
		{
			{ "Set_Variable3", new SetVariable
				{
					ActionIdentifier = "Set_Variable3",
					Inputs = new SetVariable.Variable
					{
						Name = "name",
						Value = "Joe Bloggs"
					}
				}
			}
		},
	}
};

WorkflowDefinition workflowDefinition = new WorkflowDefinitionBuilder()
	.WithTrigger(new RequestTrigger())
	.AddAction(new InitializeVariable()
	{
		ActionIdentifier = "Initialize_Variable1",
		Inputs = new InitializeVariable.Input
		{
			Variables = new List<InitializeVariable.Variable>
			{
				new InitializeVariable.Variable
				{
					Name = "name",
					Type = VariableDataType.String,
					Value = "John Doe"
				}
			}
		}
	})
	.AddAction("Initialize_Variable1", new InitializeVariable()
	{
		ActionIdentifier = "Initialize_Variable2",
		Inputs = new InitializeVariable.Input
		{
			Variables = new List<InitializeVariable.Variable>
			{
				new InitializeVariable.Variable
				{
					Name = "address",
					Type = VariableDataType.Object,
					Value = new
					{
						addressLine1 = "Ellerslie",
						City = "Auckland"
					}
				}
			}
		}
	})
	.AddAction("Initialize_Variable2", new SetVariable
	{
		ActionIdentifier = "Set_Variable1",
		Inputs = new SetVariable.Variable
		{
			Name = "name",
			Value = "Fred Bloggs"
		}
	})
	.AddAction("Set_Variable1", ifCondition)
	.Build();

Workflow workflow = new WorkflowBuilder()
	.WithKind(WorkflowKind.Stateful)
	.WithDefinition(workflowDefinition)
	.Build();

var jsonSerializerOptions = new JsonSerializerOptions
{
	WriteIndented = true,
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	PropertyNameCaseInsensitive = true,
	Converters =
	{
		new JsonStringEnumConverter()
	},
};

string jsonString = JsonSerializer.Serialize(workflow, jsonSerializerOptions);

//Console.WriteLine(jsonString);

Workflow parsedWorkflow = JsonSerializer.Deserialize<Workflow>(jsonString, jsonSerializerOptions);
jsonString = JsonSerializer.Serialize(parsedWorkflow, jsonSerializerOptions);

Console.WriteLine(jsonString);

Console.ReadLine();