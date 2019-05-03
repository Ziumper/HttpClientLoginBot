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
                DataLoaderService dataLoaderService = new DataLoaderService();
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
                DataLoaderService dataLoaderService = new DataLoaderService();
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
        }
    }
}
