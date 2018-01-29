using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChangeNotification
{
    public class ConfigReloadToken 
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public bool HasChanged => _cts.IsCancellationRequested;

        public bool ActiveChangeCallbacks => true;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return _cts.Token.Register(callback, state);
        }

        public void OnReload() => _cts.Cancel();
    }
}
