using NUnit.Framework;
using RestSharp;
using System.Net;

namespace NunitTrelloTest.Tests.Boards
{
    public class GetABoard: BaseTest
    {
        [Test]
        public void ValidateHappyPathGetABoard()
        {
            var request = CreateRequestAuth(EndPoints.GetABoardUrl, Method.Get);
            request.AddQueryParameter("fields", "id,name")
                .AddUrlSegment("id", "615c63af1029d92fe7031865");
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetABoardJsonSchemaFileName);
        }
    }
}
