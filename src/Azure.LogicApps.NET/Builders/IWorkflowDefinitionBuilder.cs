using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Triggers;

namespace Azure.LogicApps.NET.Builders;

public interface IWorkflowDefinitionBuilder
{
	IWorkflowDefinitionBuilder WithTrigger(string name, WorkflowTriggerBase trigger);

	IWorkflowDefinitionBuilder WithRequestTrigger(RequestTrigger trigger);

	IWorkflowDefinitionBuilder AddAction(WorkflowActionBase action);

	IWorkflowDefinitionBuilder AddActions(Dictionary<string, WorkflowActionBase> actions);

	IWorkflowDefinitionBuilder AddAction(string previousActionIdentifier, WorkflowActionBase action);

	WorkflowDefinition Build();
}
