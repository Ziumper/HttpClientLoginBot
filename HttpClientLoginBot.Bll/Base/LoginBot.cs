using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public abstract class LoginBot : ILoginBot {
        protected readonly string _pathToCredentials;
        protected readonly string _pathoToProxyFileList;
        protected readonly string _url;
        protected List<LoginProxy> _proxyList;
        protected List<LoginCredential> _credentials;
        protected string _resultFileName;

        public List<LoginProxy> ProxyList {
            get {
                return _proxyList;
            }
        }

        public List<LoginCredential> Credentials {
            get {
                return _credentials;
            }
        }

        public LoginBot (
            string pathToCredentials,
            string pathToProxyFileList,
            string resultFileName,
            string url
        ) {
            _pathoToProxyFileList = pathToProxyFileList;
            _pathToCredentials = pathToCredentials;
            _resultFileName = resultFileName;
            _url = url;
        }

        public virtual void LoadCredentials (string seperator) {
            var credentialsList = new List<LoginCredential> ();
            using (var reader = new StreamReader (_pathToCredentials)) {
                var line = reader.ReadLine ();
                var lineSplitResult = line.Split (seperator);
                var username = lineSplitResult[0];
                var password = lineSplitResult[1];
                var credential = new LoginCredential (username, password);
                
                credential.RequestBody = username + password;
                credential.Url = _url;

                credentialsList.Add (credential);
            }
            _credentials = credentialsList;
        }

        public virtual void LoadProxyList (string seperator) {
            var proxyList = new List<LoginProxy> ();
            using (var reader = new StreamReader (_pathoToProxyFileList)) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine ();
                    var lineSplitResult = line.Split (seperator);
                    var host = lineSplitResult[0];
                    var port = lineSplitResult[1];
                    var proxy = new LoginProxy (host, port);
                    proxyList.Add (proxy);
                }
            }

            _proxyList = proxyList;
        }

        public async void Run () {
            foreach (var credential in _credentials) {
                var client = new LoginClient ();
                var result = await client.Login (credential);
            }

        }
    }
}