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
    public partial class tabl1 : Form
    {
        public tabl1()
        {
            InitializeComponent();
            h.ConStr = "Server=localhost; port=3306;  User=root; Password=201222; Database=biblio; charset=cp1251;";
         
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

        private void Osoba_Load(object sender, EventArgs e)
        {
            this.Height = 270;
            DataTable minmaxValue = h.myFunDt("SELECT min(DNO),max(DNO)," +
                "MIN(KursStaz),MAX(KursStaz) FROM Osoba");
            dtpIn.Value = Convert.ToDateTime(minmaxValue.Rows[0][0].ToString());
            dtpOut.Value = Convert.ToDateTime(minmaxValue.Rows[0][1].ToString());
            txtKSIn.Text = minmaxValue.Rows[0][2].ToString();
            txtKSOut.Text = minmaxValue.Rows[0][3].ToString();
            minmaxValue = h.myFunDt("SELECT DISTINCT SpecialtyO FROM Osoba");
            cmbNameField.Items.Add("");
            for (int i = 0; i < minmaxValue.Rows.Count; i++)
                cmbNameField.Items.Add(minmaxValue.Rows[i][0].ToString());
            cmbSex.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSex.Items.Add("");
            cmbSex.Items.Add("Ч");
            cmbSex.Items.Add("Ж");
            cmbSex.Text = "Ч";
            h.bs1 = new BindingSource();
            h.bs1.DataSource = h.myFunDt("SELECT * FROM Osoba");
            dataGridView1.DataSource = h.bs1;
            bindingNavigator1.BindingSource = h.bs1;

            h.bs1.Sort = "FirstName";
            h.bs1.Sort = dataGridView1.Columns[2].Name;

            dataGridView1.DefaultCellStyle.SelectionBackColor= Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Black;

            dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver;
        }

        private void tabl1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            label1.Visible = true;
            label1.Text = "Пошук:";
            textBox1.Focus();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for(int i=0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for(int j = 0;j< dataGridView1.ColumnCount;j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox1.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
           

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            label1.Visible = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (btnFilter.Checked)
            {
                this.Height = 430;
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
                h.bs1.Filter = "";
                this.Height = 270;
            }
        }

        private void btnOkFilter_Click(object sender, EventArgs e)
        {
            string strFilter = "idO > 0";
            strFilter += "AND (DNO >= '" + dtpIn.Value.ToString("yyyy-MM-dd") + "'" +
                "AND DNO <= '" + dtpOut.Value.ToString("yyyy-MM-dd") + "')";
            if ((txtKSIn.Text != "") && (txtKSOut.Text != ""))
            {
                strFilter += " AND (KursStaz >= '" + int.Parse(txtKSIn.Text) +
                    "' AND KursStaz <= '" + int.Parse(txtKSOut.Text) + "') ";
            }
            else if ((txtKSIn.Text == "") && (txtKSOut.Text != ""))
                strFilter += " AND (KursStaz <= '" + int.Parse(txtKSOut.Text) + "') ";
            else if ((txtKSIn.Text != "") && (txtKSOut.Text == ""))
                strFilter += " AND (KursStaz >= '" + int.Parse(txtKSIn.Text) + "')";
            if (cmbNameField.Text != "")
                strFilter += " AND (SpecialtyO LIKE '%" + cmbNameField.Text + "%')";
            if (String.Equals(cmbSex.Text, "Ч"))
                strFilter += " AND Stat = 'TRUE' ";
            else if (String.Equals(cmbSex.Text, "Ж"))
                strFilter += " AND Stat = 'FALSE'";
            h.bs1.Filter = strFilter;
        }

        private void btnCancleFilter_Click(object sender, EventArgs e)
        {
            h.bs1.Filter = "";
        }

        private void AddNew_Click(object sender, EventArgs e)
        {
            newAdd f1add = new newAdd();
            f1add.ShowDialog();
            h.bs1.DataSource = h.myFunDt("select * from Katalog");
            dataGridView1.DataSource = h.bs1;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            h.curVa10 = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            h.keyName = dataGridView1.Columns[0].Name;

            delete f3= new delete();
            f3.ShowDialog();

            h.bs1.DataSource = h.myFunDt("SELECT * FROM Katalog");
            dataGridView1.DataSource = h.bs1;   
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            int curColidx = dataGridView1.CurrentCellAddress.X;
            int curRowidx = dataGridView1.CurrentCellAddress.Y;
            string curColName0 = dataGridView1.Columns[0].Name;
            string curColName = dataGridView1.Columns[curColidx].Name;
            h.curVa10 = dataGridView1[0,curRowidx].Value.ToString();
            string newCurCellVal = e.Value.ToString();
            if(curColName == "NameK" || curColName == "AvtorK" || curColName == "VydanK")
            {
                newCurCellVal = "'" + newCurCellVal + "'";
            }
            string sqlStr = "UPDATE Katalog SET " + curColName + "=" + newCurCellVal + "WHERE" + curColName0 + "=" + h.curVa10;
            using (MySqlConnection con = new MySqlConnection(h.ConStr))
            {
                MySqlCommand cmd =new MySqlCommand(sqlStr, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            h.curVa10 = dataGridView1[0,dataGridView1.CurrentRow.Index].Value.ToString();
            h.keyName = dataGridView1.Columns[0].Name;

            Edit f4 = new Edit();
            f4.ShowDialog();

            h.bs1.DataSource = h.myFunDt("SELECT * FROM Katalog");
            dataGridView1.DataSource = h.bs1;
        }
    }
}
