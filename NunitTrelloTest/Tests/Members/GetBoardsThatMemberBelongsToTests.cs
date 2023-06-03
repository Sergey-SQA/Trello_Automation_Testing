using NUnit.Framework;
using NunitTrelloTest.TestDataProviders;
using RestSharp;
using System.Net;

namespace NunitTrelloTest.Tests.Members
{
    public class GetBoardsThatMemberBelongsToTests: BaseTest
    {
        [Test]
        [Author("Sergey Polushin")]
        [Description("Validate Happy Path GetBoardsThatMemberBelongsTo")]
        public void ValidateHappyPathGetBoardsThatMemberBelongsTo()
        {
            testMethodHolder.Debug("Prepare Rest request for GetBoardsThatMemberBelongsTo");
            var request = CreateRequestAuth(EndPoints.GetBoardsThatMemberBelongsToUrl, Method.Get)
                .AddQueryParameter("fields", "id,name,closed");
            testMethodHolder.Debug("Execute REST request");
            var response = client.Execute(request);
            testMethodHolder.Debug("Assert expected status code");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            testMethodHolder.Debug("Assert JSON schema");
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetAllBoardsJsonSchemaFileName);
        }

        [Test]
        [Author("Sergey Polushin")]
        [Description("Validate GetBoardsThatMemberBelongsTo Is Secured")]
        [TestCaseSource(typeof(AuthDataProvider))]
        public void ValidateGetBoardsThatMemberBelongsToEndpointIsSecured(Parameter queryParameter)
        {
            testMethodHolder.Debug("Prepare Rest request for GetBoardsThatMemberBelongsTo");
            var request = CreateRequest(EndPoints.GetBoardsThatMemberBelongsToUrl, Method.Get)
                .AddQueryParameter("fields", "id,name,closed")
                .AddParameter(queryParameter);
            testMethodHolder.Debug("Execute REST request");
            var response = client.Execute(request);
            testMethodHolder.Debug("Assert expected status code");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            testMethodHolder.Debug("Assert JSON schema");
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.EmptyArrayJsonSchemaFileName);
        }

    }
}