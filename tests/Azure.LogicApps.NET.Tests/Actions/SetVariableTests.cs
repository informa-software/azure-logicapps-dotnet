using Azure.LogicApps.NET.Actions;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class SetVariableTests
{
	[Fact]
	public void ToWorkflowJsonString_WhenTypeIsString_PopulatesCorrectionJson()
	{
		SetVariable setVariable = new SetVariable
		{
			ActionIdentifier = "Set_Variable1",
			Inputs = new SetVariable.Variable
			{
				Name = "name",
				Value = "Fred Bloggs"
			}
		};

		var actualJsonString = setVariable.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "SetVariable",
              "inputs": {
                "name": "name",
                "value": "Fred Bloggs"
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WhenTypeIsObject_PopulatesCorrectionJson()
	{
		SetVariable setVariable = new SetVariable
		{
			ActionIdentifier = "Set_Variable1",
			Inputs = new SetVariable.Variable
			{
				Name = "address",
				Value = new
				{
					AddressLine1 = "AddressLine1Value",
					AddressLine2 = "AddressLine2Value",
				}
			}
		};

		var actualJsonString = setVariable.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "SetVariable",
              "inputs": {
                "name": "address",
                "value": {
                  "addressLine1": "AddressLine1Value",
                  "addressLine2": "AddressLine2Value"
                }
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}