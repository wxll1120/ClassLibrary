using System;
using System.IO;
using System.Text;
using System.Drawing;

namespace ClassLibrary.Utility.IO
{
    public class FileUtil
    {
        /// <summary>
        /// 拷贝目录
        /// </summary>
        /// <param name="srcDirectory">源目录</param>
        /// <param name="destDirectory">目标目录</param>
        /// <param name="deleteSrc">删除目录</param>
        public static void CopyDirectory(string srcDirectory, string destDirectory, bool deleteSrc)
        {
            if (srcDirectory.EndsWith("\\"))
                srcDirectory = srcDirectory.Substring(0, srcDirectory.Length - 1);

            if (destDirectory.EndsWith("\\"))
                destDirectory = destDirectory.Substring(0, destDirectory.Length - 1);

            DirectoryInfo srcDirecotoryInfo = new DirectoryInfo(srcDirectory);
            DirectoryInfo destDirectoryInfo = new DirectoryInfo(destDirectory);

            if (!srcDirecotoryInfo.Exists)
                return;

            if (!destDirectoryInfo.Exists)
                destDirectoryInfo.Create();

            foreach(FileInfo fileInfo in srcDirecotoryInfo.GetFiles())
            {
                fileInfo.CopyTo(Path.Combine(destDirectoryInfo.FullName ,fileInfo.Name), true);
            }

            foreach (DirectoryInfo directoryInfo in srcDirecotoryInfo.GetDirectories())
            {
                CopyDirectory(directoryInfo.FullName, 
                    destDirectory + directoryInfo.FullName.Replace(srcDirectory,string.Empty), 
                    false);
            }

            if (deleteSrc)
            {
                srcDirecotoryInfo.Delete(true);
                srcDirecotoryInfo.Create();
            }
        }

        /// <summary>
        /// 删除文件夹和文件夹下所有的文件及文件夹
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <param name="keepDirectory">是否保留该文件夹;true:保留;false:不保留</param>
        public static void DeleteDirectory(string directory, bool keepDirectory)
        {
            if (Directory.Exists(directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                directoryInfo.Delete();

                if (keepDirectory)
                {
                    directoryInfo.Create();
                }
            }
        }

        public static string GetFileName(string fullFileName)
        {
            if (!fullFileName.Contains("/"))
                return fullFileName;

            return fullFileName.Substring(fullFileName.LastIndexOf("/") + 1);
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

        public static byte[] GetFileData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            if (!System.IO.File.Exists(filePath))
                return null;
            
            int readCount = 1024;
            int index = 0;
            byte[] data = null;
            FileStream stream = null;

            try
            {
                using (stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
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

            if (!System.IO.File.Exists(filePath))
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
    }
}
