using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Tests
{

    [TestClass]
    public class TibiaLoginClientTests
    {
        private string _url = "https://www.tibia.com/account/?subtopic=accountmanagement";
        private TibiaLoginData _fakeCorrectLoginData = new TibiaLoginData
        {
            Username = "TestAccountForGoats",
            Password = "TestAccountForGoats10Password"
        };
        private TibiaLoginData _fakeWrongLoginData = new TibiaLoginData
        {
            Username = "TestLogin",
            Password = "TestPassword"
        };
        private LoginProxy _httpLoginProxy = new LoginProxy("35.235.75.244", "3128");
        private LoginProxy _httpsLoginProxy = new LoginProxy("81.15.197.82", "31280");

        [TestMethod]
        public async Task Login_With_Correct_Credentials_To_Tibia_Account()
        {

            var client = new TibiaLoginClient(_url);

            var result = await client.Login(_fakeCorrectLoginData);

            Assert.AreEqual(true, result.IsSucces);
        }

        [TestMethod]
        public async Task Login_With_Wrong_Credential_To_Tibia_Account()
        {
            var client = new TibiaLoginClient(_url);
            var result = await client.Login(_fakeWrongLoginData);
       
            Assert.AreEqual(false, result.IsSucces);
        }

        [TestMethod]
        public async Task Login_With_Http_Proxy_With_Correct_Credentials_To_Tibia_Account()
        {
            var client = new TibiaLoginClient(_url);

            client.ActiveProxy = _httpLoginProxy;
            var result = await client.Login(_fakeCorrectLoginData);

            Assert.AreEqual(true, result.IsSucces);
        }

        [TestMethod] 
        public async Task Login_With_Https_Proxy_With_Correct_Credentials_To_Tibia_Account()
        {
            var client = new TibiaLoginClient(_url);

            client.ActiveProxy = _httpsLoginProxy;
            var result = await client.Login(_fakeCorrectLoginData);

            Assert.AreEqual(true, result.IsSucces);
        }

   
        
    }
}
