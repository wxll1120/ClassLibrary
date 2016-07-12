using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ClassLibrary.Utility.SMS
{
    /// <summary>
    /// 华信5平台
    /// 您的验证码是@请不要把验证码泄露给其他人。如非本人操作，可不用理会！【信持云修】
    /// </summary>
    public class Sms
    {
        private static string SmsUrl = "http://114.113.154.5/sms.aspx";
        private static string SmsUserId = "";
        private static string SmsAccount = "xd001273";
        private static string SmsPassword = "xd001273asd123";

        /// <summary>
        /// 获取短信剩余信息
        /// </summary>
        public static SmsInfo GetSMSOver()
        {
            SmsInfo sms = new SmsInfo();
            Encoding myEncoding = Encoding.GetEncoding("UTF-8");
            string url = SmsUrl + "?action=overage&userid=" + SmsUserId + "&account=" + SmsAccount + "&password=" + SmsPassword;
            byte[] postBytes = Encoding.ASCII.GetBytes(url);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            myRequest.ContentLength = postBytes.Length;

            using (Stream reqStream = myRequest.GetRequestStream())
            {
                reqStream.Write(postBytes, 0, postBytes.Length);
            }
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            using (WebResponse wr = myRequest.GetResponse())
            {
                StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                StreamReader xmlStreamReader = sr;
                xmlDoc.Load(xmlStreamReader);
            }
            if (xmlDoc == null)
            {
                sms.status = "Faild";
                sms.message = "请求异常！";
            }
            else
            {
                string returnstatus = xmlDoc.GetElementsByTagName("returnstatus").Item(0).InnerText.ToString();
                string message = xmlDoc.GetElementsByTagName("message").Item(0).InnerText.ToString();
                string payinfo = xmlDoc.GetElementsByTagName("payinfo").Item(0).InnerText.ToString();
                string overage = xmlDoc.GetElementsByTagName("overage").Item(0).InnerText.ToString();
                string sendTotal = xmlDoc.GetElementsByTagName("sendTotal").Item(0).InnerText.ToString();
                sms.status = returnstatus;
                sms.message = message;
                sms.overnum = Convert.ToInt32(overage);
                sms.sendTotal = Convert.ToInt32(sendTotal);
            }
            return sms;
        }



        /// <summary>
        /// 发送短息
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">内容[eg:您的验证码是@请不要把验证码泄露给其他人。如非本人操作，可不用理会！【信持云修】]</param>
        /// <returns></returns>
        public static SmsInfo SendSms(string mobile, string content)
        {
            SmsInfo csms = new SmsInfo();
            csms = GetSMSOver();

            if (csms.overnum > 0)
            {
                Encoding myEncoding = Encoding.GetEncoding("UTF-8");
                string url = SmsUrl+"?action=send&userid=" + SmsUserId + "&account=" +SmsAccount + "&password=" + SmsPassword + "&mobile=" + mobile + "&content=" + content;
                byte[] postBytes = Encoding.ASCII.GetBytes(url);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                myRequest.ContentLength = postBytes.Length;

                using (Stream reqStream = myRequest.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                using (WebResponse wr = myRequest.GetResponse())
                {
                    StreamReader sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);
                    StreamReader xmlStreamReader = sr;
                    xmlDoc.Load(xmlStreamReader);
                }
                if (xmlDoc == null)
                {
                    csms.status = "Faild";
                    csms.message = "请求异常！";
                }
                else
                {
                    string returnstatus = xmlDoc.GetElementsByTagName("returnstatus").Item(0).InnerText.ToString();
                    string message = xmlDoc.GetElementsByTagName("message").Item(0).InnerText.ToString();
                    string remainpoint = xmlDoc.GetElementsByTagName("remainpoint").Item(0).InnerText.ToString();
                    string taskID = xmlDoc.GetElementsByTagName("taskID").Item(0).InnerText.ToString();
                    string successCounts = xmlDoc.GetElementsByTagName("successCounts").Item(0).InnerText.ToString();
                    csms.status = returnstatus;
                    //if (returnstatus == "Success")
                    //{
                    //    csms.message = "发送成功！";
                    //}
                    csms.message = message;
                    csms.overnum = Convert.ToInt32(remainpoint);
                    csms.successCounts = Convert.ToInt32(successCounts);

                }
            }
            else
            {
                csms.status = "Faild";
            }
            return csms;
        }



        #region 随机数
        public static int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
            }
            return arrNum;
        }
        public static int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    getNum(arrNum, tmp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                }
                n++;
            }
            return tmp;
        }
        public static string GetRandomCode(int num,int minValue,int maxValue)
        {
            int[] arr = getRandomNum(num, minValue, maxValue); //从1至20中取出6个互不相同的随机数
            int i = 0;
            string temp = "";
            while (i <= arr.Length - 1)
            {
                temp = arr[i].ToString();
                i++;
            }
            return temp;
        }
        #endregion
    }



    /// <summary>
    /// SMS Info
    /// </summary>
    public class SmsInfo
    {
        /// <summary>
        /// 返回状态 Faild / Success
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int overnum { get; set; }
        /// <summary>
        /// 已发送数量
        /// </summary>
        public int sendTotal { get; set; }
        /// <summary>
        /// 发送成功数量
        /// </summary>
        public int successCounts { get; set; }
    }
}
