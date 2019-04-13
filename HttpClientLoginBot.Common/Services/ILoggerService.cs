using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Common.Services
{
    public interface ILoggerService
    {
        void Log(string message);
        void LogAsync(string message);
    }
}
