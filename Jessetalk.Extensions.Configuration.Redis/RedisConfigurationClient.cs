using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using ServiceStack.Redis;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Jessetalk.Extensions.Configuration.Redis
{
    internal sealed class RedisConfigurationClient 
    {
        private readonly object _lastVersionLock = new object();
        private int _lastVersion = 1;
        private RedisConfigurationSource _source;
        private string _configValueVersionPostfix = "_Version";

        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        public RedisConfigurationClient(RedisConfigurationSource source)
        {
            _source = source;
        }

        public async Task<ConfigQueryResult> GetConfig()
        {
            var result = await GetConfigValue();
            UpdateLastVersion(result);
            return result;
        }

        public IChangeToken Watch(Action<RedisWatchExceptionContext> onException)
        {
            Task.Run(() => PollForChanges(onException));
            return _reloadToken;
        }

        private async Task PollForChanges(Action<RedisWatchExceptionContext> onException)
        {
            while (!_source.CancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (await HasValueChanged())
                    {
                        var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
                        previousToken.OnReload();
                        return;
                    }
                }
                catch (Exception exception)
                {
                    var exceptionContext = new RedisWatchExceptionContext(_source, exception);
                    onException?.Invoke(exceptionContext);
                }

                await Task.Delay(2000);
            }
        }

        private async Task<bool> HasValueChanged()
        {
            ConfigQueryResult queryResult;
            queryResult = await GetConfigValue();
            return queryResult != null && UpdateLastVersion(queryResult);           
        }

        private async Task<ConfigQueryResult> GetConfigValue() => await Task.Run(() =>
        {
            var manager = new RedisManagerPool(_source.Server);
            using (var client = manager.GetClient())
            {
                var result = new ConfigQueryResult();

                if (!client.ContainsKey(_source.Key))
                {
                    result.Exists = false;
                    return result;
                }

                result.Exists = true;

                result.Value = client.Get<string>(_source.Key);
                result.Version = client.Get<int>($"{_source.Key}{_configValueVersionPostfix}");
                return result;
            }
        });

        private bool UpdateLastVersion(ConfigQueryResult queryResult)
        {
            lock (_lastVersionLock)
            {
                if (queryResult.Version > _lastVersion)
                {
                    _lastVersion = queryResult.Version;
                    return true;
                }
            }

            return false;
        }


    }
}
