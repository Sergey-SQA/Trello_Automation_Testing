using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NunitTrelloTest.Utils;
using RestSharp;
using System.Collections.Generic;
using System.IO;

namespace NunitTrelloTest.Tests
{
    public class BaseTest
    {
        private const string ApiKey = "a5159a76aae4a1b5ce3046b7d1b96469";
        private const string ApiToken = "db0fe8eea6005d411ae8013d3cc39c0e60ba5eb1753e03512b336a9f07b7639a";

        protected static RestClient client;

        [OneTimeSetUp]
        public static void InitializeRestClient() => client = new("https://api.trello.com");

        protected static RestRequest CreateRequest(string relativeUrl, Method httpMethod) => new(relativeUrl, httpMethod);

        protected static RestRequest CreateRequestAuth(string relativeUrl, Method httpMethod) =>
            CreateRequest(relativeUrl, httpMethod)
                .AddQueryParameter("key", ApiKey)
                .AddParameter("token", ApiToken);

        protected static void AssertResponseMatchesExpectedJsonSchema(RestResponse response, string expectedSchemaFileName)
        {
            var responseObject = JToken.Parse(response.Content);
            var expectedSchema = ReadSchemaFromJsonFile(expectedSchemaFileName);
            Assert.IsTrue(responseObject.IsValid(expectedSchema, out IList<string> errorMessages), $"response object:\n{responseObject}\n" +
                $"expected schema:\n{expectedSchema}\n{errorMessages.JoinByNewLine()}");
        }

        private static JSchema ReadSchemaFromJsonFile(string relativePath)
        {
            string fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), JsonSchemaNames.SchemasFolderName, relativePath);
            using JsonTextReader reader = new(File.OpenText(fullFilePath));
            return JSchema.Load(reader);
        }
    }
}
