using FluentAssertions;
using Azure.LogicApps.NET.Actions;
using Azure.LogicApps.NET.Constants;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class InitializeVariableTests
{
	[Fact]
	public void InitializeVariable_WhenTypeIsString_PopulatesCorrectionJson()
	{
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

		InitializeVariable initializeVariable = new InitializeVariable
		{
			ActionIdentifier = "Initialize_variable",
			Name = "strVariable1",
			Type = VariableDataType.String,
			Value = "Hello World"
		};
		var actualJsonString = initializeVariable.ToWorkflowTemplateJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void InitializeVariable_WhenTypeIsObject_PopulatesCorrectionJson()
	{
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

		InitializeVariable initializeVariable = new InitializeVariable
		{
			ActionIdentifier = "Initialize_variable",
			Name = "objVariable1",
			Type = VariableDataType.Object,
			Value = new
			{
				firstName = "John",
				lastName = "Doe",
			}
		};
		var actualJsonString = initializeVariable.ToWorkflowTemplateJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}