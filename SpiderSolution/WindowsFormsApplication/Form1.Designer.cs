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
            this.tbSourceFile = new System.Windows.Forms.TextBox();
            this.tbSougou = new System.Windows.Forms.TextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.lbSourceFile = new System.Windows.Forms.Label();
            this.lbSougou = new System.Windows.Forms.Label();
            this.openSourceFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Location = new System.Drawing.Point(714, 6);
            this.btnSourceFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(158, 36);
            this.btnSourceFile.TabIndex = 0;
            this.btnSourceFile.Text = "源文件";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(714, 46);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(158, 36);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbSourceFile
            // 
            this.tbSourceFile.Location = new System.Drawing.Point(108, 16);
            this.tbSourceFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbSourceFile.Name = "tbSourceFile";
            this.tbSourceFile.Size = new System.Drawing.Size(586, 21);
            this.tbSourceFile.TabIndex = 1;
            // 
            // tbSougou
            // 
            this.tbSougou.Location = new System.Drawing.Point(108, 56);
            this.tbSougou.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbSougou.Name = "tbSougou";
            this.tbSougou.Size = new System.Drawing.Size(586, 21);
            this.tbSougou.TabIndex = 1;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.webBrowser1.Location = new System.Drawing.Point(0, 86);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(10, 10);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(878, 665);
            this.webBrowser1.TabIndex = 2;
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
            // lbSougou
            // 
            this.lbSougou.AutoSize = true;
            this.lbSougou.Location = new System.Drawing.Point(12, 59);
            this.lbSougou.Name = "lbSougou";
            this.lbSougou.Size = new System.Drawing.Size(65, 12);
            this.lbSougou.TabIndex = 3;
            this.lbSougou.Text = "抓取地址：";
            // 
            // openSourceFile
            // 
            this.openSourceFile.FileName = "openSourceFile";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 751);
            this.Controls.Add(this.lbSougou);
            this.Controls.Add(this.lbSourceFile);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.tbSougou);
            this.Controls.Add(this.tbSourceFile);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSourceFile);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbSourceFile;
        private System.Windows.Forms.TextBox tbSougou;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label lbSourceFile;
        private System.Windows.Forms.Label lbSougou;
        private System.Windows.Forms.OpenFileDialog openSourceFile;
    }
}

