using System;

namespace Jbe.NewsReader.Presentation.Converters
{
    internal static class ConverterHelper
    {
        public static bool IsParameterSet(string expectedParameter, object actualParameter)
        {
            string parameter = actualParameter as string;
            return string.Equals(parameter, expectedParameter, StringComparison.OrdinalIgnoreCase);
        }
    }
}
