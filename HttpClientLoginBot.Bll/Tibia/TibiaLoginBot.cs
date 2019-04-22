using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginBot : LoginBot,ILoginBot
    {
        public TibiaLoginBot(string resultFileName, ILoginClient<LoginResult> loginClient, List<LoginData> loginDataList, ProxyList proxyList) : base(resultFileName, loginClient, loginDataList, proxyList)
        {
            UseProxy = true;
        }

        public async override void Run()
        {
          foreach(var data in _loginDataList) {
                var result = new LoginResult();
                result.IsFinished = false;
                while(!result.IsFinished)
                {
                    try
                    {
                        result = await _loginClient.Login(data);
                    }
                    catch (HttpRequestException e)
                    {
                        if (UseProxy)
                        {
                            SetProxy();
                        }
                        else
                        {
                            throw e;
                        }
                    }
                    catch (TibiaBlockIpException e)
                    {
                        if (UseProxy)
                        {
                            if(_proxyList.IsEndOfProxyList)
                            {
                                result.IsFinished = true;
                            } else
                            {
                                SetProxy();
                            }
                            
                        }
                        else
                        {
                            throw e;
                        }


                    }
                }
               
                
          }
        }

    
    }
}
