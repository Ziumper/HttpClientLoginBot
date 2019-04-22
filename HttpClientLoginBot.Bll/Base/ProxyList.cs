using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class ProxyList
    {
        protected readonly List<LoginProxy> _proxyList;
        protected int _currentProxyIndex;

        public bool IsEndOfProxyList
        {
            get { return _currentProxyIndex == -1; }
        }

        public LoginProxy CurrentProxy
        {
            get
            {
                if(_currentProxyIndex == -1)
                {
                    return null;
                }
                return _proxyList[_currentProxyIndex];
            }
        }

        public LoginProxy NextProxy
        {
            get
            {
                _currentProxyIndex++;
               if(_currentProxyIndex >= _proxyList.Count)
               {
                    _currentProxyIndex = -1;
                    return null;
               } 

               return _proxyList[_currentProxyIndex];
            }
        }

        public ProxyList(List<LoginProxy> proxyList)
        {
            _proxyList = proxyList;
            _currentProxyIndex = 0;
        }

        

    }
}
