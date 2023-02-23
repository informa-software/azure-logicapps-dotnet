using Azure.LogicApps.NET.Actions;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Azure.LogicApps.NET.Tests.Actions;

public class HttpTests
{
	[Fact]
	public void ToWorkflowJsonString_PopulatesCorrectJson()
	{
		Http action = new Http
		{
			ActionIdentifier = "Http1",
			Inputs = new Http.HttpInput
			{
				Method = "GET",
				Uri = new Uri("http://www.xyz.com")
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Http",
              "inputs": {
                "method": "GET",
                "uri": "http://www.xyz.com",
                "headers": {},
                "queries": {},
                "body": null,
                "authentication": null
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WithActiveDirectoryOAuth_PopulatesCorrectJson()
	{
		Http action = new Http
		{
			ActionIdentifier = "Http1",
			Inputs = new Http.HttpInput
			{
				Method = "GET",
				Uri = new Uri("http://www.xyz.com"),
				Authentication = new Http.ActiveDirectoryOAuthentication
				{
					Audience = "SomeAudience",
					ClientId = "SomeClientId",
					Secret = "SomeSecret",
					Tenant = "SomeTenant",
				}
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Http",
              "inputs": {
                "method": "GET",
                "uri": "http://www.xyz.com",
                "headers": {},
                "queries": {},
                "body": null,
                "authentication": {
                  "type": "ActiveDirectoryOAuth",
                  "audience": "SomeAudience",
                  "clientId": "SomeClientId",
                  "secret": "SomeSecret",
                  "tenant": "SomeTenant"
                }
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WithRawAuth_PopulatesCorrectJson()
	{
		Http action = new Http
		{
			ActionIdentifier = "Http1",
			Inputs = new Http.HttpInput
			{
				Method = "GET",
				Uri = new Uri("http://www.xyz.com"),
				Authentication = new Http.RawAuthentication
				{
					Value = "SomeValue"
				}
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Http",
              "inputs": {
                "method": "GET",
                "uri": "http://www.xyz.com",
                "headers": {},
                "queries": {},
                "body": null,
                "authentication": {
                  "type": "Raw",
                  "value": "SomeValue"
                }
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WithCustomHeaders_PopulatesCorrectJson()
	{
		Http action = new Http
		{
			ActionIdentifier = "Http1",
			Inputs = new Http.HttpInput
			{
				Method = "GET",
				Uri = new Uri("http://www.xyz.com"),
				Headers =
				{
					{ "X-TenantId", "SomeTenant" }
				}
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Http",
              "inputs": {
                "method": "GET",
                "uri": "http://www.xyz.com",
                "headers": {
                  "X-TenantId": "SomeTenant"
                },
                "queries": {},
                "body": null,
                "authentication": null
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}

	[Fact]
	public void ToWorkflowJsonString_WithBody_PopulatesCorrectJson()
	{
		Http action = new Http
		{
			ActionIdentifier = "Http1",
			Inputs = new Http.HttpInput
			{
				Method = "POST",
				Uri = new Uri("http://www.xyz.com"),
				Body = new
				{
					FirstName = "John",
					LastName = "Doe"
				}
			}
		};

		var actualJsonString = action.ToWorkflowJsonString();
		var actualJObject = JObject.Parse(actualJsonString);

		string expectedJsonString =
			"""
            {
              "type": "Http",
              "inputs": {
                "method": "POST",
                "uri": "http://www.xyz.com",
                "headers": {},
                "queries": {},
                "body": {
                  "firstName": "John",
                  "lastName": "Doe"
                },
                "authentication": null
              },
              "runAfter": {}
            }
            """;
		var expectedJObject = JObject.Parse(expectedJsonString);

		JToken.DeepEquals(actualJObject, expectedJObject).Should().BeTrue();
	}
}