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
            var request = CreateRequestAuth(EndPoints.GetAllBoardsThatUserBelongsToUrl, Method.Get)
                .AddQueryParameter("fields", "id,name,closed");
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.GetAllBoardsJsonSchemaFileName);
        }

        [Test]
        public void ValidateGetBoardsThatMemberBelongsToEndpointIsSecured()
        {
            var request = CreateRequest(EndPoints.GetAllBoardsThatUserBelongsToUrl, Method.Get)
                .AddQueryParameter("fields", "id,name,closed");
            var response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            AssertResponseMatchesExpectedJsonSchema(response, JsonSchemaNames.EmptyArrayJsonSchemaFileName);
        }

    }
}