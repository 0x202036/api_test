﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            try
            {
                var sendJson = GetInfo();
                TbSendJson.Text = sendJson;
                try
                {
                    var startTime = DateTime.Now;
                    string receiveJson = "";
                    var receive = ConnectServer(sendJson,out receiveJson);
                    FillBox(receive,receiveJson,startTime);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Receive ConnectServer(string sendJson,out string receiveJson)
        {
            receiveJson = SentData(TbServer.Text, sendJson);
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<Receive>(receiveJson, setting);
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
            result = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            response.Close();
            return Decode(result);
        }

        private static string Decode(string str)
        {
            var regex = new Regex(@"\\u(\w{4})");

            string result = regex.Replace(str, delegate (Match m)
            {
                string hexStr = m.Groups[1].Value;
                string charStr = ((char)int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber)).ToString();
                return charStr;
            });

            return result;
        }

        private string GetInfo()
        {
            var send = new Send(TbUser.Text, TbKey.Text, TbSendWord.Text, Convert.ToInt32(TbLevel.Text), Convert.ToInt32(TbCount.Text));
            return JsonConvert.SerializeObject(send);
        }

        private void FillBox(Receive receive,string receiveJson,DateTime startTime)
        {
            LbTime.Content = (DateTime.Now - startTime).TotalMilliseconds.ToString("f0");
            TbReceiveWord.Text = receive.Word;
            TbCode.Text = receive.StatueCode.ToString();
            TbDescription.Text = receive.Translate;
            TbSentenceCount.Text = receive.Sentences.Count.ToString();
            TbReceiveJson.Text = receiveJson;
            if (receive.Sentences.Count != 0)
            {
                LvSentence.ItemsSource = receive.Sentences;
            }
            else
            {
                LvSentence.ItemsSource = new List<Sentence>();
            }
            LvSentence.UpdateLayout();
        }
    }
}
