using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia;
using HttpClientLoginBot.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HttpClientLoginBot.Desktop
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnLoadProxy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? canOpen = ofd.ShowDialog();
            if (canOpen == true)
            {
                DataService dataLoaderService = new DataService();
                txtPathToProxyFile.Text = ofd.FileName;
                var result = await dataLoaderService.LoadProxyList(ofd.FileName);
                ObservableCollection<LoginProxy> collection = new ObservableCollection<LoginProxy>(result);
                DGProxy.ItemsSource = collection;
            }
        }

        private async void BtnLoadCredentials_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            bool? canOpen = ofd.ShowDialog();
            if (canOpen == true)
            {
                txtPathToCredentials.Text = ofd.FileName;
                DataService dataLoaderService = new DataService();
                var result = await dataLoaderService.LoadTibiaLoginData(ofd.FileName);
                ObservableCollection<TibiaLoginData> collection = new ObservableCollection<TibiaLoginData>(result);
                DGCredentials.ItemsSource = collection;
            }
        }

        private async void BtnTestProxy_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://www.tibia.com/account/?subtopic=accountmanagement";
            ProxyQueque proxyQueque = new ProxyQueque(DGProxy.ItemsSource.OfType<LoginProxy>());
            var proxyList = DGProxy.ItemsSource.OfType<LoginProxy>();
            TibiaLoginClient tibiaLoginClient = new TibiaLoginClient(url, proxyQueque);
            tibiaLoginClient.UseProxy = true;
            var timeout = double.Parse(txtTimeout.Text);
            tibiaLoginClient.TimeoutTime = timeout;
            var loginData = new TibiaLoginData();

            var amount = proxyList.Count();
            var current = 0;


            //correct credentials for prevent blockip error
            loginData.Username = "TestAccountForGoats";
            loginData.Password = "TestAccountForGoats10Password";

            List<LoginProxy> goodProxyList = new List<LoginProxy>();
            foreach(var proxy in proxyList)
            {
                try
                {
                    current++;
                    PBLoading.Value = (double) (100 * current) / amount;

                    var result = await tibiaLoginClient.LoginAsync(loginData,proxy);
                    if (result.IsSucces)
                    {
                        goodProxyList.Add(proxy);
                    }
                } catch (Exception)
                {
                    //do nothing
                }
                
            }

            DGProxy.ItemsSource = new ObservableCollection<LoginProxy>(goodProxyList);
            MessageBox.Show("Proxy Tested");
            PBLoading.Value = 0;
        }

        private async void BtnSaveProxy_Click(object sender, RoutedEventArgs e)
        {
            var proxyList = DGProxy.ItemsSource;
            var proxyFileNameToSave = txtPathToProxyFile.Text;
            DataService dataService = new DataService();
            await dataService.SaveProxyList(proxyFileNameToSave,proxyList.OfType<LoginProxy>());
            MessageBox.Show("Proxy list saved");
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ProxyQueque proxyQueque = new ProxyQueque();
            string url = "https://www.tibia.com/account/?subtopic=accountmanagement";
            if(DGProxy.AlternationCount > 0)
            {
                IEnumerable<LoginProxy> listOfProxy = DGProxy.ItemsSource.OfType<LoginProxy>();
                proxyQueque = new ProxyQueque(listOfProxy);
            }
            
          
            TibiaLoginClient tibiaLoginClient = new TibiaLoginClient(url, proxyQueque);
            List<TibiaLoginData> loginDataList = new List<TibiaLoginData>(DGCredentials.ItemsSource.OfType<TibiaLoginData>());
            PBLoading.Value = 0;
            int current = 0;
            int amount = loginDataList.Count;
            foreach(var data in loginDataList)
            {
                current++;
                PBLoading.Value = (double)(100 * current) / amount;
                try
                {
                    var result = await tibiaLoginClient.LoginAsync(data);
                    ResultsDG.Items.Add(result);
                }catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                
                
            }

            PBLoading.Value = 0;
        }

        private async void BtnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            var dataService = new DataService();
            var results = DGProxy.ItemsSource.OfType<TibiaLoginResult>();
            await dataService.SaveResults(txtParhToResults.Text,results);
            MessageBox.Show("Results saved in "+ txtParhToResults.Text);
        }
    }
}
