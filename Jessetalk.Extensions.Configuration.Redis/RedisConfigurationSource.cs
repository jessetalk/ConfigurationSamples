using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Threading;
using Jessetalk.Extensions.Configuration.Redis.Parsers;

namespace Jessetalk.Extensions.Configuration.Redis
{
    internal sealed class RedisConfigurationSource :IRedisConfigurationSource
    {
        public RedisConfigurationSource(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            CancellationToken = cancellationToken;
            Parser = new Json.JsonConfigurationFileParser();
        }

        public string Key { get; }

        /// <summary>
        /// 是否可选，如果非必须，当找不到对应的key时，将会抛出异常
        /// </summary>
        public bool Optional { get; set; }

        public string Server { get; set; }

        public CancellationToken CancellationToken { get; set; }

        public Action<RedisWatchExceptionContext> OnWatchException { get; set;}

        public Action OnReload { get; set; }

        public IConfigurationParser Parser { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var client = new RedisConfigurationClient(this);
            return new RedisConfigurationProvider(this, client);
        }
    }
}
