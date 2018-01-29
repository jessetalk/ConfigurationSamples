using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChangeNotification
{
    public class Config
    {

        private int changeTime = 0;
        private CancellationTokenSource _reloadToken = new CancellationTokenSource();
        private string privateValue;

        public string Value
        {

           get  { return privateValue;  }

            set { privateValue = value;
                var previousToken = Interlocked.Exchange(ref _reloadToken, new CancellationTokenSource());
                previousToken.Cancel();
            }
        }


        public CancellationTokenSource GetReloadToken() => _reloadToken;
        public CancellationTokenSource Watch()
        {
            return _reloadToken;
        }

        
     


    }
}
