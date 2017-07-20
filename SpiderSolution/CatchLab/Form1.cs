using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.Navigate("https://mp.weixin.qq.com/mp/profile_ext?action=home&__biz=MjM5NjcwNzA2Mw==&scene=124#wechat_redirect");
        }
    }
}
