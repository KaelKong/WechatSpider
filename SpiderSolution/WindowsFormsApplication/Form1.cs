using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        private string ConnStr = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = {0}; Extended Properties = 'Excel 8.0;HDR=Yes;IMEX=1;'";

        public Form1()
        {
            InitializeComponent();
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
            string connStr = string.Format(ConnStr, tbSourceFile.Text.Trim());
            OleDbConnection conn = new OleDbConnection(connStr);
            conn.Open();
            using (OleDbCommand cmd = conn.CreateCommand())
            {
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });

                cmd.CommandText = "SELECT * FROM [" + schemaTable.Rows[0]["TABLE_NAME"].ToString() + "]";
            }
        }
    }
}
