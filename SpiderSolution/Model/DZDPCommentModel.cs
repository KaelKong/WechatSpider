using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DZDPCommentModel
    {
        public int ID { get; set; }
        public int MallID { get; set; }
        public string MallName { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string AuthorIcon { get; set; }
        public string Content { get; set; }
        public string AddDate { get; set; }
        public string LikeNum { get; set; }
        public int CommentStatus { get; set; }
    }
}
