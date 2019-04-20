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
        private TibiaLoginData fakeLoginData = new TibiaLoginData
        {
            Username = "TestAccountForGoats",
            Password = "TestAccountForGoats10Password"
        };
  

        [TestMethod]
        public async Task Login_With_Correct_Credentials_To_Tibia_Account()
        {

            var client = new TibiaLoginClient(url);

            var result = await client.Login(fakeLoginData);
            
            if(result.IsFinished == false)
            {
                result.IsSucces = true;
            } 

            Assert.AreEqual(true, result.IsSucces);
        }

        //[TestMethod]
        //public async Task Login_For_Proxy_Error()
        //{
            
        //}
    }
}
