using Azure.LogicApps.NET.Base;

namespace Azure.LogicApps.NET.Actions;

public class HttpWebhook : WorkflowActionBase
{
	public HttpWebhookInput Inputs { get; set; }

	public class HttpWebhookInput
	{
		public HttpWebhookSubscription Subscribe { get; set; } = new();

		public HttpWebhookSubscription Unsubscribe { get; set; }
	}

	public class HttpWebhookSubscription
	{
		public string Method { get; set; }

		public Uri Uri { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public object Body { get; set; }

		public object Authentication { get; set; }
	}
}
