using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BIBLIO
{
    public partial class delete : Form
    {
        public static class h
        {
            public static string EncriptedPassword_MD5(string s)
            {
                if (string.Compare(s, "null", true) == 0)
                    return "NULL";
                byte[] bytes = Encoding.Unicode.GetBytes(s);
                MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
                byte[] byteHach = CSP.ComputeHash(bytes);
                string hash = string.Empty;
                foreach (byte b in byteHach)
                    hash += String.Format("{0:x2}", b);
                return hash;

            }

            public static string EncriptedPassword_SHA256(string s)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));
                    StringBuilder stringbuilder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        stringbuilder.Append(bytes[i].ToString("x2"));
                    }
                    return stringbuilder.ToString();
                }
            }

            public static string ConStr { get; set; }
            public static string typeUser { get; set; }
            public static string nameUser { get; set; }
            public static BindingSource bs1 { get; set; }

            public static string curVa10 { get; set; }
            public static string keyName { get; set; }

            public static DataTable myFunDt(string commandString)
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(h.ConStr))
                {
                    MySqlCommand cmd = new MySqlCommand(commandString, con);
                    try
                    {
                        con.Open();
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dt.Load(dr);
                            }
                        }
                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Неможливо з'єднатися з SQL-сервером! \nПеревірте наявність Інтернету...", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return dt;

            }
        }

        public delete()
        {
            InitializeComponent();
        }

        private void delete_Load(object sender, EventArgs e)
        {
            textBox1.Text = h.keyName + " = " + h.curVa10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlStr = "DELETE FROM Katalog WHERE " + textBox1.Text;
            if(MessageBox.Show("Ви впевнені, що хочете видалити запис", "Видалення",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(h.ConStr))
                {
                    MySqlCommand cmd = new MySqlCommand(sqlStr, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();    
                }
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
