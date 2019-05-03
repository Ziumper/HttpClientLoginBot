using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Common
{
    public class DataLoaderService
    {
        public async Task<List<LoginProxy>> LoadProxyList(string pathToFile)
        {
            var result = new List<LoginProxy>();
            using (var reader = new StreamReader(pathToFile))
            {
                while (!reader.EndOfStream)
                {
                    var line =  await reader.ReadLineAsync();
                    var lineResult = line.Split(':');
                    var host = lineResult[0];
                    var port = lineResult[1];
                    var loginProxy = new LoginProxy(host, port);
                    result.Add(loginProxy);
                }
            }

            return result;
        }

        public async Task<List<TibiaLoginData>> LoadTibiaLoginData(string pathToFile)
        {
            var result = new List<TibiaLoginData>();

            using (var reader = new StreamReader(pathToFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var lineResult = line.Split(':');
                    var username = lineResult[0];
                    var password = lineResult[1];
                    var tibiaLoginData = new TibiaLoginData();
                    tibiaLoginData.Username = username;
                    tibiaLoginData.Password = password;
                    result.Add(tibiaLoginData);
                }
            }


            return result;
        }
    }
}
