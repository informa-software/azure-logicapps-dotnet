using Azure.LogicApps.NET;
using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Triggers;
using System.Text.Json;
using System.Text.Json.Nodes;

InitializeVariable initializeVariable1 = new InitializeVariable
{
	ActionIdentifier = "Initialize_Variable1",
	Name = "objVariable",
	Type = VariableDataType.Object,
	Value = new
	{
		firstName = "John",
		lastName = "Doe"
	}
};

InitializeVariable initializeVariable2 = new InitializeVariable
{
	ActionIdentifier = "Initialize_Variable2",
	Name = "intVariable",
	Type = VariableDataType.Integer,
	Value = 10
};

InitializeVariable initializeVariable3 = new InitializeVariable
{
	ActionIdentifier = "Initialize_Variable3",
	Name = "arrVariable",
	Type = VariableDataType.Array,
	Value = new[]
	{
		"John",
		"Doe"
	}
};

InitializeVariable initializeVariable4 = new InitializeVariable
{
	ActionIdentifier = "Initialize_Variable4",
	Name = "strVariable",
	Type = VariableDataType.String,
	Value = "Fred Bloggs"
};

InitializeVariable initializeVariable5 = new InitializeVariable
{
	ActionIdentifier = "Initialize_Variable5",
	Name = "boolVariable",
	Type = VariableDataType.Boolean,
	Value = true
};

SetVariable setVariable1 = new SetVariable
{
	ActionIdentifier = "Set_Variable1",
	Name = "objVariable",
	Value = new
	{
		firstName = "John",
		lastName = "Doe"
	},
	Type = VariableDataType.Object
};

SetVariable setVariable2 = new SetVariable
{
	ActionIdentifier = "Set_Variable2",
	Name = "strVariable",
	Value = "hello",
	Type = VariableDataType.String
};

SetVariable setVariable3 = new SetVariable
{
	ActionIdentifier = "Set_Variable3",
	Name = "arrVariable",
	Value = new[]
	{
		"Hello",
		"World"
	},
	Type = VariableDataType.Array
};

SetVariable setVariable4 = new SetVariable
{
	ActionIdentifier = "Set_Variable4",
	Name = "boolVariable",
	Value = true,
	Type = VariableDataType.Boolean
};

SetVariable setVariable5 = new SetVariable
{
	ActionIdentifier = "Set_Variable5",
	Name = "intVariable",
	Value = 100,
	Type = VariableDataType.Integer
};

var jsonSerializerOptions = new JsonSerializerOptions
{
	WriteIndented = true,
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
};

Workflow workflow = new Workflow(WorkflowKind.Stateful);
workflow.AddTrigger(new RequestTrigger());
workflow.AddAction(initializeVariable1);
workflow.AddAction(initializeVariable1.ActionIdentifier, initializeVariable2);
workflow.AddAction(initializeVariable2.ActionIdentifier, initializeVariable3);
workflow.AddAction(initializeVariable3.ActionIdentifier, initializeVariable4);
workflow.AddAction(initializeVariable4.ActionIdentifier, initializeVariable5);
workflow.AddAction(initializeVariable5.ActionIdentifier, setVariable1);
workflow.AddAction(setVariable1.ActionIdentifier, setVariable2);
workflow.AddAction(setVariable2.ActionIdentifier, setVariable3);
workflow.AddAction(setVariable3.ActionIdentifier, setVariable4);
workflow.AddAction(setVariable4.ActionIdentifier, setVariable5);

JsonNode workflowTemplate = workflow.ToWorkflowTemplate();
string jsonString = JsonSerializer.Serialize(workflowTemplate, jsonSerializerOptions);

Console.WriteLine(workflow.ToWorkflowTemplateJsonString());

Workflow parsedWorkflow = Workflow.Parse(jsonString);

//string resourceId = "/subscriptions/4e7d5747-5253-4cb5-ab61-db99bffdc924/resourceGroups/rg-logic-apps-demo/providers/Microsoft.Web/sites/logic-ju-test-001/workflows/multiple-level-approval-workflow";
//Azure.ResourceManager.ArmClient armClient = new Azure.ResourceManager.ArmClient(new DefaultAzureCredential(), "4e7d5747-5253-4cb5-ab61-db99bffdc924");
//LogicWorkflowResource workflowResource = armClient.GetLogicWorkflowResource(new Azure.Core.ResourceIdentifier(resourceId));

Console.ReadLine();