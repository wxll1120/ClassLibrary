using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibrary.Utility.Common
{
    public class CommonUtil
    {
        /// <summary>
        /// 过滤输入（移除首尾空格）
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string FilterInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Trim();
        }
        /// <summary>
        /// 检查字符串，如果字符串为NULL，则返回空串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CheckText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input;
        }

        /// <summary>
        /// 获取文件拓展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExtension(string fileName)
        {
            int index = fileName.LastIndexOf(".");

            if (index < 0)
                return string.Empty;

            return fileName.Substring(index);
        }

        /// <summary>
        /// 将二进制的文件数据以指定完全限定路径进行保存
        /// 如果目标文件已经存在，则覆盖原有文件
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <param name="data">文件二进制数据</param>
        public static void SaveFile(string filePath, byte[] data)
        {
            string directory = filePath.Substring(0, filePath.LastIndexOf('\\'));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (FileStream fileStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length); fileStream.Flush();
            }
        }


        public static void SaveFile(string filePath, string content)
        {
            string directory = filePath.Substring(0, filePath.LastIndexOf('\\'));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (FileStream fileStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(content);
                }
            }
        }

        /// <summary>
        /// 判断上传到服务器的文件扩展名是否合法
        /// </summary>
        /// <param name="myName">文件名</param>
        /// <returns></returns>
        public static bool CheckUploadFileExt(string myName)
        {
            myName = myName.Trim().ToLower();
            if (myName == "")
                return false;

            //判断非法文件名
            string wrongExtStr = ";|.asp|.aspx|.htm|.html|.shtml|.jsp|.php|#";
            string[] wrongExtString = wrongExtStr.Split('|');

            for (int i = 0; i < wrongExtString.Length; i++)
            {
                if (wrongExtString[i] != "" && myName.IndexOf(wrongExtString[i]) >= 0)
                {
                    return false;
                }
            }

            //判断合法的扩展名
            if (myName.LastIndexOf(".") >= 0)
            {
                myName = myName.Substring(myName.LastIndexOf(".") + 1);
                if (myName == "")
                    return false;
            }

            string extStr = "doc|docx|xls|xlsx|ppt|pptx|wps|rar|zip|7z|pdf|txt|"
                + "jpg|bmp|gif|png|jepg|psd|ico|tif|mp3|mp4|wma|ape|mid|ra|wav|"
                + "mpg|mpeg|avi|rm|rmvb|mov|wmv|dat|apk|jar|sis|sisx|xap|cab";
            string[] extString = extStr.Split('|');

            for (int i = 0; i < extString.Length; i++)
            {
                if (extString[i] != "" && myName == extString[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static byte[] GetFileData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            int readCount = 1024;
            int index = 0;
            byte[] data = null;
            FileStream stream = null;

            try
            {
                using (stream = new FileStream(filePath, FileMode.Open))
                {
                    data = new byte[stream.Length];

                    while (index < stream.Length)
                    {
                        if (stream.Length - index >= readCount)
                        {
                            stream.Read(data, index, readCount);
                            index += readCount;
                        }
                        else
                        {
                            stream.Read(data, index, (int)stream.Length - index);
                            break;
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();

                throw ex;
            }
        }

        /// <summary>
        /// 图片转为Byte字节数组
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns>字节数组</returns>
        public static byte[] GetImageBinaryData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Length.Equals(0))
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                using (Image image = Image.FromFile(filePath))
                {
                    using (Bitmap bitmap = new Bitmap(image))
                    {
                        bitmap.Save(stream, image.RawFormat);
                    }
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// 读取文件并返回字符串形式的文件内容
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <returns>返回字符串形式的文件内容</returns>
        public static string ReadFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (FileStream stream = new FileStream(filePath,
                    FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = string.Empty;

                        do
                        {
                            line = reader.ReadLine();

                            if (line == null)
                                break;

                            sb.Append(line);
                        }
                        while (!reader.EndOfStream);
                    }
                }
            }
            catch
            {

            }

            return sb.ToString();
        }

        /// <summary>
        /// 检查字符串是否是0、空串或NULL
        /// </summary>
        /// <param name="input"></param>
        /// <returns>返回一个布尔值，true为0、空串或NULL，false为否</returns>
        public static bool IsNullOrZero(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Equals("0"))
                return true;

            return false;
        }

        /// <summary>
        /// 获取可空日期的值
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>如果日期没有值则返回NULL，否则返回可空日期的值</returns>
        public static object GetNullableDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return null;

            return dateTime.Value;
        }

        /// <summary>
        /// 返回年月日格式的短日期（yyyy-MM-dd）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>返回yyyy-MM-dd格式的短日期</returns>
        public static string GetShortDateString(object dateTime)
        {
            if (dateTime == null)
                return string.Empty;

            DateTime convertResult = DateTime.MinValue;

            if (!DateTime.TryParse(dateTime.ToString(), out convertResult))
                return string.Empty;

            return convertResult.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 将指定枚举中的名称/值以字典的方式返回
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>返回字典形式的枚举名称/值</returns>
        public static Dictionary<string, string> GetEnumData(Type type)
        {
            Dictionary<string, string> enumData = new Dictionary<string, string>();

            foreach (string name in System.Enum.GetNames(type))
            {
                enumData.Add(name, System.Enum.Format(type,
                    System.Enum.Parse(type, name), "d"));
            }

            return enumData;
        }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <typeparam name="T">指定类型枚举</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns>返回枚举名称，如果无匹配则返回空串。</returns>
        public static string GetEnumName<T>(T value)
        {
            string result = string.Empty;

            foreach (var valueInArray in System.Enum.GetValues(typeof(T)))
            {
                if (valueInArray.Equals(value))
                {
                    result = System.Enum.GetName(typeof(T), valueInArray);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 深度复制对象
        /// </summary>
        /// <typeparam name="T">源对象泛型</typeparam>
        /// <param name="input">源对象</param>
        /// <returns>返回复制后的新对象</returns>
        public static T DeepClone<T>(T input)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, input);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// List<string>转以“，”分隔的字符串
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public static string listToString(List<String> stringList)
        {
            StringBuilder result = new StringBuilder();

            if (stringList == null)
                return null;

            List<string> dataSource = new List<string>();
            foreach (var item in stringList)
            {
                if (!dataSource.Contains(item))
                    dataSource.Add(item);
            }

            foreach (string item in dataSource)
            {
                result.Append(item + ",");
            }
            return result.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 从当前input对象移除“trimStr”的所有尾部匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string TrimEnd(string input, string trimStr)
        {
            return input.Substring(0, input.LastIndexOf(trimStr));
        }

        /// <summary>
        /// 从当前input对象移除“trimStr”的所有头匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string TrimStart(string input, string trimStr)
        {
            return input.Substring(trimStr.Length, input.Length - trimStr.Length);
        }

        /// <summary>
        /// 将字符串中的半角字符转换为全角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /// <summary>
        /// 将字符串中的全角字符转换为半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }

        public static bool IsNumeric(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (index >= input.Length)
                throw new IndexOutOfRangeException("索引超出长度范围！");

            return input[index] >= 48 && input[index] <= 57;
        }

        public static bool IsNumeric(string input, int index, int count)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (index + count > input.Length)
                throw new IndexOutOfRangeException("索引超出长度范围！");

            for (int i = index; i < index + count; i++)
            {
                if (input[i] < 48 || input[i] > 57)
                    return false;
            }

            return true;
        }

        public static bool IsCharacter(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (index >= input.Length)
                throw new IndexOutOfRangeException("索引超出长度范围！");

            return input[index] >= 65 && input[index] <= 90 || input[index] >= 97 && input[index] <= 122;
        }

        public static bool IsCharacter(string input, int index, int count)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            if (index + count > input.Length)
                throw new IndexOutOfRangeException("索引超出长度范围！");

            for (int i = index; i < index + count; i++)
            {
                if (input[i] < 65 || input[i] > 90 && input[i] < 97 || input[i] > 122)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 正则验证手机号表达式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTelePhone(string input)
        {
            Regex regex = new Regex(@"^0{0,1}(13[0-9]|15[0-9]|17[0-9]|18[0-9])[0-9]{8}$");
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 正则验证邮箱格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(string input)
        {
            Regex regex = new Regex(@"^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$");
            return regex.IsMatch(input);
        }

        public static bool ContainsCharacter(string input)
        {
            foreach (char c in input)
            {
                if (c >= 65 && c <= 90 || c >= 97 && c <= 122)
                    return true;
            }

            return false;
        }

        public static int GetLastDayOfMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public static DateTime GetLatestDateTime(int year, int month)
        {
            return new DateTime(year, month, GetLastDayOfMonth(year, month), 23, 59, 59);
        }

        public static DateTime GetLatestDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day, 23, 59, 59);
        }

        public static DateTime GetLatestDateTime(string date)
        {
            DateTime convertResult = DateTime.MinValue;

            DateTime.TryParse(date, out convertResult);

            return new DateTime(convertResult.Year, convertResult.Month, convertResult.Day, 23, 59, 59);
        }

        public static DateTime GetLatestDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        public static DateTime GetEarliestDateTime(int year, int month)
        {
            return new DateTime(year, month, 1, 23, 59, 59);
        }

        public static DateTime GetEarliestDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static DateTime GetEarliestDateTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime GetEarliesDateTime(string date)
        {
            DateTime convertResult = DateTime.MinValue;

            DateTime.TryParse(date, out convertResult);

            return new DateTime(convertResult.Year, convertResult.Month, convertResult.Day, 0, 0, 0);
        }
    }
}
