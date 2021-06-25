using System;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WFAPhoneBook
{
    public partial class BusinessContactsForm : Form
    {
        private readonly string _connString = @"Data Source=localhost;Initial Catalog=AddressBook;Integrated Security=True";

        private SqlDataAdapter _sqlDataAdapter; // Create the connection between the program and the database

        private DataTable _table;// an instance of the DB _table in code that enables us fill the data grid view
        public BusinessContactsForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BusinessContactsForm_Load(object sender, EventArgs e)
        {
            cboSearch.SelectedIndex = 0; // ensure that the first item in the combo box is auto selected
            dataGridView1.DataSource = bindingSource1; // Here we set the source of our data to the grid view

            GetData("Select * from PhoneBook");// Call a get data method and select all records from the _table called BizContacts
        }

        private void GetData(string selectCommand)
        {
            try
            {
                _sqlDataAdapter = new SqlDataAdapter(selectCommand, _connString);//Use the select statement command and the connection string
                _table = new DataTable();//Create a new data _table object
                _table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                _sqlDataAdapter.Fill(_table);// Fill the data _table
                bindingSource1.DataSource = _table;// set the data source on teh binding source to teh _table
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);// this shows an sql related message to the user. 
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlCommand command;// declares a new sql command object
            string insert = @"INSERT INTO PhoneBook(Id, Date_Added, Company, Website, Title, First_Name, Last_Name, Address, 
                                                        City, State, PostCode, Email, Mobile, Notes
                                                         )
                                                        VALUES(@Id, @Date_Added, @Company, @Website, @Title, @First_Name, @Last_Name, @Address, 
                                                        @City, @State, @PostCode, @Email, @Mobile, @Notes)";
            using (SqlConnection conn = new SqlConnection(_connString))//Ensures that we dispose low level resources
            {
                try
                { 
                    conn.Open();// open the connection
                    command = new SqlCommand(insert, conn);// creates the new sql command object
                    command.Parameters.AddWithValue(@"Id", 7);// read the value from the form and save it to the DB table.
                    command.Parameters.AddWithValue(@"Date_Added", dateTimePicker1.Value.Date);// read the value from the form and save it to the DB table. 
                    command.Parameters.AddWithValue(@"Company", txtCompany.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"Website", txtWebsite.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"Title", txtTitle.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"First_Name", txtFirstName.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"Last_Name", txtLastName.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"Address", txtAddress.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"City", txtCity.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"State", txtState.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"PostCode", txtPostalCode.Text);//read value from form and save to table
                    command.Parameters.AddWithValue(@"Email", txtEmail.Text);
                    command.Parameters.AddWithValue(@"Mobile", txtMobile.Text);
                    command.Parameters.AddWithValue(@"Notes", txtNotes.Text);
                    command.ExecuteNonQuery();// allow the pushing of records from the form to the table

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);// Show an exception if there is an error.
                }
            }
            GetData("SELECT * FROM PhoneBook");
            dataGridView1.Update();// update the datagrid view area with newly entered records.
        }
    }
}
