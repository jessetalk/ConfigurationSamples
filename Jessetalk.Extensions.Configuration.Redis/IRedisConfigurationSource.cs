using Jessetalk.Extensions.Configuration.Redis.Parsers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Jessetalk.Extensions.Configuration.Redis
{
    public interface IRedisConfigurationSource:IConfigurationSource
    {
        string Key { get; }

        /// <summary>
        /// 是否可选，如果上必选，当找不到对应的key时，将会抛出异常
        /// </summary>
        bool Optional { get; set; }

        string Server { get; set; }

        CancellationToken CancellationToken { get; set; }

        Action<RedisWatchExceptionContext> OnWatchException { get; set; }

        Action OnReload { get; set; }

        IConfigurationParser Parser { get; set; }
    }
}
