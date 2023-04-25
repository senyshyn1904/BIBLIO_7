using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BIBLIO
{
    public partial class newAdd : Form
    {
        public newAdd()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using(MySqlConnection con = new MySqlConnection(h.ConStr))
            {
                string tb1 = textBox1.Text;
                string tb2 = textBox2.Text;
                string tb3 = textBox3.Text;
                string tb4 = textBox4.Text;
                string tb5 = textBox5.Text;
                string tb6 = textBox6.Text;
                string sql = "INSERT INTO Katalog" +
                    "(IPk, NameK, AvtorK, KilkK,RikVydK, VydavnK)" +
                    " VALUES (@TK1 , @TK2, @TK3, @TK4, @TK5, @TK6)";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@TK1", tb1);
                cmd.Parameters.AddWithValue("@TK2", tb2);
                cmd.Parameters.AddWithValue("@TK3", tb3);
                cmd.Parameters.AddWithValue("@TK4", tb4);
                cmd.Parameters.AddWithValue("@TK5", tb5);
                cmd.Parameters.AddWithValue("@TK6", tb6);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Додавання запису пройшло вдало");
            }
            this.Close();
        }

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
    }
}
