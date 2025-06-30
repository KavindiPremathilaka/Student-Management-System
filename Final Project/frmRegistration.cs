using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Final_Project
{
    public partial class frmRegistration : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=Kavindi\SQLEXPRESS01;Initial Catalog=Student;Persist Security Info=True;Integrated Security=true;");

        private string gender;
        public frmRegistration()
        {
            InitializeComponent();
            LoadDataIntoComboBox();
        }



        private void LoadDataIntoComboBox()
        {
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
  
                string query = "SELECT regNo FROM Registration";
                SqlCommand cmd = new SqlCommand(query, connect);

                SqlDataReader reader = cmd.ExecuteReader();
                
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["regNo"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        private void frmRegistration_Load(object sender, EventArgs e)
        {
            txtFirstName.Select();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    String insertData = "INSERT INTO registration (firstName, lastName,dateOfBirth, gender ,address, email ,mobilePhone, homePhone ,parentName ,nic,contactNo)" +
                        "VALUES(@firstName, @lastName, @dateOfBirth, @gender ,@address, @email ,@mobilePhone, @homePhone ,@parentName ,@nic, @contactNo)";

                    using (SqlCommand cmd = new SqlCommand(insertData, connect))
                    {
                        cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@dateOfBirth", dateofbirth.Value.Date.ToString().Trim());
                        cmd.Parameters.AddWithValue("@gender", gender.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@mobilePhone", int.Parse(txtMobilePhone.Text.Trim()));
                        cmd.Parameters.AddWithValue("@homePhone", int.Parse(txtHomePhone.Text.Trim()));
                        cmd.Parameters.AddWithValue("@parentName", txtParentName.Text.Trim());
                        cmd.Parameters.AddWithValue("@nic", txtNIC.Text.Trim());
                        cmd.Parameters.AddWithValue("@contactNo", int.Parse(txtContactNumber.Text.Trim()));

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Added Succesfully", "Register Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataIntoComboBox();
                        BindData();
                        ClearInputFields();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connection DB: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (connect.State == ConnectionState.Open)
                    {
                        connect.Close();
                    }
                }
            }
        }

        void BindData()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM registration", connect);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dc = new DataTable();
            da.Fill(dc);
          
        }




        private void ClearInputFields()
        {
           
           comboBox1.Text = "";
          
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dateofbirth.Value = DateTime.Now;
            gender = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtMobilePhone.Text = "";
            txtHomePhone.Text = "";
            txtParentName.Text = "";
            txtNIC.Text = "";
            txtContactNumber.Text = "";


            txtFirstName.Focus();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open();


                    if (comboBox1.SelectedItem != null)
                    {
                        string selectedRegNo = comboBox1.SelectedItem.ToString();
                        DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {

                            string deleteQuery = "DELETE FROM Registration WHERE regNo = @RegNo";

                            using (SqlCommand cmd = new SqlCommand(deleteQuery, connect))
                            {
                                cmd.Parameters.AddWithValue("@RegNo", selectedRegNo);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record Deleted Successfully", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    LoadDataIntoComboBox();
                                    ClearInputFields();


                                    BindData();
                                }
                                else
                                {
                                    MessageBox.Show("No records deleted", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            {
                txtFirstName.Clear();
                txtLastName.Clear();
                dateofbirth.Value = DateTime.Now;
                gender = "";
                txtAddress.Clear();
                txtEmail.Clear();
                txtMobilePhone.Clear();
                txtHomePhone.Clear();
                txtParentName.Clear();
                txtNIC.Clear();
                txtContactNumber.Clear();
            }

            txtFirstName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();


                    if (comboBox1.SelectedItem != null)
                    {
                        string selectedRegNo = comboBox1.SelectedItem.ToString();

                        string updateQuery = "UPDATE Registration SET " +
                            "firstName = @FirstName, " +
                            "lastName = @LastName, " +
                            "dateOfBirth = @DateOfBirth, " +
                            "gender = @Gender, " +
                            "address = @Address, " +
                            "email = @Email, " +
                            "mobilePhone = @MobilePhone, " +
                            "homePhone = @HomePhone, " +
                            "parentName = @ParentName, " +
                            "nic = @Nic, " +
                            "contactNo = @ContactNo " +
                            "WHERE regNo = @RegNo";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@RegNo", selectedRegNo);
                            cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateOfBirth", dateofbirth.Value.Date);
                            cmd.Parameters.AddWithValue("@Gender", radioButton1.Checked ? "Male" : "Female");
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@MobilePhone", int.Parse(txtMobilePhone.Text.Trim()));
                            cmd.Parameters.AddWithValue("@HomePhone", int.Parse(txtHomePhone.Text.Trim()));
                            cmd.Parameters.AddWithValue("@ParentName", txtParentName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Nic", txtNIC.Text.Trim());
                            cmd.Parameters.AddWithValue("@ContactNo", int.Parse(txtContactNumber.Text.Trim()));



                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record Updated Successfully", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);


                                DisplayDetails(selectedRegNo);


                                BindData();
                            }
                            else
                            {
                                MessageBox.Show("No records updated", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying details: " + ex.Message);
                }
                finally
                {
                    if (connect.State == ConnectionState.Open)
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                gender = "Female";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRegNo = comboBox1.SelectedItem.ToString();
            DisplayDetails(selectedRegNo);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 sForm = new Form1();
            sForm.Show();
            this.Hide();
        }

        private void DisplayDetails(string regNo)
        {
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                string query = "SELECT * FROM Registration WHERE regNo = @RegNo";
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@RegNo", regNo);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtFirstName.Text = reader["firstName"].ToString();
                    txtLastName.Text = reader["lastName"].ToString();
                    dateofbirth.Text = reader["dateOfBirth"].ToString();
                    txtAddress.Text = reader["address"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    txtMobilePhone.Text = reader["mobilePhone"].ToString();
                    txtHomePhone.Text = reader["homePhone"].ToString();
                    txtParentName.Text = reader["parentName"].ToString();
                    txtNIC.Text = reader["nic"].ToString();
                    txtContactNumber.Text = reader["contactNo"].ToString();

                    string genderValue = reader["gender"].ToString();
                    radioButton1.Checked = (genderValue == "Male");
                    radioButton2.Checked = (genderValue == "Female");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying details: " + ex.Message);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
    }
}

