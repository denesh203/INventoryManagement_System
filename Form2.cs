using Net.Pkcs11Interop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace communicationapplication
{
    public partial class Form2 : Form
    {
        string conn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"F:\\Abc Project\\inventory Management system\\2communicationapplication\\Database1.mdf\";Integrated Security=True;";
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\Abc Project\inventory Management system\2communicationapplication\Database1.mdf"";Integrated Security=True;");
        SqlDataAdapter sAdapter;

        DataTable sTable;
        SqlCommand cmd;
        public Form2()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form_Closing);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime d = new DateTime();
            d = DateTime.Now;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Table1 WHERE Id = @Id  ", connection);
                cmdCount.Parameters.AddWithValue("@Id", textBox4.Text);
                int count = (int)cmdCount.ExecuteScalar();
                if (count > 0)
                {
                    SqlCommand command = new SqlCommand("SELECT Quantity  FROM Table1 WHERE Id = @Id ", connection);
                    command.Parameters.AddWithValue("@Id", textBox4.Text);
                    SqlCommand cmd = new SqlCommand("Update  Table1  set Name = @Name　, Quantity = @Quantity　, Model = @Model,　Arrived= @Arrived, Maker = @Maker  where Id = @Id  ", connection);
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Id", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Model", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Maker", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Arrived", d.ToString("yyyy / MM / dd"));
                    cmd.Parameters.AddWithValue("@Quantity", numericUpDown1.Value);
                    cmd.ExecuteNonQuery();
                    DisplayData();
                }
                connection.Close();
                cmt.Text = "updated";
                cmt.BackColor = Color.LightGreen;
            }
            else 
            {
                cmt.Text = "please enter all the details";
                cmt.BackColor = Color.Yellow;
            }
        }
        int index;
    
            private void button10_Click(object sender, EventArgs e)
        {
            DateTime d = new DateTime();
            d = DateTime.Now;
            if (textBox1.Text != "" && textBox2.Text != "" && numericUpDown1.Value > 0)
            {
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Table1 WHERE Name = @Name And Model = @Model ", connection);
                cmdCount.Parameters.AddWithValue("@Name", textBox1.Text);
                cmdCount.Parameters.AddWithValue("@Model", textBox2.Text);
                int count = (int)cmdCount.ExecuteScalar();
                int i = 0;
                if (count > 0)
                {
                    SqlCommand command = new SqlCommand("SELECT Quantity FROM Table1 WHERE Name = @Name And Model = @Model ", connection);
                    command.Parameters.AddWithValue("@Name", textBox1.Text);
                    command.Parameters.AddWithValue("@Model", textBox2.Text);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            i = Convert.ToInt32(reader["Quantity"]);

                        }
                    }
                    SqlCommand cmd = new SqlCommand("Update  Table1  set  Quantity = @Quantity,Arrived= @Arrived,Total=@Total,Price=@Price where Name = @Name And Model = @Model  And Maker = @Maker", connection);
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Model", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Quantity", i + numericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@Arrived", d.ToString("yyyy / MM / dd"));
                   // cmd.Parameters.AddWithValue("@Id", textBox4.Text);

                    cmd.Parameters.AddWithValue("@Maker", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Price", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Total", textBox6.Text);

                    cmd.ExecuteNonQuery();
                    dataGridView1.DataSource = null;
                    DisplayData();
                    cmt.Text = "データは前に追加された行で更新されます";
                    cmt.BackColor = Color.LightGreen;
                }
                else 
                {
                    

                    cmd = new SqlCommand("insert into [Table1] (Name,Model,Maker,Quantity,Arrived,Total,Price,Id) values(@Name,@Model,@Maker,@Quantity,@Arrived,@Total,@Price,@Id)", connection);
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@Model", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Quantity", i + numericUpDown1.Value);
                    cmd.Parameters.AddWithValue("@Arrived", d.ToString("yyyy / MM / dd"));
                   
                    //cmd.Parameters.AddWithValue("@Id", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Maker", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Price", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Total", textBox6.Text);
                    int iid = 1;
                    for (int j = 0; j < dataGridView1.RowCount; j++)
                    {
                        iid = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value) + 1;
                       
                    }

                    cmd.Parameters.AddWithValue("@Id", iid .ToString());
                    cmd.ExecuteNonQuery();

                    dataGridView1.DataSource = null;
                    DisplayData();
                }
                connection.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                numericUpDown1.Value = 0;
                textBox5.Text = "";
                textBox6.Text = "";
                cmt.Text = "Data successfully inserted";
                cmt.BackColor = Color.LightGreen;
            }
            else
            {
               
                cmt.Text = "Enter all the details correctly";
                cmt.BackColor = Color.Yellow;

            }
        }
        private void DisplayData()
        {
            con.Open();
            sTable = new DataTable();
            sAdapter = new SqlDataAdapter("SELECT * FROM [Table1] ", con);
            sAdapter.Fill(sTable);
            dataGridView1.DataSource = sTable;
          //  updatesn();
            //this.dataGridView1.Columns["Id"].Visible = false;
            con.Close();


        }
        private void updatesn()
        {
            int Id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            con.Open();

            string str = string.Format("Update [Table1] set Id=Id - 1 where  Id>'" + Id + "'");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = str;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to delete Yes/No", "Delete", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    DataTable ds = new DataTable();
                    using (SqlCommand cmd = con.CreateCommand())
                    {

                        con.Open();
                        int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                        cmd.CommandText = "Delete from [Table1] where id='" + id + "'";

                       
                        cmd.ExecuteNonQuery();
                        con.Close();
                         updatesn();
                        DisplayData();
                        dataGridView1.Update();
                        dataGridView1.Refresh();
                    }
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    numericUpDown1.Value = 0;
                    textBox5.Text = "";
                    textBox6.Text = "";
                    cmt.Text = "Data delete sucessfully";
                    cmt.BackColor = Color.LightGreen;
                }
            }
            else
            {
                MessageBox.Show("select the data to delete");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label9.Text = DateTime.Now.ToString(" yyyy年M月d日");
            DisplayData();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
        public void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // DialogResult dialogResult = MessageBox.Show("削除してよろしいですか Yes/No", "Delete", MessageBoxButtons.YesNo);
            Application.Exit();
            
        }
       // int index;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            if (index >= 0)
                {

                    DataGridViewRow selectedRow = dataGridView1.Rows[index];
                    textBox1.Text = selectedRow.Cells[1].Value.ToString();
                    textBox2.Text = selectedRow.Cells[2].Value.ToString();
                    textBox3.Text = selectedRow.Cells[3].Value.ToString();
                    numericUpDown1.Value = (int)selectedRow.Cells[6].Value;
                    textBox5.Text = selectedRow.Cells[5].Value.ToString();
                    textBox6.Text = selectedRow.Cells[7].Value.ToString();
                    textBox4.Text = selectedRow.Cells[0].Value.ToString();

                }
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            string Message = textBox7.Text;

           
            if (textBox7.TextLength > 0)
            {
                String someValue = textBox7.Text;
                con.Open();
                DataTable ds = new DataTable();
               SqlDataAdapter adp = new SqlDataAdapter("SELECT *  FROM table1 WHERE [Name] LIKE N'%" + textBox7.Text + "%' OR [Model] LIKE '%" + textBox7.Text + "%'", con);

                
                adp.Fill(ds);
                dataGridView1.DataSource = ds;
                con.Close();
            }

            else
            {
                DisplayData();
                //MessageBox.Show("モデル名を入力して検索してください");
            }

        }

        
    }
}
