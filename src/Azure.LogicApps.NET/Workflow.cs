using Azure.LogicApps.NET.Constants;

namespace Azure.LogicApps.NET;

public class Workflow
{
	public WorkflowDefinition Definition { get; set; }

	public WorkflowKind Kind { get; set; }
}
