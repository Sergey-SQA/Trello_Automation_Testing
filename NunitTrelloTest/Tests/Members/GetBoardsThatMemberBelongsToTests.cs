using NUnit.Framework;
using RestSharp;
using System.Net;

namespace NunitTrelloTest.Tests.Members
{
    public class GetBoardsThatMemberBelongsToTests: BaseTest
    {
        [Test]
        public void ValidateHappyPathGetBoardsThatMemberBelongsTo()
        {
            var request = CreateRequestAuth(EndPoints.GetAllBoardsThatUserBelongsToUrl, Method.Get);
            request.AddQueryParameter("fields", "id,name,closed");
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetAllBoardsJsonSchemaFileName);
        }

    }
}