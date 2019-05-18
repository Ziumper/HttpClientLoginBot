using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia;
using HttpClientLoginBot.Bll.Tibia.Exceptions;
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
        private ProxyQueque proxyQueque = new ProxyQueque(new List<LoginProxy>());

        [TestMethod]
        public async Task Login_With_Correct_Credentials_To_Tibia_Account()
        {

            var client = new TibiaLoginClient(_url, proxyQueque);

            var result = await client.LoginAsync(_fakeCorrectLoginData);

            Assert.AreEqual(true, result.IsSuccess);
        }

        [TestMethod]
        public async Task Login_With_Wrong_Credential_To_Tibia_Account()
        {
            var client = new TibiaLoginClient(_url, proxyQueque);
            var result = await client.LoginAsync(_fakeWrongLoginData);

            Assert.AreEqual(false, result.IsSuccess);
        }

        /*Only wrong credentials generate block ip exception */
        [TestMethod]
        [ExpectedException(typeof(TibiaQuequeProxyEnd))]
        public async Task Login_Many_Times_To_Get_Tibia_Block_Ip_Exception()
        {

            try
            {
                var client = new TibiaLoginClient(_url, proxyQueque);
                for (var i = 0; i < 100; i++)
                {
                    var result = await client.LoginAsync(_fakeWrongLoginData);
                }
            }
            catch (TibiaQuequeProxyEnd e)
            {
                throw e;
            }

        }

    }
}