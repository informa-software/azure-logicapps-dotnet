using Azure.LogicApps.NET.Triggers;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET.Base;

[JsonDerivedType(typeof(RequestTrigger))]
public class WorkflowTriggerBase
{
	public string Kind { get; set; }

	public string Type { get; set; }
}
