
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JobManage.Service.Utility
{
    public class FileHelper
    {
     

        public static string ReadFile(string path)
        {
            path = path.ReplacePath();
            if (!File.Exists(path))
                return "";
            using (StreamReader stream = new StreamReader(path))
            {
                return stream.ReadToEnd(); // 读取文件
            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string path, string fileName, string content, bool appendToLast = false)
        {
            if (!path.EndsWith("\\"))
            {
                path = path + "\\";
            }
            path = path.ReplacePath();
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream stream = File.Open(path + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = Encoding.Default.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }
    }
}
