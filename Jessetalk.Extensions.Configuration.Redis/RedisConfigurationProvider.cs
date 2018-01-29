using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Jessetalk.Extensions.Configuration.Redis
{
    internal sealed class RedisConfigurationProvider:ConfigurationProvider
    {
        private readonly RedisConfigurationSource _source;
        private readonly RedisConfigurationClient _redisConfigurationClient;
        private readonly object _reloadLock = new object();

        public RedisConfigurationProvider(RedisConfigurationSource source, RedisConfigurationClient client)
        {
            _source = source;
            _redisConfigurationClient = client;

            ChangeToken.OnChange(
                 () => _redisConfigurationClient.Watch(_source.OnWatchException),
                 async () =>
                 {
                      await DoLoad(reloading: true);
                      OnReload();
                     _source.OnReload?.Invoke();
                 });
        }


        public override void Load()
        {
            DoLoad(false).Wait();
        }

        private async Task DoLoad(bool reloading)
        {
            var configQueryResult = await _redisConfigurationClient.GetConfig();
            if (!configQueryResult.Exists && !_source.Optional)
            {
                if (!reloading)
                {
                    throw new Exception($"在Redis中没有找到对应的key {_source.Key} ");
                }
                else
                {
                    return;
                }
            }

            
            LoadIntoMemory(configQueryResult);
        }

        private void LoadIntoMemory(ConfigQueryResult configQueryResult)
        {
            if (!configQueryResult.Exists)
            {
                Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return;
            }
            else
            {        
                IDictionary<string, string> parsedData = _source.Parser.Parse(configQueryResult.Value);
                Data = new Dictionary<string, string>(parsedData, StringComparer.OrdinalIgnoreCase);   
            }
        }

    }
}
