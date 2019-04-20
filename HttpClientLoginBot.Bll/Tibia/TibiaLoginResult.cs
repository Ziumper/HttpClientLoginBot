using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginResult : LoginResult
    {
        public bool IsBlockIpError { get; set; }

        public TibiaLoginResult(LoginResult result)
        {
            IsFinished = result.IsFinished;
            IsSucces = result.IsSucces;
            Message = result.Message;
            Response = result.Response;
            IsBlockIpError = false;
        }
    }
}
