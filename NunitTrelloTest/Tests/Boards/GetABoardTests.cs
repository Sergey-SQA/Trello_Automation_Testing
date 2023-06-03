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
        public void ValidateHappyPathGetABoard()
        {
            //var serviceRequest = CreateRequestAuth(EndPoints.GetAllBoardsThatUserBelongsToUrl, Method.Get);
            //var serviceResponse = client.Execute(serviceRequest);
            //var responseObject = JToken.Parse(serviceResponse.Content);
            //var Id = responseObject.Children();

            var request = CreateRequestAuth(EndPoints.GetABoardUrl, Method.Get)
                .AddQueryParameter("fields", "id,name")
                .AddUrlSegment("id", "615c63af1029d92fe7031865");
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetABoardJsonSchemaFileName);
        }

        [TestCaseSource(typeof(AuthDataProvider))]
        [Test]
        public void ValidateGetABoardEndpointIsSecured(Parameter queryParameter)
        {
            var request = CreateRequest(EndPoints.GetABoardUrl, Method.Get)
                .AddQueryParameter("fields", "id,name")
                .AddUrlSegment("id", "615c63af1029d92fe7031865")
                .AddParameter(queryParameter);
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual("invalid key", response.Content.ToString());
        }
    }
}
