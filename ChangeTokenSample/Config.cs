using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChangeTokenSample
{
    public class Config
    {

        private int changeTime = 0;
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();
        private string privateValue;

        public string Value
        {

           get  { return privateValue;  }

            set { privateValue = value;
                var previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
                previousToken.OnReload();
            }
        }


        public ConfigurationReloadToken GetReloadToken() => _reloadToken;


        public void Sync()
        {
            privateValue = Value;
        }


        public ConfigurationReloadToken Watch()
        {
            return _reloadToken;
        }

        
     


    }
}
