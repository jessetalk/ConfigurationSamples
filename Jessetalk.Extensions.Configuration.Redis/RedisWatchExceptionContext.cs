using System;

namespace Jessetalk.Extensions.Configuration.Redis
{
    public sealed class RedisWatchExceptionContext
    {
        internal RedisWatchExceptionContext(IRedisConfigurationSource source, Exception exception)
        {
            Source = source;
            Exception = exception;
        }

        public IRedisConfigurationSource Source { get; }

        public Exception Exception { get; }


    }
}