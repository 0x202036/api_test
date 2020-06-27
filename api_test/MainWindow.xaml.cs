using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

namespace api_test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            var send = new Send(TbUser.Text,TbKey.Text,TbSendWord.Text,Convert.ToInt32(TbLevel.Text));
            var sendJson = JsonConvert.SerializeObject(send);
            TbSendJson.Text = sendJson;
            try
            {
                var receiveJson = SentData(TbServer.Text, sendJson);
                var Receive = JsonConvert.DeserializeObject<Receive>(receiveJson);
                TbReceiveWord.Text = Receive.Word;
                TbCode.Text = Receive.StatueCode.ToString();
                TbDescription.Text = Receive.Translate;
                TbSentenceCount.Text = Receive.Sentences.Count.ToString();
                TbReceiveJson.Text = receiveJson;
                if (Receive.Sentences.Count != 0)
                {
                    LvSentence.DataContext = Receive.Sentences;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private static string SentData(string url,string sendJson)
        {
            string result = "";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.KeepAlive = true;
            webRequest.ContentType = "application/json";
            webRequest.Accept = "*/*";
            webRequest.Method = "POST";
            byte[] data = Encoding.UTF8.GetBytes(sendJson);
            webRequest.ContentLength = data.Length;
            Stream writer = webRequest.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
            var response = (HttpWebResponse) webRequest.GetResponse();
            result= new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();
            response.Close();
            return result;
        }
    }
}
