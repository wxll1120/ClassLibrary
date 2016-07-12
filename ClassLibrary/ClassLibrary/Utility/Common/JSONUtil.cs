using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace ClassLibrary.Utility.Common
{
    public class JSONUtil
    {
        /// <summary>
        /// 判断JSON格式的内容是否为空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool JSONNull(object input)
        {
            return input == null || string.IsNullOrEmpty(input.ToString());
        }

        public static bool HasValue(JToken token)
        {
            return token != null && !string.IsNullOrEmpty(token.ToString());
        }
    }
}
