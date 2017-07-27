using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
        private DateTime LastDate { get; set; }
        private string ArticleFolder = ConfigurationManager.AppSettings["article"];
        private string CampaignFolder = ConfigurationManager.AppSettings["campaign"];
        private int DelayTime = Convert.ToInt32(ConfigurationManager.AppSettings["delay"]);

        public void StartWork()
        {
            ConsoleMessage("#########抓取程序开始#########");
            InitialData("SELECT * FROM WechatList WHERE WechatStatus = 1");
            LastDate = Convert.ToDateTime(SqlHelper.ExecuteScalarText("SELECT TOP 1 AddDate FROM ArticleList ORDER BY ID DESC"));
            GetIndexUrl();
            ConsoleMessage("#########抓取程序结束#########");
        }

        private void InitialData(string sql)
        {
            WechatNames = SqlHelper.ExecuteDataSetText(sql).Tables[0];
            SearchUrl = tbSougou.Text.Trim();
        }

        //文章抓取
        private void GetIndexUrl()
        {
            prgb.Maximum = WechatNames.Rows.Count;
            int i = 1;
            foreach (DataRow dr in WechatNames.Rows)
            {
                prgb.Value = i++;
                ConsoleMessage("****************************");
                string name = dr["Name"].ToString().Trim();
                string wechatID = dr["ID"].ToString();
                string url = string.Format(SearchUrl, System.Web.HttpUtility.UrlEncode(name));

                tbAddress.Text = url;
                myWebBrowser.Navigate(url);
                WaitWebPageLoad();
                Delay(DelayTime);
                string result = string.Empty;
                try
                {
                    result = myWebBrowser.DocumentText;
                }
                catch
                {
                    return;
                }
                CheckUrlContent(ref result, "用户您好，您的访问过于频繁，为确认本次访问为正常用户行为，需要您协助验证。", url);
                Regex reg1 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)""><em><!--red_beg-->" + name + "<!--red_end--></em></a>", RegexOptions.IgnoreCase);
                Regex reg2 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)"">" + name + "</a>", RegexOptions.IgnoreCase);
                MatchCollection matches1 = reg1.Matches(result);
                MatchCollection matches2 = reg2.Matches(result);
                if (matches1 != null && matches1.Count > 0)
                {
                    ConsoleMessage(name);
                    GetDetailUrl(matches1[0].Groups[1].Value, dr[0].ToString(), wechatID, name, dr);
                }
                else if (matches2 != null && matches2.Count > 0)
                {
                    ConsoleMessage(name);
                    GetDetailUrl(matches2[0].Groups[1].Value, dr[0].ToString(), wechatID, name, dr);
                }
                else
                {
                    SqlHelper.ExecteNonQueryText($"UPDATE WechatList SET WechatStatus = 2 WHERE ID = {dr["ID"].ToString()}");
                    ConsoleMessage(name + "未搜索到匹配的主页");
                }

                ConsoleMessage("****************************");
            }
        }

        //活动抓取
        private void GetIndexUrl2()
        {
            prgb.Maximum = WechatNames.Rows.Count;
            int i = 1;

            foreach (DataRow dr in WechatNames.Rows)
            {
                prgb.Value = i++;
                ConsoleMessage("****************************");
                string name = dr["WechatName"].ToString().Trim();
                string wechatID = dr["Name"].ToString();
                string url = string.Format(SearchUrl, System.Web.HttpUtility.UrlEncode(name));
                tbAddress.Text = url;
                myWebBrowser.Navigate(url);
                WaitWebPageLoad();
                Delay(DelayTime);
                string result = string.Empty;
                try
                {
                    result = myWebBrowser.DocumentText;
                }
                catch
                {
                    return;
                }
                CheckUrlContent(ref result, "用户您好，您的访问过于频繁，为确认本次访问为正常用户行为，需要您协助验证。", url);
                Regex reg1 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)""><em><!--red_beg-->" + name + "<!--red_end--></em></a>", RegexOptions.IgnoreCase);
                Regex reg2 = new Regex(@"<a target=""_blank"" uigs=""account_name_[0-9]"" href=""([^""]*)"">" + name + "</a>", RegexOptions.IgnoreCase);
                MatchCollection matches1 = reg1.Matches(result);
                MatchCollection matches2 = reg2.Matches(result);
                if (matches1 != null && matches1.Count > 0)
                {
                    ConsoleMessage(name);
                    GetDetailUrl2(matches1[0].Groups[1].Value, dr[0].ToString(), wechatID, name);
                }
                else if (matches2 != null && matches2.Count > 0)
                {
                    ConsoleMessage(name);
                    GetDetailUrl2(matches2[0].Groups[1].Value, dr[0].ToString(), wechatID, name);
                }
                else
                {
                    SqlHelper.ExecteNonQueryText($"UPDATE DZDP SET MallStatus = 2 WHERE WechatName = '{dr["WechatName"].ToString()}' ");
                    ConsoleMessage(name + "未搜索到匹配的主页");
                }

                ConsoleMessage("****************************");
            }
        }

        private void GetIndexUrl3()
        {
            prgb.Maximum = WechatNames.Rows.Count;
            int i = 1;
            Regex reg = new Regex(@"<li id=""rev_[0-9]*"" data-id=""[0-9]*"">\s*<div class=""pic"">\s*<a target=""_blank"" rel=""nofollow"" href=""/member/[0-9]*"" user-id=""([0-9]*)"" class=""J_card"">\s*<img title=""([^""]*)"" alt=""[^""]*"" src=""([^""]*)"">[.\s\S]*?(?=(J_brief-cont))J_brief-cont"">([.\s\S]*?(?=(</div>)))</div>[.\s\S]*?(?=(class=""time""))class=""time"">([^<]*)</span>[.\s\S]*?(?=(heart-name))heart-name"">赞</span>\s*(.*)");

            foreach (DataRow dr in WechatNames.Rows)
            {
                prgb.Value = i++;
                ConsoleMessage(dr["MallName"].ToString());
                string name = dr["WechatName"].ToString().Trim();
                string wechatID = dr["Name"].ToString();
                string url = dr["Link"].ToString();
                if (string.IsNullOrEmpty(url)) continue;
                tbAddress.Text = url + $"/review_all?pageno=1";
                myWebBrowser.Navigate(tbAddress.Text);
                WaitWebPageLoad();
                Delay(DelayTime);
                string result = string.Empty;
                try
                {
                    result = myWebBrowser.DocumentText;
                }
                catch
                {
                    return;
                }
                MatchCollection matches = reg.Matches(result);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        DZDPCommentModel model = new DZDPCommentModel()
                        {
                            AuthorID = match.Groups[1].Value,
                            AuthorName = match.Groups[2].Value,
                            AuthorIcon = match.Groups[3].Value,
                            Content = match.Groups[5].Value.Trim(),
                            AddDate = match.Groups[8].Value,
                            LikeNum = match.Groups[10].Value,
                            MallID = Convert.ToInt32(dr["MallID"]),
                            MallName = dr["MallName"].ToString(),
                            CommentStatus = 1
                        };

                        Save(model);
                    }
                }
                else
                {
                    break;
                }

                ConsoleMessage("****************************");
            }
        }

        private void GetIndexUrl4()
        {
            prgb.Maximum = WechatNames.Rows.Count;
            int i = 1;
            foreach (DataRow dr in WechatNames.Rows)
            {
                prgb.Value = i++;
                ConsoleMessage("****************************");
                string name = dr["Name"].ToString().Trim();
                string wechatID = dr["ID"].ToString();
                string url = string.Format(SearchUrl, System.Web.HttpUtility.UrlEncode(name));

                tbAddress.Text = url;
                myWebBrowser.Navigate(url);
                WaitWebPageLoad();
                Delay(DelayTime);
                string result = string.Empty;
                try
                {
                    result = myWebBrowser.DocumentText;
                }
                catch
                {
                    return;
                }
                CheckUrlContent(ref result, "用户您好，您的访问过于频繁，为确认本次访问为正常用户行为，需要您协助验证。", url);
                Regex image = new Regex(@"<a target=""_blank"" uigs=""account_image_0"" href=""[^""]*""><span></span><img src=""([^""]*)""");
                Match match = image.Match(result);
                if (match.Success)
                {
                    string rpl = "/Avatar/" + ReplaceAsLocalImg(match.Groups[1].Value, "E:\\Avatar", wechatID);
                    UpdateIcon(wechatID, rpl);
                }
                else
                {
                    ConsoleMessage(name + "未搜索到匹配的主页");
                }



                ConsoleMessage("****************************");
            }
        }

        private void GetDetailUrl(string url, string id, string wechatID, string wechatName, DataRow rowData)
        {
            ConsoleMessage("详情页信息抓取......");
            ConsoleMessage("--------------------------------");
            url = url.Replace("&amp;", "&");
            tbAddress.Text = url;
            int executeNum = 0;
            myWebBrowser.Navigate(url);
            WaitWebPageLoad();
            Delay(DelayTime);
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
                    DateTime at = WebHelper.GetTime(app.comm_msg_info.datetime);
                    if (at > LastDate)
                    {
                        executeNum += AddWechatContent(app, wechatID, wechatName, rowData["Icon"].ToString());
                    }
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

        private void GetDetailUrl2(string url, string id, string wechatID, string wechatName)
        {
            ConsoleMessage("详情页信息抓取......");
            ConsoleMessage("--------------------------------");
            url = url.Replace("&amp;", "&");
            tbAddress.Text = url;
            int executeNum = 0;
            myWebBrowser.Navigate(url);
            WaitWebPageLoad();
            Delay(DelayTime);
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
                    DateTime at = WebHelper.GetTime(app.comm_msg_info.datetime);
                    if (at > LastDate)
                    {
                        executeNum += AddWechatContent2(app, wechatID, wechatName);
                    }

                }
            }
            ConsoleMessage("--------------------------------");
        }

        private Tuple<string, string, string> GetDetailContent(string url, string folder, string newFolder, string phsicPath)
        {
            
            //url = url.Replace("&amp;", "&");
            if (string.IsNullOrEmpty(url)) return Tuple.Create<string, string, string>("", "", "");
            url = url.Contains("http://") ? url : "http://mp.weixin.qq.com" + url;
            tbAddress.Text = url;
            myWebBrowser.Navigate(url);
            WaitWebPageLoad();
            Delay(DelayTime);
            //WaitingJs(@"sg_readNum3");
            string result = myWebBrowser.Document.Body.OuterHtml;
            //Regex reg = new Regex(@"<div class=""rich_media_content "" id=""js_content"">([.\s\S]*?(?=(</div>)))</div>[.\s\S]*?(?=(sg_readNum3))sg_readNum3"">([^<]*)</span>[.\s\S]*?(?=(sg_likeNum3))sg_likeNum3"">([^<]*)</span>");
            Regex reg = new Regex(@"<div class=""rich_media_content "" id=""js_content"">([.\s\S]*?(?=(</div>)))</div>");
            Regex imgReg = new Regex(@"<img[.\s\S]*?(?=(data-src=""))data-src=""([^""]*)""[^>]*>");

            MatchCollection matches = reg.Matches(result);
            if (matches != null && matches.Count == 1)
            {
                string content = matches[0].Groups[1].Value;
                string readNum = matches[0].Groups[4].Value;
                string likeNum = matches[0].Groups[6].Value;

                MatchCollection imgMatches = imgReg.Matches(content);
                foreach (Match match in imgMatches)
                {

                    //string guid = Guid.NewGuid().ToString("N");
                    string oldUrl = match.Groups[2].Value;
                    //string[] imgArr = oldUrl.Split(new string[] { "?wx_fmt=" }, StringSplitOptions.RemoveEmptyEntries);
                    //string exetent = "jpg";
                    //if (imgArr.Length == 2 && !string.IsNullOrEmpty(imgArr[1])) exetent = imgArr[1];
                    //string imgName = guid + "." + exetent;
                    //DownloadImg(oldUrl, folder + "\\" + imgName);
                    //content = content.Replace(oldUrl, "/Image/" + folder + "/" + imgName);
                    string newUrl = ReplaceAsLocalImg(oldUrl, phsicPath + "//" + folder);
                    content = content.Replace(match.Groups[0].Value, "<img src='/" + newFolder + "/" + folder + "/" + newUrl + "' />");
                }

                return Tuple.Create<string, string, string>(content, readNum, likeNum);
            }
            else
            {
                return Tuple.Create<string, string, string>("", "", "");
            }

        }

        private string ReplaceAsLocalImg(string url, string folderPath)
        {

            return ReplaceAsLocalImg(url, folderPath, Guid.NewGuid().ToString("N"));
        }

        private string ReplaceAsLocalImg(string url, string folderPath, string name)
        {
            if (string.IsNullOrEmpty(url)) return "";
            string[] formats = url.Split(new string[] { "wx_fmt=" }, StringSplitOptions.RemoveEmptyEntries);
            string newUrl = name + "." + (formats.Length == 2 ? formats[1] : "jpg");
            DownloadImg(url, folderPath + "\\" + newUrl);
            return newUrl;
        }

        private void DownloadImg(string Url, string newUrl)
        {
            try
            {
                Url = Url.Contains("http://") ? Url : "http://mp.weixin.qq.com" + Url;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                long count = response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)
                {
                    int n = myResponseStream.Read(buf, offset, (int)count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }

                if (File.Exists(newUrl)) File.Delete(newUrl);

                FileStream writer = new FileStream(newUrl, FileMode.OpenOrCreate, FileAccess.Write);
                writer.Write(buf, 0, buf.Length);
                writer.Flush();
                writer.Close();
            }
            catch
            {

            }
        }

        public Form1()
        {
            InitializeComponent();
            tbSougou.Text = @"http://weixin.sogou.com/weixin?type=1&query={0}&ie=utf8&s_from=input&_sug_=y&_sug_type_=";
            tbSourceFile.Text = @"C:\Users\KaelKong\Desktop\wechatnaem.xls";
            prgb.Minimum = 0;

        }

        
        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            ConsoleMessage("#########抓取程序开始#########");
            InitialData("SELECT * FROM DZDP WHERE MallStatus = 1");
            LastDate = Convert.ToDateTime(SqlHelper.ExecuteScalarText("SELECT TOP 1 AddDate FROM Campaign ORDER BY ID DESC"));
            GetIndexUrl2();
            ConsoleMessage("#########抓取程序结束#########");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartWork();
        }

        public int AddWechatContent(WechatCotent content, string wechatID, string wechatName, string wechatIcon)
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
                                                            ,[WechatID]
                                                            ,ReadNum
                                                            ,LikeNum
                                                            ,FolderName
                                                            ,WechatName
                                                            ,WechatIcon) 
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
                                                            ,@wechatID
                                                            ,@readNum
                                                            ,@likeNum
                                                            ,@folderName
                                                            ,@wechatName
                                                            ,@wechatIcon)";
            string folderName = Guid.NewGuid().ToString("N");
            ConsoleMessage(content.app_msg_ext_info.title);
            SqlParameter[] parameters = new SqlParameter[20];
            parameters[0] = new SqlParameter("@author", content.app_msg_ext_info.author);
            parameters[1] = new SqlParameter("@contentUrl", content.app_msg_ext_info.content_url.Replace("&amp;", "&"));
            parameters[2] = new SqlParameter("@copyrightStat", content.app_msg_ext_info.copyright_stat == null ? "" : content.app_msg_ext_info.copyright_stat);
            parameters[3] = new SqlParameter("@cover", "");
            parameters[4] = new SqlParameter("@digest", content.app_msg_ext_info.digest);
            parameters[5] = new SqlParameter("@fileID", content.app_msg_ext_info.fileid);
            parameters[6] = new SqlParameter("@sourceUrl", content.app_msg_ext_info.source_url.Replace("&amp;", "&"));
            parameters[7] = new SqlParameter("@title", content.app_msg_ext_info.title);
            parameters[8] = new SqlParameter("@articleDate", content.comm_msg_info.datetime);
            parameters[9] = new SqlParameter("@fakeID", content.comm_msg_info.fakeid);
            parameters[10] = new SqlParameter("@commID", content.comm_msg_info.id);
            parameters[11] = new SqlParameter("@commStatus", content.comm_msg_info.status);
            parameters[12] = new SqlParameter("@commType", content.comm_msg_info.type);
            parameters[13] = new SqlParameter("@content", "");
            parameters[14] = new SqlParameter("@wechatID", wechatID);
            parameters[15] = new SqlParameter("@readNum", 0);
            parameters[16] = new SqlParameter("@likeNum", 0);
            parameters[17] = new SqlParameter("@folderName", folderName);
            parameters[18] = new SqlParameter("@wechatName", wechatName);
            parameters[19] = new SqlParameter("@wechatIcon", wechatIcon);
            if (IsNewArticle(content.comm_msg_info.id, content.app_msg_ext_info.title))
            {
                Directory.CreateDirectory(ArticleFolder + folderName);
                Tuple<string, string, string> details = GetDetailContent(content.app_msg_ext_info.content_url.Replace("&amp;", "&"), folderName, "Image", ArticleFolder);
                parameters[3].Value = "/Image/" + folderName + "/" + ReplaceAsLocalImg(content.app_msg_ext_info.cover, ArticleFolder + folderName);
                parameters[13].Value = details.Item1;
                parameters[15].Value = string.IsNullOrEmpty(details.Item2) ? "0" : details.Item2;
                parameters[16].Value = string.IsNullOrEmpty(details.Item3) ? "0" : details.Item3;
                result += SqlHelper.ExecteNonQueryText(sql, parameters);
            }

            foreach (multi_app_msg_item_list li in content.app_msg_ext_info.multi_app_msg_item_list)
            {
                ConsoleMessage(li.title);
                if (IsNewArticle(content.comm_msg_info.id, li.title))
                {
                    Directory.CreateDirectory(ArticleFolder + folderName);
                    parameters[0].Value = li.author;
                    parameters[1].Value = li.content_url.Replace("&amp;", "&");
                    parameters[2].Value = li.copyright_stat == null ? "" : li.copyright_stat;
                    parameters[3].Value = "/Image/" + folderName + "/" + ReplaceAsLocalImg(li.cover, ArticleFolder + folderName);
                    parameters[4].Value = li.digest;
                    parameters[5].Value = li.fileid;
                    parameters[6].Value = li.source_url;
                    parameters[7].Value = li.title;
                    Tuple<string, string, string> details = GetDetailContent(li.content_url.Replace("&amp;", "&"), folderName, "Image",ArticleFolder);
                    parameters[13].Value = details.Item1;
                    parameters[15].Value = string.IsNullOrEmpty(details.Item2) ? "0" : details.Item2;
                    parameters[16].Value = string.IsNullOrEmpty(details.Item3) ? "0" : details.Item3;
                    result += SqlHelper.ExecteNonQueryText(sql, parameters);
                }

            }

            return result;
        }

        public int AddWechatContent2(WechatCotent content, string wechatID, string wechatName)
        {
            int result = 0;
            string sql = @"INSERT INTO Campaign(Title,WechatName,Cover,Content,ReadNum,LikeNum,CampaignStatus) VALUES(@title,@wechatName,@cover,@content,@readNum,@likeNum,0)";
            string folderName = Guid.NewGuid().ToString("N");
            ConsoleMessage(content.app_msg_ext_info.title);
            SqlParameter[] parameters = new SqlParameter[6];
            parameters[0] = new SqlParameter("@title", content.app_msg_ext_info.title);
            parameters[1] = new SqlParameter("@wechatName", wechatName);
            parameters[2] = new SqlParameter("@cover", "");
            parameters[3] = new SqlParameter("@content", "");
            parameters[4] = new SqlParameter("@readNum", "");
            parameters[5] = new SqlParameter("@likeNum", "");

            if (IsNewCampaign(content.app_msg_ext_info.title, wechatName))
            {
                Directory.CreateDirectory(CampaignFolder + folderName);
                Tuple<string, string, string> details = GetDetailContent(content.app_msg_ext_info.content_url.Replace("&amp;", "&"), folderName, "Camp", CampaignFolder);
                parameters[2].Value = "/Camp/" + folderName + "/" + ReplaceAsLocalImg(content.app_msg_ext_info.cover, CampaignFolder + folderName);
                parameters[3].Value = details.Item1;
                parameters[4].Value = details.Item2;
                parameters[5].Value = details.Item3;
                result += SqlHelper.ExecteNonQueryText(sql, parameters);
            }

            foreach (multi_app_msg_item_list li in content.app_msg_ext_info.multi_app_msg_item_list)
            {
                ConsoleMessage(li.title);
                if (IsNewCampaign(li.title, wechatName))
                {
                    Directory.CreateDirectory(CampaignFolder + folderName);
                    Tuple<string, string, string> details = GetDetailContent(li.content_url.Replace("&amp;", "&"), folderName, "Camp", CampaignFolder);
                    parameters[0].Value = li.title;
                    parameters[2].Value = "/Camp/" + folderName + "/" + ReplaceAsLocalImg(li.cover, CampaignFolder + folderName);
                    parameters[3].Value = details.Item1;
                    parameters[4].Value = details.Item2;
                    parameters[5].Value = details.Item3;
                    result += SqlHelper.ExecteNonQueryText(sql, parameters);
                }

            }

            return result;
        }
        static public int AddWechatArticleContent(string content, int id)
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
                    Delay(DelayTime);
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
                Delay(DelayTime);  //系统延迟50毫秒，够少了吧！               
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
            string sql = "SELECT TOP 100 ID, Cover,Content FROM ArticleList ";
            DataSet ds = SqlHelper.ExecuteDataSetText(sql);
            if (ds.Tables != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RemoveArticle(dr);
                }
            }
        }

        private void WaitingJs(string key)
        {
            while (true)
            {
                if (!myWebBrowser.Document.Body.OuterHtml.Contains(key)) break;
                Delay(DelayTime);
            }
        }

        private string GetLastArticleDate(int wechatID)
        {
            string sql = "SELECT MAX(ArticleDate) FROM ArticleList WHERE WechatID = @id";
            SqlParameter id = new SqlParameter("@id", wechatID);
            object result = SqlHelper.ExecuteScalarText(sql, id);
            return result == null ? "" : result.ToString();
        }

        private bool IsNewArticle(string commID, string title)
        {
            string sql = "SELECT ID FROM ArticleList WHERE CommID = @commID AND Title = @title";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@commID", commID);
            parameters[1] = new SqlParameter("@title", title);
            object result = SqlHelper.ExecuteScalarText(sql, parameters);
            return result == null;
        }

        private bool IsNewCampaign(string title, string wechatName)
        {
            string sql = "SELECT COUNT(ID) FROM Campaign WHERE Title = @title AND WechatName = @wechatName";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@title", title);
            parameters[1] = new SqlParameter("@wechatName", wechatName);
            return Convert.ToInt32(SqlHelper.ExecuteScalarText(sql, parameters)) == 0;
        }

        private void RemoveArticle(DataRow dr)
        {
            string content = dr["content"].ToString();
            string cover = dr["cover"].ToString();
            Regex reg = new Regex(@"<img src=['|""]([^'|""]*)");
            Match match = reg.Match(content);
            if (match.Success)
            {
                string img = match.Groups[1].Value;
                ConsoleMessage(img);
            }
            else
            {
                MessageBox.Show(dr["ID"].ToString());
            }

        }

        private void RemoveFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        private void RemoveDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void UpdateIcon(string id, string icon)
        {
            string sql = "UPDATE WechatList SET Icon =@icon WHERE ID = @id";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@icon", icon);
            parameters[1] = new SqlParameter("@id", id);
            SqlHelper.ExecteNonQueryText(sql, parameters);
        }

        private void BtnDZDP_Click(object sender, EventArgs e)
        {
            InitialData("SELECT * FROM DZDP");
            GetIndexUrl3();
        }

        public void Save(DZDPCommentModel model)
        {
            string sql = @"INSERT INTO [dbo].[MallComment]
           ([MallID]
           ,[MallName]
           ,[AuthorID]
           ,[AuthorName]
           ,[AuthorIcon]
           ,[Content]
           ,[AddDate]
           ,[LikeNum]
           ,[CommentStatus])
     VALUES
           (@mallID
           ,@mallName
           ,@authorID
           ,@authorName
           ,@authorIcon
           ,@content
           ,@addDate
           ,@likeNum
           ,@commentStatus)";

            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@mallID", model.MallID);
            parameters[1] = new SqlParameter("@mallName", model.MallName);
            parameters[2] = new SqlParameter("@authorID", model.AuthorID);
            parameters[3] = new SqlParameter("@authorName", model.AuthorName);
            parameters[4] = new SqlParameter("@authorIcon", model.AuthorIcon);
            parameters[5] = new SqlParameter("@content", model.Content);
            parameters[6] = new SqlParameter("@addDate", model.AddDate);
            parameters[7] = new SqlParameter("@likeNum", model.LikeNum);
            parameters[8] = new SqlParameter("@commentStatus", model.CommentStatus);
            SqlHelper.ExecteNonQueryText(sql, parameters);
            //SQLHelper.ExecuteNonQueryText(sql, parameters);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            InitialData("SELECT * FROM WechatList WHERE ICON IS NULL AND WECHATSTATUS = 1");
            GetIndexUrl4();
        }
    }
}
