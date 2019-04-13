using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpClientLoginBot.Common.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string _loggerPathfilename;

        public LoggerService(string loggerPathFilename)
        {
            _loggerPathfilename = loggerPathFilename;
        }

        public async void LogAsync(string message)
        {
            var nowTime = DateTime.Now.ToShortDateString();
            var datePrefixMessage = nowTime + ": ";

            using(StreamWriter outPutFile = new StreamWriter(_loggerPathfilename))
            {
                var resultLineToSave = datePrefixMessage + message;
                await outPutFile.WriteLineAsync(resultLineToSave);
            }
        }

        public void Log(string message)
        {
            var nowTime = DateTime.Now.ToShortDateString();
            var datePrefixMessage = nowTime + ": ";

            using (StreamWriter outPutFile = new StreamWriter(_loggerPathfilename))
            {
                var resultLineToSave = datePrefixMessage + message;
                outPutFile.WriteLine(resultLineToSave);
            }
        }


    }
}
