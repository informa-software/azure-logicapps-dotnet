using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Base;
using FluentAssertions;

namespace Azure.LogicApps.NET.Tests;

public class WorkflowDeserializerTests
{
	[Fact]
	public void FromJsonString_PopulatesCorrectionObject()
	{
		string jsonString =
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
                  "manual": {
                    "kind": "Http",
                    "type": "Request",
                    "inputs": null
                  }
                },
                "contentVersion": "1.0.0.0",
                "outputs": null
              },
              "kind": "Stateful"
            }
            """;

		Workflow workflow = Workflow.FromJsonString(jsonString);
		bool isWorkflowActionIdentifierPopulatedWithCorrectValue = IsWorkflowActionIdentifierPopulatedWithCorrectValue(workflow.Definition.Actions);
		isWorkflowActionIdentifierPopulatedWithCorrectValue.Should().BeTrue();
	}

	private bool IsWorkflowActionIdentifierPopulatedWithCorrectValue(Dictionary<string, WorkflowActionBase> workflowAction)
	{
		foreach (KeyValuePair<string, WorkflowActionBase> item in workflowAction)
		{
			if (item.Value is IfCondition ifCondition)
			{
				return IsWorkflowActionIdentifierPopulatedWithCorrectValue(ifCondition.Actions);
			}

			if (item.Value is Until untilCondition)
			{
				return IsWorkflowActionIdentifierPopulatedWithCorrectValue(untilCondition.Actions);
			}

			if (item.Value.ActionIdentifier != item.Key)
			{
				return false;
			}
		}

		return true;
	}
}
