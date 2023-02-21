using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Constants;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class InitializeVariableTests
{
	[Fact]
	public void ToWorkflowJsonString_WhenTypeIsString_PopulatesCorrectionJson()
	{
		InitializeVariable initializeVariable = new InitializeVariable
		{
			ActionIdentifier = "Initialize_variable",
			Inputs = new InitializeVariable.Input
			{
				Variables = new List<InitializeVariable.Variable>
				{
					new InitializeVariable.Variable
					{
						Name = "strVariable1",
						Type = VariableDataType.String,
						Value = "Hello World"
					}
				}
			}
		};

		var actualJsonString = initializeVariable.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
                "inputs": {
                    "variables": [
                        {
                            "name": "strVariable1",
                            "type": "string",
                            "value": "Hello World"
                        }
                    ]
                },
                "runAfter": {},
                "type": "InitializeVariable"
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WhenTypeIsObject_PopulatesCorrectionJson()
	{
		InitializeVariable initializeVariable = new InitializeVariable
		{
			ActionIdentifier = "Initialize_variable",
			Inputs = new InitializeVariable.Input
			{
				Variables = new List<InitializeVariable.Variable>
				{
					new InitializeVariable.Variable
					{
						Name = "objVariable1",
						Type = VariableDataType.Object,
						Value = new
						{
							FirstName = "John",
							LastName = "Doe"
						}
					}
				}
			}
		};

		var actualJsonString = initializeVariable.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
			{
				"inputs": {
					"variables": [
						{
							"name": "objVariable1",
							"type": "object",
							"value": {
								"firstName": "John",
								"lastName": "Doe",
							}
						}
					]
				},
				"runAfter": {},
				"type": "InitializeVariable"
			}
			""";
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}