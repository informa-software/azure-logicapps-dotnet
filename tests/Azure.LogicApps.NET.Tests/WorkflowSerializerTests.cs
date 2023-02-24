using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using Azure.LogicApps.NET.Builders;
using Azure.LogicApps.NET.Constants;
using Azure.LogicApps.NET.Triggers;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace Azure.LogicApps.NET.Tests;

public class WorkflowSerializerTests
{
	[Fact]
	public void ToWorkflowJsonString_PopulatesCorrectJson()
	{
		IfCondition ifCondition = new IfCondition
		{
			ActionIdentifier = "SomeCondition",
			Actions = new Dictionary<string, WorkflowActionBase>
			{
				{ "Set_Variable2", new SetVariable
					{
						ActionIdentifier = "Set_Variable2",
						Inputs = new SetVariable.Variable
						{
							Name = "name",
							Value = "Fred Bloggs"
						}
					}
				}
			},
			Expression = new JsonObject
			{
				["and"] = new JsonArray
				{
					new JsonObject
					{
						[ "equals"] = new JsonArray()
						{
							"@variables('name')",
							"hello"
						}
					}
				}
			},
			Else = new IfCondition.ElseStatement
			{
				Actions = new Dictionary<string, WorkflowActionBase>
				{
					{ "SomeOtherCondition", new IfCondition
						{
							ActionIdentifier = "SomeOtherCondition",
							Actions = new Dictionary<string, WorkflowActionBase>
							{
								{ "Set_Variable4", new SetVariable
									{
										ActionIdentifier = "Set_Variable4",
										Inputs = new SetVariable.Variable
										{
											Name = "name",
											Value = "Fred Bloggs"
										}
									}
								}
							},
							Expression = new JsonObject
							{
								["and"] = new JsonArray
								{
									new JsonObject
									{
										[ "equals"] = new JsonArray()
										{
											"@variables('name')",
											"hello"
										}
									}
								}
							},
							Else = new IfCondition.ElseStatement
							{
								Actions = new Dictionary<string, WorkflowActionBase>
								{
									{ "Set_Variable5", new SetVariable
										{
											ActionIdentifier = "Set_Variable5",
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
			}
		};

		WorkflowDefinition workflowDefinition = new WorkflowDefinitionBuilder()
			.WithRequestTrigger(new RequestTrigger())
			.AddAction(new InitializeVariable()
			{
				ActionIdentifier = "Initialize_Variable1",
				Inputs = new InitializeVariable.Input
				{
					Variables = new List<InitializeVariable.Variable>
					{
				new InitializeVariable.Variable
				{
					Name = "name",
					Type = VariableDataType.String,
					Value = "John Doe"
				}
					}
				}
			})
			.AddAction("Initialize_Variable1", new InitializeVariable()
			{
				ActionIdentifier = "Initialize_Variable2",
				Inputs = new InitializeVariable.Input
				{
					Variables = new List<InitializeVariable.Variable>
					{
						new InitializeVariable.Variable
						{
							Name = "address",
							Type = VariableDataType.Object,
							Value = new
							{
								addressLine1 = "Ellerslie",
								City = "Auckland"
							}
						}
					}
				}
			})
			.AddAction("Initialize_Variable2", new SetVariable
			{
				ActionIdentifier = "Set_Variable1",
				Inputs = new SetVariable.Variable
				{
					Name = "name",
					Value = "Fred Bloggs"
				}
			})
			.AddAction("Set_Variable1", ifCondition)
			.AddAction(ifCondition.ActionIdentifier, new Until
			{
				ActionIdentifier = "Until1",
				Actions = new Dictionary<string, WorkflowActionBase>
				{
					{ "Set_Variable6", new SetVariable
						{
							ActionIdentifier = "Set_Variable6",
							Inputs = new SetVariable.Variable
							{
								Name = "name",
								Value = "Jane Bloggs"
							}
						}
					},
					{ "Set_Variable7", new SetVariable
						{
							ActionIdentifier = "Set_Variable7",
							Inputs = new SetVariable.Variable
							{
								Name = "name",
								Value = "Joe Bloggs"
							},
							RunAfter = new Dictionary<string, List<string>>
							{
								{ "Set_Variable6", new List<string> { "Succeeded" } }
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
			})
			.Build();

		Workflow workflow = new WorkflowBuilder()
			.WithKind(WorkflowKind.Stateful)
			.WithDefinition(workflowDefinition)
			.Build();

		var actualJsonString = workflow.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "definition": {
                "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
                "actions": {
                  "Initialize_Variable1": {
                    "type": "InitializeVariable",
                    "inputs": {
                      "variables": [
                        {
                          "name": "name",
                          "type": "string",
                          "value": "John Doe"
                        }
                      ]
                    },
                    "runAfter": {}
                  },
                  "Initialize_Variable2": {
                    "type": "InitializeVariable",
                    "inputs": {
                      "variables": [
                        {
                          "name": "address",
                          "type": "object",
                          "value": {
                            "addressLine1": "Ellerslie",
                            "city": "Auckland"
                          }
                        }
                      ]
                    },
                    "runAfter": {
                      "Initialize_Variable1": [
                        "Succeeded"
                      ]
                    }
                  },
                  "Set_Variable1": {
                    "type": "SetVariable",
                    "inputs": {
                      "name": "name",
                      "value": "Fred Bloggs"
                    },
                    "runAfter": {
                      "Initialize_Variable2": [
                        "Succeeded"
                      ]
                    }
                  },
                  "SomeCondition": {
                    "type": "If",
                    "actions": {
                      "Set_Variable2": {
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
                        "SomeOtherCondition": {
                          "type": "If",
                          "actions": {
                            "Set_Variable4": {
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
                              "Set_Variable5": {
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
                    "runAfter": {
                      "Set_Variable1": [
                        "Succeeded"
                      ]
                    }
                  },
                  "Until1": {
                    "type": "Until",
                    "actions": {
                      "Set_Variable6": {
                        "type": "SetVariable",
                        "inputs": {
                          "name": "name",
                          "value": "Jane Bloggs"
                        },
                        "runAfter": {}
                      },
                      "Set_Variable7": {
                        "type": "SetVariable",
                        "inputs": {
                          "name": "name",
                          "value": "Joe Bloggs"
                        },
                        "runAfter": {
                          "Set_Variable6": [
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
                    "runAfter": {
                      "SomeCondition": [
                        "Succeeded"
                      ]
                    }
                  }
                },
                "triggers": {
                  "Manual": {
                    "type": "Request",
                    "kind": "Http",
                    "inputs": null
                  }
                },
                "contentVersion": "1.0.0.0",
                "outputs": null
              },
              "kind": "Stateful"
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}
