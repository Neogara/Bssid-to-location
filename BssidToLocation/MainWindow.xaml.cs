using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Xml;

namespace BssidToLocation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Uri uri = new Uri(@"https://yandex.ru/maps/");
            Web.Source = uri;
        }

        private void FindLocation_Click(object sender, RoutedEventArgs e)
        {
            string bssid = BssidText.Text.Trim();
            bssid = bssid.Replace("-", "");

            var url =
            "http://mobile.maps.yandex.net/cellid_location/?clid=1866854&lac=-1&cellid=-1&operatorid=null&countrycode=null&signalstrength=-1&wifinetworks=" +
            bssid +
            ":-65&app=ymetro";
            
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            try
            {
                var strJson = wc.DownloadString(url);
                var doc = new XmlDocument();
                doc.Load(url);
                LocationText.Text = doc.LastChild.FirstChild.Attributes[0].Value + " " + doc.LastChild.FirstChild.Attributes[1].Value;
            }
            catch (WebException ex)
            {
                MessageBox.Show("Something went wrong :( "+ex.Message+" )");
            }
        }
    }
}
