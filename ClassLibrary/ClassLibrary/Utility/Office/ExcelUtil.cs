using System;
using System.Data.OleDb;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Web;
using System.Collections.Generic;

namespace ClassLibrary.Utility.Office
{
    public class ExcelUtil
    {
        /// <summary>
        /// 2003Excel连接字符串
        /// </summary>
        private const string excel2003ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;";

        /// <summary>
        /// 2007Excel连接字符串
        /// </summary>
        //private const string excel2007ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        private const string excel2007ConnectionString = "Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source ={0};Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";

        /// <summary>
        /// CSV文件连接字符串
        /// </summary>
        //private const string csvConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";
        //private const string csvConnectionString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=Extensions=asc,csv,tab,txt";
        private const string csvConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";

        /// <summary>
        /// 将DataTable数据导出到CSV文件
        /// </summary>
        /// <param name="table">DataTable数据源</param>
        /// <param name="file">导出文件完整路径</param>
        public static void ConvertToCSV(DataTable table, string file)
        {
            string title = string.Empty;

            using (FileStream fileStream = new FileStream(file, 
                FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(
                    new BufferedStream(fileStream), System.Text.Encoding.Default))
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        title += column.ColumnName + ",";
                    }

                    title = title.Substring(0, title.Length - 1) + "\n";

                    streamWriter.Write(title);

                    foreach (DataRow row in table.Rows)
                    {
                        string line = string.Empty;

                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            line += row[i].ToString().Trim() + ",";
                        }

                        line = line.Substring(0, line.Length - 1) + "\n";

                        streamWriter.Write(line);
                    }
                }
            }
        }
    }
}
