using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cleaner
{
    class Program
    {
        static void Main(string[] args)
        {

            //DataTable dt = null;
            //string sql = "SELECT top 10 * FROM ArticleList WHERE ArticleStatus NOT IN (1,99) AND  ModifyTime < DATEADD(day,-3,getdate()) ORDER BY ID DESC";
            //do
            //{
            //    dt = SQLHelper.GetFirstTableText(sql);
            //    if (dt != null && dt.Rows.Count > 0) RemoveFile(dt);
            //}
            //while (dt != null && dt.Rows.Count > 0);
            RemoveFolder(@"C:\Website\Management\Image");
            Console.WriteLine("-------------------------清理结束-----------------------------------");
            Console.Read();
        }

        static void RemoveFile(DataTable dt)
        {
            Regex reg = new Regex(@"<img src=['|""]([^'|""]*)");
            string sql = "UPDATE ArticleList Set ArticleStatus = 99 WHERE ID = {0}";
            foreach (DataRow dr in dt.Rows)
            {
                MatchCollection matches = reg.Matches(dr["Content"].ToString());
                string cover = "C:\\Website\\Management\\" + dr["Cover"].ToString();
                if (File.Exists(cover))
                {
                    File.Delete(cover);
                    Console.WriteLine("删除文件" + cover);
                }
                else
                {
                    Console.WriteLine("文件不存在" + cover);
                }
 
                foreach (Match m in matches)
                {
                    string image = "C:\\Website\\Management\\" + m.Groups[1].Value;
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                        Console.WriteLine("删除文件" + image);
                    }
                    else
                    {
                        Console.WriteLine("文件不存在" + image);
                    }
                }

                int result = SQLHelper.ExecuteNonQueryText(string.Format(sql, dr["ID"].ToString()));
                Console.WriteLine(dr["Title"].ToString());
            }

        }

        static void RemoveFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories = di.GetDirectories();
            foreach(DirectoryInfo info in directories)
            {
                if (info.GetFiles().Count() == 0)
                {
                    Console.WriteLine("删除文件夹" + info.FullName);
                    info.Delete();
                }
            }
        }
    }
}
