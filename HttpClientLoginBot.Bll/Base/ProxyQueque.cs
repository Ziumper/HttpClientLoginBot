using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class ProxyQueque 
    {
        protected Queue<LoginProxy> _proxyQueque;
        public int? Count { get { return _proxyQueque?.Count; } }
        protected List<LoginProxy> _proxyList;


        public Boolean IsEnd
        {
            get
            {
                if(_proxyQueque == null)
                {
                    return true;
                }

                return _proxyQueque.Count == 0;
            }
        }
        
        public LoginProxy Proxy
        {
            get
            {
                var proxy = _proxyQueque.Dequeue();
                _proxyList.Add(proxy);
                return proxy;
            }
        }

        public ProxyQueque()
        {
            _proxyQueque = new Queue<LoginProxy>();
            _proxyList = new List<LoginProxy>();
        }

        public ProxyQueque(IEnumerable<LoginProxy> proxyQueque)
        {
            _proxyQueque = new Queue<LoginProxy>(proxyQueque);
            _proxyList = new List<LoginProxy>();
        }

        public void ResetProxyQueque()
        {
            var tempList = new List<LoginProxy>();
            if(_proxyList.Count > 0)
            {
                tempList = AddLastElementOfProxyList(tempList);
            }
            
            tempList = AddElementsOfProxyQueque(tempList);
            tempList = AddElementsOfProxyListWithoutLast(tempList);

            _proxyList = new List<LoginProxy>();
            _proxyQueque = new Queue<LoginProxy>(tempList);
        }

        private List<LoginProxy> AddLastElementOfProxyList(List<LoginProxy> tempList)
        {
            var last = _proxyList.Last();

            var isLastSet = last != null;
            if (isLastSet)
            {
                tempList.Add(last);
            }

            return tempList;
        }

        private List<LoginProxy> AddElementsOfProxyQueque(List<LoginProxy> tempList)
        {
            if (!IsEnd)
            {
                foreach (var proxy in _proxyQueque)
                {
                    tempList.Add(proxy);
                }
            }

            return tempList;
        }
        
        private List<LoginProxy> AddElementsOfProxyListWithoutLast(List<LoginProxy> tempList)
        {
            if(_proxyList.Count > 0)
            {
                for(var i = 0; i <= _proxyList.Count; i++)
                {
                    tempList.Add(_proxyList[i]);
                }
            }
            return tempList;
        }

    }
}
