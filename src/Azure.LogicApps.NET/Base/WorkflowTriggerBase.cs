using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Azure.LogicApps.NET.Triggers;

namespace Azure.LogicApps.NET.Base;

[JsonDerivedType(typeof(RequestTrigger))]
public abstract class WorkflowTriggerBase
{
    public string Kind { get; set; }

    public string Type { get; set; }

    public abstract JsonNode ToWorkflowTemplate();
}
