using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class SwitchConditionTests
{
	[Fact]
	public void ToJsonString_PopulatesCorrectionJson()
	{
		SwitchCondition switchCondition = new SwitchCondition
		{
			ActionIdentifier = "Switch_Condition1",
			Expression = "@variables('name')",
			Cases = new Dictionary<string, SwitchCondition.SwitchCaseStatement>
			{
				{
					"Case_1", new SwitchCondition.SwitchCaseStatement
					{
						Actions = new Dictionary<string, WorkflowActionBase>
						{
							{ "SomeCondition", new IfCondition
								{
									ActionIdentifier = "SomeCondition",
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
								}
							}
						},
						Case = "A",
					}
				},
				{
					"Case_2", new SwitchCondition.SwitchCaseStatement
					{
						Actions = new Dictionary<string, WorkflowActionBase>
						{
							{ "Set_Variable3", new SetVariable
								{
									ActionIdentifier = "Set_Variable3",
									Inputs = new SetVariable.Variable
									{
										Name = "name",
										Value = "Joe Bloggs"
									}
								}
							}
						},
						Case = "B",
					}
				}
			},
			Default = new SwitchCondition.SwitchDefaultCaseStatement
			{
				Actions = new Dictionary<string, WorkflowActionBase>
				{
					{ "Set_Variable4", new SetVariable
						{
							ActionIdentifier = "Set_Variable4",
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

		var actualJsonString = switchCondition.ToJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Switch",
              "expression": "@variables(\u0027name\u0027)",
              "default": {
                "actions": {
                  "Set_Variable4": {
                    "type": "SetVariable",
                    "inputs": {
                      "name": "name",
                      "value": "Joe Bloggs"
                    },
                    "runAfter": {}
                  }
                }
              },
              "cases": {
                "Case_1": {
                  "case": "A",
                  "actions": {
                    "SomeCondition": {
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
                  }
                },
                "Case_2": {
                  "case": "B",
                  "actions": {
                    "Set_Variable3": {
                      "type": "SetVariable",
                      "inputs": {
                        "name": "name",
                        "value": "Joe Bloggs"
                      },
                      "runAfter": {}
                    }
                  }
                }
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}