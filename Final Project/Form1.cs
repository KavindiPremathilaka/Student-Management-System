using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Hello from the console!");
           

            SqlConnection connect = new SqlConnection(@"Data Source=Kavindi\SQLEXPRESS01;Initial Catalog=Student;Persist Security Info=True;Integrated Security=true;");

            Console.WriteLine("Hello from the console 1!");
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    Console.WriteLine("Hello from the console 2!");
                    connect.Open();
                    Console.WriteLine("Hello from the console 3!");
                    string selectData = "SELECT * FROM Logins WHERE username = @username COLLATE SQL_Latin1_General_CP1_CS_AS AND password = @password";
                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@username", txtusername.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        if (table.Rows.Count >= 1)
                        {

                            if (IsPasswordComplex(txtPassword.Text))
                            {
                                frmRegistration sForm = new frmRegistration();
                                sForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login credentials, please check Username and Password and try again", "Invalid login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Login credentials, please check Username and Password and try again", "Invalid login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connection DB: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }


            bool IsPasswordComplex(string password)
            {

                return password.Any(char.IsUpper) && password.Any(char.IsLower);
            }





        }

        private void NameTxt_TextChanged(object sender, EventArgs e)
        {

        }

        
        
    }
}
