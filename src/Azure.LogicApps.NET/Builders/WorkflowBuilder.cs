using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Triggers;

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

	public IWorkflowBuilder WithRequestTrigger(RequestTrigger trigger)
	{
		ArgumentNullException.ThrowIfNull(trigger);

		_workflow.Definition ??= new WorkflowDefinition();

		if (_workflow.Definition.Triggers?.Any() == true)
		{
			throw new InvalidOperationException("Workflow can have only one trigger.");
		}

		_workflow.Definition.Triggers = new Dictionary<string, WorkflowTriggerBase>
		{
			{"Manual", trigger }
		};

		return this;
	}

	public IWorkflowBuilder WithTrigger(string name, WorkflowTriggerBase trigger)
	{
		ArgumentException.ThrowIfNullOrEmpty(name);
		ArgumentNullException.ThrowIfNull(trigger);

		_workflow.Definition ??= new WorkflowDefinition();

		if (_workflow.Definition.Triggers?.Any() == true)
		{
			throw new InvalidOperationException("Workflow can have only one trigger.");
		}

		_workflow.Definition.Triggers = new Dictionary<string, WorkflowTriggerBase>
		{
			{name, trigger }
		};

		return this;
	}

	public IWorkflowBuilder AddAction(WorkflowActionBase action)
	{
		ArgumentNullException.ThrowIfNull(action);

		_workflow.Definition ??= new WorkflowDefinition();
		_workflow.Definition.Actions ??= new Dictionary<string, WorkflowActionBase>();

		if (_workflow.Definition.Actions.Any() && !action.RunAfter.Any())
		{
			action.RunAfter = new Dictionary<string, List<string>>
			{
				{ _workflow.Definition.Actions.Last().Key, new List<string> { ActionRunStatus.Succeeded } }
			};
		}

		_workflow.Definition.Actions.Add(action.ActionIdentifier, action);

		return this;
	}

	public IWorkflowBuilder AddAction(string previousActionIdentifier, WorkflowActionBase action)
	{
		ArgumentException.ThrowIfNullOrEmpty(previousActionIdentifier);
		ArgumentNullException.ThrowIfNull(action);

		action.RunAfter = new Dictionary<string, List<string>>
		{
			{ previousActionIdentifier, new List<string> { ActionRunStatus.Succeeded } }
		};

		AddAction(action);

		return this;
	}

	public IWorkflowBuilder AddActions(Dictionary<string, WorkflowActionBase> actions)
	{
		ArgumentNullException.ThrowIfNull(actions);

		foreach (KeyValuePair<string, WorkflowActionBase> action in actions)
		{
			AddAction(action.Value);
		}

		return this;
	}

	public Workflow Build()
	{
		return _workflow;
	}
}