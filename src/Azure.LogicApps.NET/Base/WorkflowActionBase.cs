using Azure.LogicApps.NET.Actions;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Azure.LogicApps.NET.Base;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(InitializeVariable), typeDiscriminator: nameof(InitializeVariable))]
[JsonDerivedType(typeof(SetVariable), typeDiscriminator: nameof(SetVariable))]
[JsonDerivedType(typeof(IfCondition), typeDiscriminator: "If")]
public abstract class WorkflowActionBase
{
	public required string ActionIdentifier { get; set; }

	public abstract string Type { get; }

	public Dictionary<string, List<string>> RunAfter { get; set; } = new Dictionary<string, List<string>>();
}
