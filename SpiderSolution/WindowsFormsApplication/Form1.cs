using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        //public string FilePath { get; set; }
        private string SearchUrl { get; set; }

        private string ConnectionString { get; set; }
        private DataTable WechatNames { get; set; }

        public void StartWork()
        {
            ConsoleMessage("#########抓取程序开始#########");
            InitialData();
            GetIndexUrl();
            //GetDetailUrl();
            //GetDetailContent();
            ConsoleMessage("#########抓取程序结束#########");
        }

        private void InitialData()
        {
            string sql = "SELECT * FROM WechatList";
            WechatNames = SqlHelper.ExecuteDataSetText(sql, null).Tables[0];
        }

        private void GetIndexUrl()
        {
            foreach (DataRow dr in WechatNames.Rows)
            {
                ConsoleMessage("****************************");
                if (!string.IsNullOrEmpty(dr["Url"].ToString()))
                {
                    ConsoleMessage(dr["Name"].ToString().Trim() + "已抓取");
                    continue;
                }
                string name = dr["Name"].ToString().Trim();
                string wechatID = dr["ID"].ToString();
                string url = string.Format(SearchUrl, System.Web.HttpUtility.UrlEncode(name));
                
                tbAddress.Text = url;
                myWebBrowser.Navigate(url);
                WaitWebPageLoad();
               Delay(10000);
                string result = myWebBrowser.DocumentText;
                CheckUrlContent(ref result, "用户您好，您的访问过于频繁，为确认本次访问为正常用户行为，需要您协助验证。", url);
                Regex reg1 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)""><em><!--red_beg-->" + name + "<!--red_end--></em></a>", RegexOptions.IgnoreCase);
                Regex reg2 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)"">" + name + "</a>", RegexOptions.IgnoreCase);
                MatchCollection matches1 = reg1.Matches(result);
                MatchCollection matches2 = reg2.Matches(result);
                if (matches1 != null && matches1.Count > 0)
                {
                    ConsoleMessage(name);
                    GetDetailUrl(matches1[0].Groups[1].Value, dr[0].ToString(), wechatID);
                }
                else if (matches2 != null && matches2.Count > 0)
                {

                    ConsoleMessage(name);
                    GetDetailUrl(matches2[0].Groups[1].Value, dr[0].ToString(), wechatID);
                }
                else
                {

                    ConsoleMessage(name + "未搜索到匹配的主页");
                }

                ConsoleMessage("****************************");
            }
        }

        private void GetDetailUrl(string url, string id,string wechatID)
        {
            ConsoleMessage("详情页信息抓取......");
            ConsoleMessage("--------------------------------");
            url = url.Replace("&amp;", "&");
            tbAddress.Text = url;
            int executeNum = 0;
            myWebBrowser.Navigate(url);
            //Delay(15000);
            WaitWebPageLoad();
            string result = myWebBrowser.DocumentText;
            CheckUrlContent(ref result, "为了保护你的网络安全，请输入验证码", url);
            Regex reg = new Regex(@"(?=(var msgList = ))var msgList =([.\s\S]*?(?=(}}]};))}}]});");
            MatchCollection matches = reg.Matches(result);
            foreach (Match m in matches)
            {
                string json = m.Groups[2].Value;
                WechatList wechatList = JsonHelper.DeserializeJsonToObject<WechatList>(json);
                foreach (WechatCotent app in wechatList.list)
                {
                    //ConsoleMessage(app.comm_msg_info.id);
                    executeNum += AddWechatContent(app,wechatID);
                }
            }
            string sql = "UPDATE WechatList SET Url = @url,DetailCount = @detailCount,ModifyDate = GETDATE() WHERE ID = @id ";
            SqlParameter[] paramaters = new SqlParameter[3];
            paramaters[0] = new SqlParameter("@url", url);
            paramaters[1] = new SqlParameter("@detailCount", executeNum);
            paramaters[2] = new SqlParameter("@id", id);
            SqlHelper.ExecteNonQueryText(sql, paramaters);
            ConsoleMessage("--------------------------------");
        }

        private string GetDetailContent(string url)
        {
            //url = url.Replace("&amp;", "&");
            if (string.IsNullOrEmpty(url)) return string.Empty;
            url=url.Contains("http://")?url : "http://mp.weixin.qq.com" + url;
            tbAddress.Text = url;
            myWebBrowser.Navigate(url);
            WaitWebPageLoad();
            //Delay(10000);
            string result = myWebBrowser.DocumentText;
            Regex reg = new Regex(@"<div class=""rich_media_content "" id=""js_content"">([.\s\S]*?(?=(</div>)))</div>");
            MatchCollection matches = reg.Matches(result);
            if (matches != null && matches.Count == 1)
            {
                return matches[0].Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }

        }

        public Form1()
        {
            InitializeComponent();
            tbSougou.Text = @"http://weixin.sogou.com/weixin?type=1&query={0}&ie=utf8&s_from=input&_sug_=y&_sug_type_=";
            tbSourceFile.Text = @"C:\Users\KaelKong\Desktop\wechatnaem.xls";
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openSourceFile.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbSourceFile.Text = openSourceFile.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ConnectionString = string.Format(@"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = {0}; Extended Properties = 'Excel 8.0;HDR=Yes;IMEX=2;';", tbSourceFile.Text.Trim());
            SearchUrl = tbSougou.Text.Trim();
            if (string.IsNullOrEmpty(ConnectionString) || string.IsNullOrEmpty(SearchUrl))
            {
                ConsoleMessage("文件地址或搜索地址为空");
                return;
            }

            StartWork();
        }

        public int AddWechatContent(WechatCotent content,string wechatID)
        {
            int result = 0;
            string sql = @"IF NOT EXISTS(SELECT ID FROM ArticleList WHERE CommID = @commID AND Title = @title)
                                            INSERT INTO ArticleList
                                                            ([Author]
                                                            ,[ContentUrl]
                                                            ,[CopyrightStat]
                                                            ,[Cover]
                                                            ,[Digest]
                                                            ,[FileID]
                                                            ,[SourceUrl]
                                                            ,[Title]
                                                            ,[ArticleDate]
                                                            ,[FakeID]
                                                            ,[CommID]
                                                            ,[CommStatus]
                                                            ,[CommType]
                                                            ,[Content]
                                                            ,[WechatID]) 
                                            VALUES
                                                            (@author
                                                            ,@contentUrl
                                                            ,@copyrightStat
                                                            ,@cover
                                                            ,@digest
                                                            ,@fileID
                                                            ,@sourceUrl
                                                            ,@title
                                                            ,@articleDate
                                                            ,@fakeID
                                                            ,@commID
                                                            ,@commStatus
                                                            ,@commType
                                                            ,@content
                                                            ,@wechatID) ";
            SqlParameter[] parameters = new SqlParameter[15];
            parameters[0] = new SqlParameter("@author", content.app_msg_ext_info.author);
            parameters[1] = new SqlParameter("@contentUrl", content.app_msg_ext_info.content_url.Replace("&amp;", "&"));
            parameters[2] = new SqlParameter("@copyrightStat", content.app_msg_ext_info.copyright_stat == null ? "" : content.app_msg_ext_info.copyright_stat);
            parameters[3] = new SqlParameter("@cover", content.app_msg_ext_info.cover);
            parameters[4] = new SqlParameter("@digest", content.app_msg_ext_info.digest);
            parameters[5] = new SqlParameter("@fileID", content.app_msg_ext_info.fileid);
            parameters[6] = new SqlParameter("@sourceUrl", content.app_msg_ext_info.source_url.Replace("&amp;", "&"));
            parameters[7] = new SqlParameter("@title", content.app_msg_ext_info.title);
            parameters[8] = new SqlParameter("@articleDate", content.comm_msg_info.datetime);
            parameters[9] = new SqlParameter("@fakeID", content.comm_msg_info.fakeid);
            parameters[10] = new SqlParameter("@commID", content.comm_msg_info.id);
            parameters[11] = new SqlParameter("@commStatus", content.comm_msg_info.status);
            parameters[12] = new SqlParameter("@commType", content.comm_msg_info.type);
            parameters[13] = new SqlParameter("@content", GetDetailContent(content.app_msg_ext_info.content_url.Replace("&amp;", "&")));
            parameters[14] = new SqlParameter("@wechatID", wechatID);
            result += SqlHelper.ExecteNonQueryText(sql, parameters);
            ConsoleMessage(content.app_msg_ext_info.title);
            foreach (multi_app_msg_item_list li in content.app_msg_ext_info.multi_app_msg_item_list)
            {
                parameters[0].Value = li.author;
                parameters[1].Value = li.content_url.Replace("&amp;", "&");
                parameters[2].Value = li.copyright_stat == null ? "" : li.copyright_stat;
                parameters[3].Value = li.cover;
                parameters[4].Value = li.digest;
                parameters[5].Value = li.fileid;
                parameters[6].Value = li.source_url;
                parameters[7].Value = li.title;
                parameters[13].Value = GetDetailContent(li.content_url.Replace("&amp;", "&"));
                result += SqlHelper.ExecteNonQueryText(sql, parameters);
                ConsoleMessage(li.title);
            }

            return result;
        }
        static public int AddWechatArticleContent(string content,int id)
        {
            string sql = "UPDATE ArticleList SET Content =@content WHERE ID = @id";
            SqlParameter acontent = new SqlParameter("@content", content);
            SqlParameter aid = new SqlParameter("@id", id);
            return SqlHelper.ExecteNonQueryText(sql, acontent, aid);
        }

        private void ConsoleMessage(string content)
        {
            rtbResult.AppendText(content + "\r\n");
            rtbResult.ScrollToCaret();
        }

        private void CheckUrlContent(ref string result, string key, string url)
        {
            if (result.Contains(key))
            {
                while (true)
                {
                    Delay(15000);
                    if (!myWebBrowser.DocumentText.Contains(key))
                    {
                        result = myWebBrowser.DocumentText;
                        break;
                    }
                }
            }
        }

        private bool WaitWebPageLoad()
        {
            int i = 0;
            string sUrl;
            while (true)
            {
                Delay(50);  //系统延迟50毫秒，够少了吧！               
                if (myWebBrowser.ReadyState == WebBrowserReadyState.Complete) //先判断是否发生完成事件。  
                {
                    if (!myWebBrowser.IsBusy) //再判断是浏览器是否繁忙                    
                    {
                        i = i + 1;
                        if (i == 2)   //为什么 是2呢？因为每次加载frame完成时就会置IsBusy为false,未完成就就置IsBusy为false，你想一想，加载一次，然后再一次，再一次...... 最后一次.......  
                        {
                            sUrl = myWebBrowser.Url.ToString();
                            if (sUrl.Contains("res")) //这是判断没有网络的情况下                             
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        continue;
                    }
                    i = 0;
                }
            }
        }

        private void Delay(int Millisecond) //延迟系统时间，但系统又能同时能执行其它任务；  
        {

            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(Millisecond) > DateTime.Now)
            {
                Application.DoEvents();//转让控制权              
            }
            return;
        }

        private void btnContent_Click(object sender, EventArgs e)
        {
            string sql = "SELECT ID,ContentUrl FROM ArticleList WHERE Content IS NULL OR Content = ''";
            DataSet ds = SqlHelper.ExecuteDataSetText(sql, null);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string content = GetDetailContent(dr["ContentUrl"].ToString());
                    AddWechatArticleContent(content, Convert.ToInt32(dr["ID"]));
                    ConsoleMessage("文章ID：" + dr["ID"].ToString());
                }
            }
        }
    }
}
