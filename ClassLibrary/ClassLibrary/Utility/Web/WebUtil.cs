using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;

namespace ClassLibrary.Utility.Web
{
    public class WebUtil
    {
        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <typeparam name="T">数据源泛型</typeparam>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        public static void BindListControl<T>(ListControl control,
            ICollection<T> dataSource, string dataTextField, string dataValueField)
        {
            if (dataSource.Count.Equals(0))
                return;

            if (dataSource is Dictionary<string, string>)
            {
                Dictionary<string, string> dicDataSource =
                    dataSource as Dictionary<string, string>;

                if (dicDataSource != null)
                    BindListControl(control, dicDataSource, dataTextField, dataValueField);

                return;
            }

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <typeparam name="T">数据源泛型</typeparam>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="selectedIndex">控件选中索引</param>
        public static void BindListControl<T>(ListControl control,
            ICollection<T> dataSource, string dataTextField,
            string dataValueField, int selectedIndex)
        {
            if (dataSource.Count.Equals(0))
                return;

            if (dataSource is Dictionary<string, string>)
            {
                Dictionary<string, string> dicDataSource =
                    dataSource as Dictionary<string, string>;

                if (dicDataSource != null)
                    BindListControl(control, dicDataSource,
                        dataTextField, dataValueField, selectedIndex);

                return;
            }

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (selectedIndex >= 0)
                control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <typeparam name="T">数据源泛型</typeparam>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="emptyItemText">空项显示文本</param>
        /// <param name="emptyItemValue">空项值</param>
        public static void BindListControl<T>(ListControl control,
            ICollection<T> dataSource, string dataTextField,
            string dataValueField, string emptyItemText, string emptyItemValue)
        {
            if (dataSource.Count.Equals(0))
                return;

            if (dataSource is Dictionary<string, string>)
            {
                Dictionary<string, string> dicDataSource =
                    dataSource as Dictionary<string, string>;

                if (dicDataSource != null)
                    BindListControl(control, dicDataSource,
                        dataTextField, dataValueField, emptyItemText, emptyItemValue);

                return;
            }

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                control.Items.Insert(0, new ListItem
                {
                    Text = emptyItemText,
                    Value = emptyItemValue
                });
            }
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <typeparam name="T">数据源泛型</typeparam>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="emptyItemText">空项显示文本</param>
        /// <param name="emptyItemValue">空项值</param>
        public static void BindListControl<T>(ListControl control,
            ICollection<T> dataSource, string dataTextField,
            string dataValueField, string emptyItemText, string emptyItemValue,
            int selectedIndex)
        {
            if (dataSource.Count.Equals(0))
                return;

            if (dataSource is Dictionary<string, string>)
            {
                Dictionary<string, string> dicDataSource =
                    dataSource as Dictionary<string, string>;

                if (dicDataSource != null)
                    BindListControl(control, dicDataSource,
                        dataTextField, dataValueField, emptyItemText, emptyItemValue, selectedIndex);

                return;
            }

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                control.Items.Insert(0, new ListItem
                {
                    Text = emptyItemText,
                    Value = emptyItemValue
                });
            }

            if (selectedIndex >= 0)
                control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <typeparam name="T">数据源泛型</typeparam>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="emptyItemText">空项显示文本</param>
        /// <param name="emptyItemValue">空项值</param>
        /// <param name="selectedIndex">控件选中索引</param>
        public static void BindListControl<T>(ListControl control, IList<T> dataSource,
            string dataTextField, string dataValueField, string emptyItemText,
            string emptyItemValue, int selectedIndex)
        {
            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (!string.IsNullOrEmpty(emptyItemText))
            {
                control.Items.Insert(0, new ListItem
                {
                    Text = emptyItemText,
                    Value = emptyItemValue
                });
            }

            if (selectedIndex >= 0)
                control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型字典数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        public static void BindListControl(ListControl control,
            Dictionary<string, string> dataSource, string dataTextField,
            string dataValueField)
        {
            if (dataSource.Count.Equals(0))
                return;

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型字典数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        public static void BindListControl(ListControl control,
            Dictionary<string, string> dataSource, string dataTextField,
            string dataValueField, int selectedIndex)
        {
            if (dataSource.Count.Equals(0))
                return;

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (selectedIndex >= 0)
                control.SelectedIndex = selectedIndex;
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型字典数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="emptyItemText">空项显示文本</param>
        /// <param name="emptyItemValue">空项值</param>
        public static void BindListControl(ListControl control,
            Dictionary<string, string> dataSource, string dataTextField,
            string dataValueField, string emptyTextField, string emptyValueField)
        {
            if (dataSource.Count.Equals(0))
                return;

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (!string.IsNullOrEmpty(emptyTextField))
            {
                control.Items.Insert(0, new ListItem
                {
                    Text = emptyTextField,
                    Value = emptyValueField
                });
            }
        }

        /// <summary>
        /// 绑定ListControl控件
        /// </summary>
        /// <param name="control">ListControl控件</param>
        /// <param name="dataSource">泛型字典数据源</param>
        /// <param name="dataTextField">控件显示字段名称</param>
        /// <param name="dataValueField">控件值字段名称</param>
        /// <param name="emptyItemText">空项显示文本</param>
        /// <param name="emptyItemValue">空项值</param>
        public static void BindListControl(ListControl control,
            Dictionary<string, string> dataSource, string dataTextField,
            string dataValueField, string emptyTextField, string emptyValueField,
            int selectedIndex)
        {
            if (dataSource.Count.Equals(0))
                return;

            control.DataSource = dataSource;
            control.DataTextField = dataTextField;
            control.DataValueField = dataValueField;
            control.DataBind();

            if (!string.IsNullOrEmpty(emptyTextField))
            {
                control.Items.Insert(0, new ListItem
                {
                    Text = emptyTextField,
                    Value = emptyValueField
                });
            }

            if (selectedIndex >= 0)
                control.SelectedIndex = selectedIndex;
        }


        public static void SetListControlSelectedByValue(ListControl control, 
            string value)
        {
            for (int i = 0; i < control.Items.Count; i++)
            {
                if (control.Items[i].Value.Equals(value))
                {
                    control.SelectedIndex = i;
                    break;
                }
            }
        }

        public static void SetListControlSelectedByText(ListControl control, 
            string text)
        {
            for (int i = 0; i < control.Items.Count; i++)
            {
                if (control.Items[i].Text.Equals(text))
                {
                    control.SelectedIndex = i;
                    break;
                }
            }
        }

        public static void Alert(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            if (page == null)
                return;

            System.Web.UI.ClientScriptManager cs = page.ClientScript;

            if (!cs.IsClientScriptBlockRegistered(page.GetType(), "alert"))
            {
                cs.RegisterStartupScript(page.GetType(), "alert",
                   "alert('" + message + "');", true);
            }
        }

        public static void OpenWindow(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
            if (page == null)
                return;

            System.Web.UI.ClientScriptManager cs = page.ClientScript;

            if (!cs.IsClientScriptBlockRegistered(page.GetType(), "OpenWindow"))
            {
                cs.RegisterStartupScript(page.GetType(), "OpenWindow",
                   "window.open('" + url + "');", true);
            }
        }

        /// <summary>
        /// 数据绑定过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BindingFilter(object data)
        {
            if (data == null)
                return string.Empty;

            return data.ToString();
        }
        /// <summary>
        /// 数据绑定过滤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BindingFilter(string data)
        {
            if (string.IsNullOrEmpty(data))
                return string.Empty;

            return data;
        }

        /// <summary>
        /// 将服务端Url转换为在请求客户端可用的 Url
        /// </summary>
        /// <param name="url">服务端Url</param>
        /// <returns></returns>
        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            return System.Web.VirtualPathUtility.ToAbsolute(url);
        }

        /// <summary>
        /// 将服务端Url转换为在请求客户端可用的 Url
        /// </summary>
        /// <param name="url">服务端Url</param>
        /// <returns></returns>
        public static string ResolveUrl(object url)
        {
            if (url == null)
                return string.Empty;

            return System.Web.VirtualPathUtility.ToAbsolute(url.ToString());
        }

        /// <summary>
        /// 合并url
        /// </summary>
        /// <param name="pathList"></param>
        /// <returns></returns>
        public static string CombineUrl(params string[] pathList)
        {
            string result = string.Empty;

            if (pathList.Length.Equals(1))
                return pathList[0];

            for (int i = 0; i < pathList.Length; i++)
            {
                if (i.Equals(0))
                {
                    result += pathList[i].TrimEnd(new char[] { '\\', '/' });
                }
                else if (i < pathList.Length - 1)
                {
                    result += "/" + pathList[i].Trim(new char[] { '\\', '/' }) + "/";
                }
                else
                {
                    result += "/" + pathList[i].TrimStart(new char[] { '\\', '/' });
                }
            }

            return result;
        }
    }
}
