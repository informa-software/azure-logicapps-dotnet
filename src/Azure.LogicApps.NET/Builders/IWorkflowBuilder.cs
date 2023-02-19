using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET.Builders;

public interface IWorkflowBuilder
{
	IWorkflowBuilder WithKind(WorkflowKind workflowKind);

	IWorkflowBuilder WithDefinition(WorkflowDefinition workflowDefinition);

	Workflow Build();
}
