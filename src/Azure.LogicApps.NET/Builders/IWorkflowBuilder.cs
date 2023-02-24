using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Triggers;

namespace Azure.LogicApps.NET.Builders;

public interface IWorkflowBuilder
{
	IWorkflowBuilder WithKind(WorkflowKind workflowKind);

	IWorkflowBuilder WithTrigger(string name, WorkflowTriggerBase trigger);

	IWorkflowBuilder WithRequestTrigger(RequestTrigger trigger);

	IWorkflowBuilder AddAction(WorkflowActionBase action);

	IWorkflowBuilder AddAction(string previousActionIdentifier, WorkflowActionBase action);

	IWorkflowBuilder AddActions(Dictionary<string, WorkflowActionBase> actions);

	Workflow Build();
}
