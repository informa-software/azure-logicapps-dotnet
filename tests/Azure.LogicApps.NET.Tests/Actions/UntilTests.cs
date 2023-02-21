using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class UntilTests
{
	[Fact]
	public void ToWorkflowJsonString_PopulatesCorrectionJson()
	{
		Until until = new Until
		{
			ActionIdentifier = "Until",
			Actions = new Dictionary<string, WorkflowActionBase>
			{
				{ "Set_Variable1", new SetVariable
					{
						ActionIdentifier = "Set_Variable1",
						Inputs = new SetVariable.Variable
						{
							Name = "name",
							Value = "Jane Bloggs"
						}
					}
				},
				{ "Set_Variable2", new SetVariable
					{
						ActionIdentifier = "Set_Variable2",
						Inputs = new SetVariable.Variable
						{
							Name = "name",
							Value = "Joe Bloggs"
						},
						RunAfter = new Dictionary<string, List<string>>
						{
							{ "Set_Variable1", new List<string> { "Succeeded" } }
						}
					}
				}
			},
			Expression = "@equals(variables('name'), 'Hello')",
			Limit = new Until.UntilLimit
			{
				Count = 60,
				Timeout = "PT1H"
			}
		};

		var actualJsonString = until.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Until",
              "actions": {
                "Set_Variable1": {
                  "type": "SetVariable",
                  "inputs": {
                    "name": "name",
                    "value": "Jane Bloggs"
                  },
                  "runAfter": {}
                },
                "Set_Variable2": {
                  "type": "SetVariable",
                  "inputs": {
                    "name": "name",
                    "value": "Joe Bloggs"
                  },
                  "runAfter": {
                    "Set_Variable1": [
                      "Succeeded"
                    ]
                  }
                }
              },
              "expression": "@equals(variables(\u0027name\u0027), \u0027Hello\u0027)",
              "limit": {
                "count": 60,
                "timeout": "PT1H"
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}