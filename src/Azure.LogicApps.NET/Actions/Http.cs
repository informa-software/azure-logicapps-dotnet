using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Constants;
using System.Text.Json.Serialization;

namespace Azure.LogicApps.NET.Actions;

public class Http : WorkflowActionBase
{
	public HttpInput Inputs { get; set; }

	public class HttpInput
	{
		public string Method { get; set; }

		public string Uri { get; set; }

		public Dictionary<string, string> Headers { get; set; } = new();

		public Dictionary<string, string> Queries { get; set; } = new();

		public object Body { get; set; }

		public HttpCallAuthentication Authentication { get; set; }
	}

	[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
	[JsonDerivedType(typeof(ActiveDirectoryOAuthentication), typeDiscriminator: AuthenticationType.ActiveDirectoryOAuth)]
	[JsonDerivedType(typeof(RawAuthentication), typeDiscriminator: AuthenticationType.Raw)]
	public abstract class HttpCallAuthentication
	{

	}

	public class ActiveDirectoryOAuthentication : HttpCallAuthentication
	{
		public string Audience { get; set; }

		public string ClientId { get; set; }

		public string Secret { get; set; }

		public string Tenant { get; set; }
	}

	public class RawAuthentication : HttpCallAuthentication
	{
		public string Value { get; set; }
	}
}
