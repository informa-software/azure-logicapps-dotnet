using Azure.LogicApps.NET.Base;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET.Triggers;

public class RequestTrigger : WorkflowTriggerBase
{
	public string Kind { get; } = "Http";

	public RequestTriggerInputs Inputs { get; set; }

	private bool _enableSchemaValidation;
	[JsonIgnore]
	public bool EnableSchemaValidation
	{
		get
		{
			return _enableSchemaValidation;
		}
		set
		{
			_extensionData ??= new Dictionary<string, object>();

			if (value)
			{
				_extensionData.TryAdd("operationOptions", "EnableSchemaValidation");
			}
			else
			{
				_extensionData.Remove("operationOptions");
			}

			_enableSchemaValidation = value;
		}
	}

	private Dictionary<string, object> _extensionData;
	[JsonExtensionData]
	public Dictionary<string, object> ExtensionData
	{
		get
		{
			return _extensionData;
		}
		set
		{
			_extensionData = value;
		}
	}

	public class RequestTriggerInputs
	{
		public object Schema { get; set; }
	}
}
