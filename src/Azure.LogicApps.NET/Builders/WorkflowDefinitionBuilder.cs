using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Triggers;

namespace Azure.LogicApps.NET.Builders;

public class WorkflowDefinitionBuilder : IWorkflowDefinitionBuilder
{
	private readonly WorkflowDefinition _workflowDefinition;

	public WorkflowDefinitionBuilder()
	{
		_workflowDefinition = new WorkflowDefinition();
	}

	public IWorkflowDefinitionBuilder WithTrigger(WorkflowTriggerBase trigger)
	{
		_workflowDefinition.Triggers = new WorkflowTriggers
		{
			Manual = trigger
		};

		return this;
	}

	public IWorkflowDefinitionBuilder AddAction(WorkflowActionBase action)
	{
		ArgumentNullException.ThrowIfNull(action);

		_workflowDefinition.Actions ??= new Dictionary<string, WorkflowActionBase>();
		_workflowDefinition.Actions.Add(action.ActionIdentifier, action);

		return this;
	}

	public IWorkflowDefinitionBuilder AddActions(Dictionary<string, WorkflowActionBase> actions)
	{
		ArgumentNullException.ThrowIfNull(actions);

		foreach (KeyValuePair<string, WorkflowActionBase> action in actions)
		{
			AddAction(action.Value);
		}

		return this;
	}

	public IWorkflowDefinitionBuilder AddAction(string previousActionIdentifier, WorkflowActionBase action)
	{
		ArgumentException.ThrowIfNullOrEmpty(previousActionIdentifier);

		action.RunAfter = new Dictionary<string, List<string>>
		{
			{ previousActionIdentifier, new List<string> { "Succeeded" } }
		};

		_workflowDefinition.Actions ??= new Dictionary<string, WorkflowActionBase>();
		_workflowDefinition.Actions.Add(action.ActionIdentifier, action);

		return this;
	}

	public WorkflowDefinition Build()
	{
		return _workflowDefinition;
	}
}
