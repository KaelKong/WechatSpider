using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication
{
    public class WechatList
    {
        public List<WechatCotent> list { get; set; }
    }

    public class WechatCotent
    {
        public app_msg_ext_info app_msg_ext_info { get; set; }
        public comm_msg_info comm_msg_info { get; set; }
    }

    public class app_msg_ext_info
    {
        public string author { get; set; }
        public string content { get; set; }
        public string content_url { get; set; }
        public string copyright_stat { get; set; }
        public string cover { get; set; }
        public string digest { get; set; }
        public string fileid { get; set; }
        public string is_multi { get; set; }
        public List<multi_app_msg_item_list> multi_app_msg_item_list { get; set; }
        public string source_url { get; set; }
        public string subtype { get; set; }
        public string title { get; set; }
    }

    public class multi_app_msg_item_list
    {
        public string author { get; set; }
        public string content { get; set; }
        public string content_url { get; set; }
        public string copyright_stat { get; set; }
        public string cover { get; set; }
        public string digest { get; set; }
        public string fileid { get; set; }
        public string source_url { get; set; }
        public string title { get; set; }
    }

    public class comm_msg_info
    {
        public string content { get; set; }
        public string datetime { get; set; }
        public string fakeid { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }
}
