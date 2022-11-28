using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NunitTrelloTest.TestDataProviders;
using RestSharp;
using System.Net;

namespace NunitTrelloTest.Tests.Boards
{
    public class GetABoardTests: BaseTest
    {
        [Test]
        [Author("Sergey Polushin")]
        //[Igonre("Bug: JIRA_ID-123")]
        [Description("Validate Happy Path GetABoard")]
        public void ValidateHappyPathGetABoard()
        {
            testMethodHolder.Debug($"Prepare '{nameof(EndPoints.GetBoardsThatMemberBelongsToUrl)}' request");
            var serviceRequest = CreateRequestAuth(EndPoints.GetBoardsThatMemberBelongsToUrl, Method.Get).
                AddQueryParameter("fields","id");
            testMethodHolder.Debug($"Send '{nameof(EndPoints.GetBoardsThatMemberBelongsToUrl)}' '{serviceRequest.Resource}' request");
            var serviceResponse = client.Execute(serviceRequest);
            testMethodHolder.Debug($"Parse response object '{serviceResponse.Content}' to JToken");
            var responseObjects = JToken.Parse(serviceResponse.Content);
            var idValue = responseObjects.SelectToken("[0].id").ToString();
            var request = CreateRequestAuth(EndPoints.GetABoardUrl, Method.Get)
                .AddQueryParameter("fields", "id,name")
                .AddUrlSegment("id", idValue);
            var response = client.Execute(request);
            testMethodHolder.Debug($"Check that actual status code '{response.StatusCode}' is equal to expected '{HttpStatusCode.OK}'");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetABoardJsonSchemaFileName);
        }

        
        [Test]
        [TestCaseSource(typeof(AuthDataProvider))]
        [Author("Sergey Polushin")]
        [Description("Validate GetABoard  is Secured")]
        public void ValidateGetABoardEndpointIsSecured(Parameter queryParameter)
        {
            testMethodHolder.Debug("Prepare GetABoard request");
            var request = CreateRequest(EndPoints.GetABoardUrl, Method.Get)
                .AddQueryParameter("fields", "id,name")
                .AddUrlSegment("id", "615c63af1029d92fe7031865")
                .AddParameter(queryParameter);
            testMethodHolder.Debug($"Send GetABoard '{request.Resource}' request");
            var response = client.Execute(request);
            testMethodHolder.Debug($"Check that actual status code '{response.StatusCode}' is equal to expected '{HttpStatusCode.Unauthorized}'");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual("unauthorized permission requested", response.Content.ToString());
        }
    }
}
