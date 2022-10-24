using RestSharp;
using System.Collections;

namespace NunitTrelloTest.TestDataProviders
{
    public class AuthDataProvider : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] {Parameter.CreateParameter("key", "a5159a76aae4a1b5ce3046b7d1b96469", ParameterType.QueryString)};
            yield return new object[] {Parameter.CreateParameter("token", "db0fe8eea6005d411ae8013d3cc39c0e60ba5eb1753e03512b336a9f07b7639a", ParameterType.QueryString)};
            //yield return new object[] {Parameter.CreateParameter("", "", ParameterType.QueryString)};
        }

    }
}
