using System.Collections.Generic;
using System.IO;

namespace Jessetalk.Extensions.Configuration.Redis.Parsers
{
    public interface IConfigurationParser
    {
        IDictionary<string, string> Parse(string json);
    }
}