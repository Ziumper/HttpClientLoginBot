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
        private string url = "https://www.tibia.com/account/?subtopic=accountmanagement";
        private string fakeUsername = "TestAccountForGoats";
        private string fakePassword = "TestAccountForGoats10Password";

        [TestMethod]
        public async Task Login_With_Correct_Credentials_To_Tibia_Account()
        {
            var credential = new TibiaLoginData();
            credential.Username = fakeUsername;
            credential.Password = fakePassword;

            var client = new TibiaLoginClient(url);

            var result = await client.Login(credential);

            Assert.AreEqual(true, result.IsSucces);
        }
    }
}
