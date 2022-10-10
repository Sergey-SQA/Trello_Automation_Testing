using System;
using System.Collections.Generic;

namespace NunitTrelloTest.Utils
{
    public static class StringListUtils
    {
        public static string JoinByNewLine(this IList<string> stringsList)
        {
            return string.Join(Environment.NewLine, stringsList);
        }
    }
}
