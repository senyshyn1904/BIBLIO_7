﻿using System;
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
using System.Security.Cryptography;

namespace BIBLIO
{
    public partial class LogIn : Form
    {
        

        public string[,] matrix;
        DataTable dt;
 
        public LogIn()
        {
            InitializeComponent();
            h.ConStr = "Server=localhost; port=3306;  User=root; Password=201222; Database=biblio; charset=cp1251;";
            dt = h.myFunDt("SELECT * FROM username;");

            int kilkz = dt.Rows.Count;
            matrix = new string[kilkz,4];

            for(int i=0; i<kilkz ;i++)
            {
                matrix[i, 0] = dt.Rows[i].Field<int>("id").ToString();
                matrix[i, 1] = dt.Rows[i].Field<string>("UserName");
                matrix[i, 2] = dt.Rows[i].Field<int>("Type").ToString();
                matrix[i, 3] = dt.Rows[i].Field<string>("Password").ToString();
                cbxUser.Items.Add(matrix[i,1]);
            }

            cbxUser.Text = matrix[0, 1];
            txtPassword.UseSystemPasswordChar = true;
            cbxUser.Focus();    
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
            public static   BindingSource bs1 { get; set; }

            public static DataTable myFunDt(string commandString)
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(h.ConStr))
                {
                    MySqlCommand cmd =new MySqlCommand(commandString, con);
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
                    catch(Exception ex)
                    {
                        MessageBox.Show("Неможливо з'єднатися з SQL-сервером! \nПеревірте наявність Інтернету...", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return dt;
                    
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void LogIn_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Avtorization();
        }

        private void Avtorization()
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (String.Equals(cbxUser.Text.ToUpper(), matrix[i, 1].ToUpper()))
                {
                    if (String.Equals(h.EncriptedPassword_MD5(txtPassword.Text), matrix[i, 3]))
                    {
                        this.Hide();
                        myBD f1 = new myBD();
                        f1.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Введіть правильний пароль!", "Помилка авторизації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Avtorization();
            else if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
