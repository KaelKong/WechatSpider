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
            DriveInfo diskC = new DriveInfo("C");
            double size = diskC.TotalSize;
            DataTable dt = null;

            Console.WriteLine("-------------------------清理开始-----------------------------------");

            Console.WriteLine("-------------------------清理文章-----------------------------------");
            string sql = "SELECT top 10 * FROM ArticleList WHERE ArticleStatus NOT IN (1,99) AND  AddDate < dateadd(day,-3,getdate()) ORDER BY AddDate DESC";
            dt = SQLHelper.GetFirstTableText(sql);
            while (dt != null && dt.Rows.Count > 0)
            {
                RemoveArticleFile(dt);
                dt = SQLHelper.GetFirstTableText(sql);
            }
            Console.WriteLine("-------------------------清理文章结束-----------------------------------");

            Console.WriteLine("-------------------------清理活动-----------------------------------");
            sql = "SELECT top 10 * FROM Campaign WHERE CampaignStatus NOT IN(1,99) AND  AddDate < dateadd(day,-3,getdate()) ORDER BY AddDate DESC";
            dt = SQLHelper.GetFirstTableText(sql);

            while (dt != null && dt.Rows.Count > 0)
            {
                RemoveCampaignFile(dt);
                dt = SQLHelper.GetFirstTableText(sql);
            }
            Console.WriteLine("-------------------------清理活动结束-----------------------------------");

            Console.WriteLine("-------------------------清理文章文件夹-----------------------------------");
            CleanFolder(@"C:\Website\Management\Image");
            Console.WriteLine("-------------------------清理文章文件夹结束-----------------------------------");

            Console.WriteLine("-------------------------清理活动文件夹-----------------------------------");
            CleanFolder(@"C:\Website\Management\Camp");
            Console.WriteLine("-------------------------清理活动文件夹结束-----------------------------------");


            Console.WriteLine("-------------------------清理结束-----------------------------------");

            Console.WriteLine(String.Format("总共释放{0}mb空间", ((size - diskC.TotalSize) / (1024 * 1024)).ToString("f2")));

            Console.Read();
        }

        static void RemoveArticleFile(DataTable dt)
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
                    //Console.WriteLine("删除文件" + cover);
                }
                else
                {
                    //Console.WriteLine("文件不存在" + cover);
                }

                foreach (Match m in matches)
                {
                    string image = "C:\\Website\\Management\\" + m.Groups[1].Value;
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                        //Console.WriteLine("删除文件" + image);
                    }
                    else
                    {
                        //Console.WriteLine("文件不存在" + image);
                    }
                }

                int result = SQLHelper.ExecuteNonQueryText(string.Format(sql, dr["ID"].ToString()));
                //Console.WriteLine(dr["Title"].ToString());
            }

        }

        static void RemoveCampaignFile(DataTable dt)
        {
            Regex reg = new Regex(@"<img src=['|""]([^'|""]*)");
            string sql = "UPDATE Campaign Set CampaignStatus = 99 WHERE ID = {0}";
            foreach (DataRow dr in dt.Rows)
            {
                MatchCollection matches = reg.Matches(dr["Content"].ToString());
                string cover = "C:\\Website\\Management\\" + dr["Cover"].ToString();
                if (File.Exists(cover))
                {
                    File.Delete(cover);
                    //Console.WriteLine("删除文件" + cover);
                }
                else
                {
                    //Console.WriteLine("文件不存在" + cover);
                }

                foreach (Match m in matches)
                {
                    string image = "C:\\Website\\Management\\" + m.Groups[1].Value;
                    if (File.Exists(image))
                    {
                        File.Delete(image);
                       // Console.WriteLine("删除文件" + image);
                    }
                    else
                    {
                       // Console.WriteLine("文件不存在" + image);
                    }
                }

                int result = SQLHelper.ExecuteNonQueryText(string.Format(sql, dr["ID"].ToString()));
                //Console.WriteLine(dr["Title"].ToString());
            }
        }

        static void CleanFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories = di.GetDirectories();
            foreach (DirectoryInfo info in directories)
            {
                if (info.GetFiles().Count() == 0)
                {
                    //Console.WriteLine("删除文件夹" + info.FullName);
                    info.Delete();
                }
            }
        }
    }
}
