using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Triggers;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Triggers;

public class RequestTriggerTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ToWorkflowJsonString_PopulatesCorrectJson(bool enableSchemaValidation)
	{
		RequestTrigger action = new RequestTrigger
		{
			EnableSchemaValidation = enableSchemaValidation,
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		if (enableSchemaValidation)
		{
			string expectedJsonString =
			"""
            {
                "type": "Request",
                "kind": "Http",
                "inputs": null,
                "operationOptions": "EnableSchemaValidation"
            }
            """;
			var expectedJObject = JObject.Parse(expectedJsonString);

			JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
		}
		else
		{
			string expectedJsonString =
			"""
            {
                "type": "Request",
                "kind": "Http",
                "inputs": null
            }
            """;
			var expectedJObject = JObject.Parse(expectedJsonString);

			JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
		}
	}

	[Fact]
	public void FromWorkflowJsonString_PopulatesCorrectObject()
	{
		string jsonString =
			"""
			{
				"type": "Request",
				"kind": "Http",
				"inputs": null,
				"operationOptions": "EnableSchemaValidation"
			}
			""";

		RequestTrigger trigger = (RequestTrigger)WorkflowTriggerBase.FromWorkflowJsonString(jsonString);
		trigger.EnableSchemaValidation.Should().BeTrue();

		jsonString =
			"""
			{
				"type": "Request",
				"kind": "Http",
				"inputs": null
			}
			""";

		trigger = (RequestTrigger)WorkflowTriggerBase.FromWorkflowJsonString(jsonString);
		trigger.EnableSchemaValidation.Should().BeFalse();
	}
}
