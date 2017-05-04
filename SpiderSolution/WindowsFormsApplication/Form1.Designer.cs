namespace WindowsFormsApplication
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSourceFile = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbSougou = new System.Windows.Forms.TextBox();
            this.lbSougou = new System.Windows.Forms.Label();
            this.myWebBrowser = new System.Windows.Forms.WebBrowser();
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnContent = new System.Windows.Forms.Button();
            this.prgb = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.openSourceFile = new System.Windows.Forms.OpenFileDialog();
            this.lbSourceFile = new System.Windows.Forms.Label();
            this.tbSourceFile = new System.Windows.Forms.TextBox();
            this.BtnDZDP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Location = new System.Drawing.Point(714, 6);
            this.btnSourceFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(158, 36);
            this.btnSourceFile.TabIndex = 0;
            this.btnSourceFile.Text = "抓取活动";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(714, 46);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(158, 36);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "抓取列表";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbSougou
            // 
            this.tbSougou.Location = new System.Drawing.Point(108, 56);
            this.tbSougou.Margin = new System.Windows.Forms.Padding(2);
            this.tbSougou.Name = "tbSougou";
            this.tbSougou.Size = new System.Drawing.Size(404, 21);
            this.tbSougou.TabIndex = 1;
            // 
            // lbSougou
            // 
            this.lbSougou.AutoSize = true;
            this.lbSougou.Location = new System.Drawing.Point(12, 59);
            this.lbSougou.Name = "lbSougou";
            this.lbSougou.Size = new System.Drawing.Size(65, 12);
            this.lbSougou.TabIndex = 3;
            this.lbSougou.Text = "抓取地址：";
            // 
            // myWebBrowser
            // 
            this.myWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myWebBrowser.Location = new System.Drawing.Point(276, 126);
            this.myWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.myWebBrowser.Name = "myWebBrowser";
            this.myWebBrowser.Size = new System.Drawing.Size(924, 571);
            this.myWebBrowser.TabIndex = 5;
            // 
            // rtbResult
            // 
            this.rtbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rtbResult.Location = new System.Drawing.Point(14, 126);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbResult.Size = new System.Drawing.Size(256, 571);
            this.rtbResult.TabIndex = 6;
            this.rtbResult.Text = "";
            // 
            // tbAddress
            // 
            this.tbAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddress.Location = new System.Drawing.Point(108, 100);
            this.tbAddress.Margin = new System.Windows.Forms.Padding(2);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(1092, 21);
            this.tbAddress.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "来源网址：";
            // 
            // btnContent
            // 
            this.btnContent.Location = new System.Drawing.Point(886, 47);
            this.btnContent.Margin = new System.Windows.Forms.Padding(2);
            this.btnContent.Name = "btnContent";
            this.btnContent.Size = new System.Drawing.Size(158, 36);
            this.btnContent.TabIndex = 0;
            this.btnContent.Text = "清理文章";
            this.btnContent.UseVisualStyleBackColor = true;
            this.btnContent.Click += new System.EventHandler(this.btnContent_Click);
            // 
            // prgb
            // 
            this.prgb.Location = new System.Drawing.Point(886, 8);
            this.prgb.Name = "prgb";
            this.prgb.Size = new System.Drawing.Size(320, 23);
            this.prgb.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1048, 47);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "抓取头像";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // openSourceFile
            // 
            this.openSourceFile.FileName = "openSourceFile";
            // 
            // lbSourceFile
            // 
            this.lbSourceFile.AutoSize = true;
            this.lbSourceFile.Location = new System.Drawing.Point(12, 19);
            this.lbSourceFile.Name = "lbSourceFile";
            this.lbSourceFile.Size = new System.Drawing.Size(77, 12);
            this.lbSourceFile.TabIndex = 3;
            this.lbSourceFile.Text = "源文件路径：";
            // 
            // tbSourceFile
            // 
            this.tbSourceFile.Location = new System.Drawing.Point(108, 16);
            this.tbSourceFile.Margin = new System.Windows.Forms.Padding(2);
            this.tbSourceFile.Name = "tbSourceFile";
            this.tbSourceFile.Size = new System.Drawing.Size(404, 21);
            this.tbSourceFile.TabIndex = 1;
            // 
            // BtnDZDP
            // 
            this.BtnDZDP.Location = new System.Drawing.Point(552, 6);
            this.BtnDZDP.Margin = new System.Windows.Forms.Padding(2);
            this.BtnDZDP.Name = "BtnDZDP";
            this.BtnDZDP.Size = new System.Drawing.Size(158, 36);
            this.BtnDZDP.TabIndex = 0;
            this.BtnDZDP.Text = "抓取评论";
            this.BtnDZDP.UseVisualStyleBackColor = true;
            this.BtnDZDP.Click += new System.EventHandler(this.BtnDZDP_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 709);
            this.Controls.Add(this.prgb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.rtbResult);
            this.Controls.Add(this.myWebBrowser);
            this.Controls.Add(this.lbSougou);
            this.Controls.Add(this.lbSourceFile);
            this.Controls.Add(this.tbSougou);
            this.Controls.Add(this.tbSourceFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnContent);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.BtnDZDP);
            this.Controls.Add(this.btnSourceFile);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbSougou;
        private System.Windows.Forms.Label lbSougou;
        private System.Windows.Forms.WebBrowser myWebBrowser;
        private System.Windows.Forms.RichTextBox rtbResult;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnContent;
        private System.Windows.Forms.ProgressBar prgb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openSourceFile;
        private System.Windows.Forms.Label lbSourceFile;
        private System.Windows.Forms.TextBox tbSourceFile;
        private System.Windows.Forms.Button BtnDZDP;
    }
}

