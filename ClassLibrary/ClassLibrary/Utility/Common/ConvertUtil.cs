using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.IO;

namespace ClassLibrary.Utility.Common
{
    public class ConvertUtil
    {
        /// <summary>
        /// 将object类型数据转换成布尔值，如果输入数据为null，则返回false。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回布尔值类型数据</returns>
        public static bool ToBoolean(object input)
        {
            if (input == null)
                return false;

            string result = input.ToString().Trim();
            if (result.Equals(string.Empty))
                return false;
            else if (result.Equals("1"))
                return true;
            else if (result.Equals("0"))
                return false;
            else if (result.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (result.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                throw new InvalidCastException("未知输入数据类型！");
        }

        /// <summary>
        /// 将object类型数据转换成布尔值，如果输入数据为null，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回布尔值类型数据</returns>
        public static bool? ToNullableBoolean(object input)
        {
            if (input == null)
                return null;

            string result = input.ToString().Trim();

            if (result.Equals(string.Empty))
                return null;
            else if (result.Equals("1"))
                return true;
            else if (result.Equals("0"))
                return false;
            else if (result.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (result.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                throw new InvalidCastException("未知输入数据类型！");
        }

        /// <summary>
        /// 将object类型数据转换成bool类型，如果输入为null或转换失败则返回false
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回bool类型数据，如果输入为null或转换失败则返回false</returns>
        public static bool ToBooleanOrDefault(object input)
        {
            bool result = false;

            if (input == null)
                return result;

            bool.TryParse(input.ToString(), out result);

            return result;
        }

        /// <summary>
        /// 将object类型数据转换成日期类型。
        /// 如果输入输入为null或无效格式日期，则返回DateTime.MinValue。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回DateTime类型数据</returns>
        public static DateTime ToDateTime(object input)
        {
            if (input == null)
                return DateTime.MinValue;

            DateTime convertResult = DateTime.MinValue;
            DateTime.TryParse(input.ToString(), out convertResult);

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成日期类型。
        /// 如果输入为null或无效格式日期，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回DateTime类型数据</returns>
        public static DateTime? ToNullableDateTime(object input)
        {
            if (input == null)
                return null;

            DateTime convertResult = DateTime.MinValue;

            if (!DateTime.TryParse(input.ToString(), out convertResult))
                return null;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成DateTime类型，如果输入为null或转换失败则返回DateTime.MinValue
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回DateTime类型数据，如果输入为null或转换失败则返回DateTime.MinValue</returns>
        public static DateTime ToDateTimeOrDefault(object input)
        {
            DateTime result = DateTime.MinValue;

            if (input == null)
                return result;

            DateTime.TryParse(input.ToString(), out result);

            return result;
        }

        /// <summary>
        /// 将object类型数据转换成整型数据
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回整型</returns>
        public static int ToInt32(object input)
        {
            return int.Parse(input.ToString());
        }

        /// <summary>
        /// 将object类型数据转换成可空的int32类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回可空的int32类型数据</returns>
        public static int? ToNullableInt32(object input)
        {
            if (input == null)
                return null;

            int convertResult = 0;
            if (!int.TryParse(input.ToString(), out convertResult))
                return null;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成Int32类型，如果输入为null或转换失败则返回0
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回Int32类型数据，如果输入为null或转换失败则返回0</returns>
        public static int ToInt32OrDefault(object input)
        {
            int result = 0;

            if (input == null)
                return result;

            int.TryParse(input.ToString(), out result);

            return result;
        }

        /// <summary>
        /// 将object类型数据转换成decimal类型
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回decimal类型数据</returns>
        public static decimal ToDecimal(object input)
        {
            if (input == null)
                return 0;
            return decimal.Parse(input.ToString());
        }

        /// <summary>
        /// 将object类型数据转换成decimal类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回decimal类型数据</returns>
        public static decimal? ToNullableDecimal(object input)
        {
            if (input == null)
                return null;

            decimal convertResult = 0;
            if (!decimal.TryParse(input.ToString(), out convertResult))
                return null;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成decimal类型，如果输入为null或转换失败则返回0
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回decimal类型数据，如果输入为null或转换失败则返回0</returns>
        public static decimal ToDecimalOrDefault(object input)
        {
            decimal result = 0;

            if (input == null)
                return result;

            decimal.TryParse(input.ToString(), out result);

            return result;
        }

        /// <summary>
        /// 将object类型数据转换成Guid数据
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回整型</returns>
        public static Guid ToGuid(object input)
        {
            return Guid.Parse(input.ToString());
        }

        /// <summary>
        /// 将object类型数据转换成Guid类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回Guid类型数据</returns>
        public static Guid? ToNullableGuid(object input)
        {
            if (input == null)
                return null;

            Guid convertResult = Guid.Empty;

            if (!Guid.TryParse(input.ToString(), out convertResult))
                return null;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成Guid类型，如果输入为null或转换失败则返回Guid.Empty
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回Guid类型数据，如果输入为null或转换失败则返回Guid.Empty</returns>
        public static Guid ToGuidOrDefault(object input)
        {
            Guid result = Guid.Empty;

            if (input == null)
                return result;

            Guid.TryParse(input.ToString(), out result);

            return result;
        }

        public static string ToString(object input)
        {
            if (input == null)
                return string.Empty;
            return input.ToString();
        }

        /// <summary>
        /// 将object类型的数据转换成指定类型枚举
        /// </summary>
        /// <typeparam name="T">指定类型枚举</typeparam>
        /// <param name="value">枚举值或名称</param>
        /// <returns>返回指定类型枚举</returns>
        public static T ToEnum<T>(object value)
        {
            T result = default(T);

            result = (T)System.Enum.Parse(typeof(T), value.ToString(), true);

            return result;
        }

        /// <summary>
        /// 将二进制数据转换成Image对象
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>返回Image对象</returns>
        public static Image ToImage(byte[] data)
        {
            Image image = null;

            if (data == null)
                return null;

            using (MemoryStream stream = new MemoryStream(data))
            {
                image = Image.FromStream(stream);
            }

            return image;
        }

        public static bool IsInt32(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            int convertResult = 0;

            return int.TryParse(input, out convertResult);
        }

        public static bool IsNumeric(object input)
        {
            if (input == null || string.IsNullOrEmpty(input.ToString()))
                return false;

            decimal convertResult = 0;

            return decimal.TryParse(input.ToString(), out convertResult);
        }
    }
}
