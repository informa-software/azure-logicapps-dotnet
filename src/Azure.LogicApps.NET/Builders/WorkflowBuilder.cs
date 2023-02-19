using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET.Builders;

public class WorkflowBuilder : IWorkflowBuilder
{
	private readonly Workflow _workflow;


	public WorkflowBuilder()
	{
		_workflow = new Workflow();
	}

	public IWorkflowBuilder WithKind(WorkflowKind workflowKind)
	{
		_workflow.Kind = workflowKind;

		return this;
	}

	public IWorkflowBuilder WithDefinition(WorkflowDefinition workflowDefinition)
	{
		_workflow.Definition = workflowDefinition;

		return this;
	}

	public Workflow Build()
	{
		return _workflow;
	}
}