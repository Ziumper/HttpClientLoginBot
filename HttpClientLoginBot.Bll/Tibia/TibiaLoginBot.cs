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
        public TibiaLoginBot(string resultFileName, ILoginClient<LoginResult> loginClient, List<LoginData> loginDataList) : base(resultFileName, loginClient, loginDataList)
        {
          
        }

        
    
    }
}
