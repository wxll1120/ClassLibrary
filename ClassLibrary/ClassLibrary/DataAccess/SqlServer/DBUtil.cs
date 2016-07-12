using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ClassLibrary.DataAccess.SqlServer
{
    public class DBUtil
    {
        /// <summary>
        /// 将object类型数据转换成布尔值类型。
        /// 如果输入为null或无效格式数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回布尔值类型数据</returns>
        public static object GetNullableBoolean(bool? input)
        {
            if (!input.HasValue)
                return DBNull.Value;

            return input.Value;
        }

        /// <summary>
        /// 将object类型数据转换成布尔值类型。
        /// 如果输入为null或无效格式数据，则引发异常。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回布尔值类型数据</returns>
        public static object GetNullableBoolean(object input)
        {
            if (input == null)
                return DBNull.Value;

            string result = input.ToString().Trim();

            if (result.Equals("1"))
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
        /// 将Image对象转换成二进制数组
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <returns>返回二进制数组</returns>
        public static byte[] GetBytes(Image image)
        {
            return GetBytes(image, image.RawFormat);
        }

        /// <summary>
        /// 将Image对象转换成二进制数组
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <param name="format">图像文件格式</param>
        /// <returns>返回二进制数组</returns>
        public static byte[] GetBytes(Image image, ImageFormat format)
        {
            byte[] data = null;

            //创建一个bitmap类型的bmp变量来读取文件。
            Bitmap bitmap = new Bitmap(image);

            //新建第二个bitmap类型的bmp2变量，我这里是根据我的程序需要设置的。
            Bitmap newBitmap = new Bitmap(image.Width, image.Height,
                PixelFormat.Format16bppRgb555);

            //将第一个bmp拷贝到bmp2中
            Graphics graphics = Graphics.FromImage(newBitmap);
            graphics.DrawImage(bitmap, 0, 0);

            using (MemoryStream stream = new MemoryStream())
            {
                newBitmap.Save(stream, format);
                data = stream.ToArray();
            }

            graphics.Dispose();
            bitmap.Dispose();

            return data;
        }

        /// <summary>
        /// 获取可空日期的值
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>如果日期没有值则返回NULL，否则返回可空日期的值</returns>
        public static object GetNullableDateTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return DBNull.Value;

            return dateTime.Value;
        }

        /// <summary>
        /// 获取可空日期的值
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>如果日期没有值则返回NULL，否则返回可空日期的值</returns>
        public static object GetNullableDateTime(object dateTime)
        {
            if (dateTime == null)
                return DBNull.Value;

            DateTime convertResult = DateTime.MinValue;

            if (!DateTime.TryParse(dateTime.ToString(), out convertResult))
                return DBNull.Value;

            return convertResult;
        }

        /// <summary>
        /// 将可空类型整数转换成int32类型数据。如果无值，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回object类型数据</returns>
        public static object GetNullableInt32(int? input)
        {
            if (!input.HasValue)
                return DBNull.Value;

            return input.Value;
        }

        /// <summary>
        /// 将可空类型整数转换成int32类型数据。如果无值，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回object类型数据</returns>
        public static object GetNullableInt32(object input)
        {
            if (input == null)
                return DBNull.Value;

            int convertResult = int.MinValue;

            if (!int.TryParse(input.ToString(), out convertResult))
                return DBNull.Value;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成decimal类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回decimal类型数据</returns>
        public static object GetNullableDecimal(decimal? input)
        {
            if (input == null)
                return DBNull.Value;

            return input.Value;
        }

        /// <summary>
        /// 将object类型数据转换成decimal类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回decimal类型数据</returns>
        public static object GetNullableDecimal(object input)
        {
            if (input == null)
                return DBNull.Value;

            decimal convertResult = 0;
            if (!decimal.TryParse(input.ToString(), out convertResult))
                return DBNull.Value;

            return convertResult;
        }

        /// <summary>
        /// 将object类型数据转换成int32类型。
        /// 如果输入为null或无效数据，则返回null。
        /// </summary>
        /// <param name="input">输入数据</param>
        /// <returns>返回int32类型数据</returns>
        public static object GetNullableInt32FromEnum(object input)
        {
            if (input == null)
                return DBNull.Value;

            return (int)input;
        }

        /// <summary>
        /// 获取可空的Guid的值，如果Guid为null、无值或者空Guid则返回DBNull。
        /// </summary>
        /// <param name="input">Guid</param>
        /// <returns>返回Guid</returns>
        public static object GetNullableGuid(Guid? input)
        {
            if (input == null || !input.HasValue || input.Value.Equals(Guid.Empty))
                return DBNull.Value;

            return input;
        }

        /// <summary>
        /// 获取可空的Guid的值，如果Guid为null、无值或者空Guid则返回DBNull。
        /// </summary>
        /// <param name="input">Guid</param>
        /// <returns>返回Guid</returns>
        public static object GetNullableGuid(object input)
        {
            if (input == null)
                return DBNull.Value;

            Guid convertResult = Guid.Empty;

            if (!Guid.TryParse(input.ToString(), out convertResult) || convertResult.Equals(Guid.Empty))
                return DBNull.Value;

            return convertResult;
        }
    }
}
