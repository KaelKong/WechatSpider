using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication
{
    public abstract class OleDbHelper
    {
        public static DataTable ExecuteQuery(string connStr, string commandText)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter oda = new OleDbDataAdapter(commandText, connStr);
            oda.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string connStr, string commandText)
        {
            int result;
            OleDbConnection conn = new OleDbConnection(connStr);
            conn.Open();
            using (OleDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = commandText;
                result = cmd.ExecuteNonQuery();
            }
            conn.Close();

            return result;
        }
    }
}
