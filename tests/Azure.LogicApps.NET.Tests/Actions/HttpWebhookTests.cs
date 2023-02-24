using Azure.LogicApps.NET.Actions;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class HttpWebhookTests
{
	[Fact]
	public void ToWorkflowJsonString_PopulatesCorrectJson()
	{
		HttpWebhook action = new HttpWebhook
		{
			ActionIdentifier = "HttpWebhook1",
			Inputs = new HttpWebhook.HttpWebhookInput
			{
				Subscribe = new HttpWebhook.HttpWebhookSubscription
				{
					Method = "POST",
					Uri = "http://www.xyz.com",
					Body = new
					{
						Person = "@variables('person')",
						CallbackUrl = "@listCallbackUrl()"
					}
				}
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "HttpWebhook",
              "inputs": {
                "subscribe": {
                  "method": "POST",
                  "uri": "http://www.xyz.com",
                  "headers": null,
                  "body": {
                    "person": "@variables(\u0027person\u0027)",
                    "callbackUrl": "@listCallbackUrl()"
                  },
                  "authentication": null
                },
                "unsubscribe": null
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}