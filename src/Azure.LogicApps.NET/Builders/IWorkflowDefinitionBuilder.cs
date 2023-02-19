using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Builders;

public interface IWorkflowDefinitionBuilder
{
	IWorkflowDefinitionBuilder WithTrigger(WorkflowTriggerBase trigger);

	IWorkflowDefinitionBuilder AddAction(WorkflowActionBase action);

	IWorkflowDefinitionBuilder AddAction(string previousActionIdentifier, WorkflowActionBase action);

	WorkflowDefinition Build();
}
