using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NunitTrelloTest.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NunitTrelloTest.Tests
{
    public class BaseTest
    {
        private const string ApiKey = "a5159a76aae4a1b5ce3046b7d1b96469";
        private const string ApiToken = "db0fe8eea6005d411ae8013d3cc39c0e60ba5eb1753e03512b336a9f07b7639a";
        
        private static ExtentReports reportHolder;
        private static ExtentTest testClassHolder;
        protected static RestClient client;
        protected ExtentTest testMethodHolder;

        [OneTimeSetUp]
        public static void InitializeRestClientAndReportHolder()
        {
            if (reportHolder == null)
            {
                reportHolder = new ExtentReports();
                reportHolder.AttachReporter(new ExtentHtmlReporter("index.html"));
            }
            testClassHolder = reportHolder.CreateTest(TestContext.CurrentContext.Test.ClassName);//create test class - left table
            client = new("https://api.trello.com");
        }

        [SetUp]
        public void SetUpTestMetadataHolderBeforeEachTest()
        {
            var testMethodName = TestContext.CurrentContext.Test.Name;
            var currentTestMethod = GetType().GetMethods().First(method => testMethodName.StartsWith(method.Name));
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute) Attribute.GetCustomAttribute(currentTestMethod, typeof(DescriptionAttribute));
            testMethodHolder = testClassHolder.CreateNode(descriptionAttribute.Properties.Get("Description").ToString());
            AuthorAttribute authorAttribute = (AuthorAttribute) Attribute.GetCustomAttribute(currentTestMethod, typeof(AuthorAttribute));
            testMethodHolder.AssignAuthor(authorAttribute.Properties.Get("Author").ToString());
        }

        [TearDown]
        public void WriteTestExecutionResults()
        {
            var testResult = TestContext.CurrentContext.Result;
            if (testResult.Outcome.Status == TestStatus.Failed) {
                testMethodHolder.Fail(testResult.StackTrace);
            }
        }

        [OneTimeTearDown]
        public static void CreateReport()
        {
            reportHolder.Flush();
        }

        protected static RestRequest CreateRequest(string relativeUrl, Method httpMethod) => new(relativeUrl, httpMethod);

        protected static RestRequest CreateRequestAuth(string relativeUrl, Method httpMethod) =>
            CreateRequest(relativeUrl, httpMethod)
                .AddQueryParameter("key", ApiKey)
                .AddParameter("token", ApiToken);

        protected void AssertResponseMatchesExpectedJsonSchema(RestResponse response, string expectedSchemaFileName)
        {
            var responseObject = JToken.Parse(response.Content);
            var expectedSchema = ReadSchemaFromJsonFile(expectedSchemaFileName);
            testMethodHolder.Debug($"Check response '{responseObject}' matches expected schema '{expectedSchema}'");
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
