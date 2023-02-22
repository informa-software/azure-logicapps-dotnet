using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class IfConditionTests
{
	[Fact]
	public void ToWorkflowJsonString_PopulatesCorrectionJson()
	{
		IfCondition action = new IfCondition
		{
			ActionIdentifier = "If_Condition",
			Actions = new Dictionary<string, WorkflowActionBase>
			{
				{ "Set_Variable1", new SetVariable
					{
						ActionIdentifier = "Set_Variable1",
						Inputs = new SetVariable.Variable
						{
							Name = "name",
							Value = "Fred Bloggs"
						}
					}
				}
			},
			Expression = new IfCondition.ConditionExpression
			{
				{ "and", new List<Dictionary<string, List<string>>>
					{
						new Dictionary<string, List<string>>
						{
							{ "equals", new List<string>
								{
									"@variables('name')",
									"hello"
								}
							}
						}
					}
				}
			},
			Else = new IfCondition.ElseStatement
			{
				Actions = new Dictionary<string, WorkflowActionBase>
				{
					{ "Set_Variable2", new SetVariable
						{
							ActionIdentifier = "Set_Variable2",
							Inputs = new SetVariable.Variable
							{
								Name = "name",
								Value = "Joe Bloggs"
							}
						}
					}
				},
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "If",
              "actions": {
                "Set_Variable1": {
                  "type": "SetVariable",
                  "inputs": {
                    "name": "name",
                    "value": "Fred Bloggs"
                  },
                  "runAfter": {}
                }
              },
              "else": {
                "actions": {
                  "Set_Variable2": {
                    "type": "SetVariable",
                    "inputs": {
                      "name": "name",
                      "value": "Joe Bloggs"
                    },
                    "runAfter": {}
                  }
                }
              },
              "expression": {
                "and": [
                  {
                    "equals": [
                      "@variables(\u0027name\u0027)",
                      "hello"
                    ]
                  }
                ]
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}