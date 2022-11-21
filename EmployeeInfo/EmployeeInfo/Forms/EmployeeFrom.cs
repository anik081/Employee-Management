using EmployeeInfo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeInfo.Forms
{
    public partial class EmployeeFrom : Form
    {
        string _token = string.Empty;
        public EmployeeFrom(string token)
        {
            InitializeComponent();
            _token = token;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ClearFields();
                txtId.Text = dataEmployees.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dataEmployees.Rows[e.RowIndex].Cells[1].Value.ToString();
                dtDOB.Value = DateTime.Parse(dataEmployees.Rows[e.RowIndex].Cells[2].Value.ToString());
                txtSex.Text = dataEmployees.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtMovieName.Text = dataEmployees.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtMovieRating.Text = dataEmployees.Rows[e.RowIndex].Cells[5].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ClearFields();
                txtId.Text = dataEmployees.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dataEmployees.Rows[e.RowIndex].Cells[1].Value.ToString();
                dtDOB.Value = DateTime.Parse(dataEmployees.Rows[e.RowIndex].Cells[2].Value.ToString());
                txtSex.Text = dataEmployees.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtMovieName.Text = dataEmployees.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtMovieRating.Text = dataEmployees.Rows[e.RowIndex].Cells[5].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearchBox.Text))
                {
                    MessageBox.Show("Please insert movie name");
                    return;
                }
                var responseString = "";
                string SearchEmployeeByMovieUrl = ConfigurationManager.AppSettings["SearchEmployeeByMovieUrl"];
                string apiURL = string.Format(SearchEmployeeByMovieUrl + txtSearchBox.Text);
                //string apiURL = string.Format("https://localhost:44385/api/Employee/Movie/" + txtSearchBox.Text);
                //string json = JsonConvert.SerializeObject(requestBody);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "GET";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + _token);
                request.ContentType = "application/json";

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                //{
                //    streamWriter.Write(json);
                //}
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                //string result = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    responseString = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                List<EmployeeModel> employeeList = JsonConvert.DeserializeObject<List<EmployeeModel>>(responseString);
                if (employeeList.Count != 0)
                {
                    dataEmployees.DataSource = employeeList;
                }
                else
                {
                    MessageBox.Show($"No employee found with movie name {txtSearchBox.Text}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void EmployeeFrom_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Please select an item!");
                    return;
                }                
                var responseString = "";
                object requestBody = new
                {
                    id = Int32.Parse(txtId.Text),
                    name = txtName.Text,
                    dob = dtDOB.Value.ToString(),
                    sex = txtSex.Text,
                    movieName = txtMovieName.Text,
                    movieRating = txtMovieRating.Text
                };
                string UpdateEmployeeById = ConfigurationManager.AppSettings["UpdateEmployeeById"];
                string apiURL = string.Format(UpdateEmployeeById);
                //string apiURL = string.Format("https://localhost:44385/api/Employee/");
                string json = JsonConvert.SerializeObject(requestBody);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "PUT";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + _token);
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                //string result = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    responseString = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                bool result = JsonConvert.DeserializeObject<bool>(responseString);
                if (result)
                {
                    MessageBox.Show($"Employee updated sucessfully");
                    ClearFields();
                    dataEmployees.DataSource = null;
                }
                else
                {
                    MessageBox.Show($"Employee not updated with id:  {txtId.Text}");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearFields();
            }
        }

        public void ClearFields()
        {
            txtSearchBox.Text = string.Empty;
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSex.Text = string.Empty;
            txtMovieName.Text = string.Empty;
            txtMovieRating.Text = string.Empty;
            dtDOB.Value = DateTime.Now;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Please select an item!");
                    return;
                }
                var responseString = "";
                //object requestBody = new
                //{
                //    id = txtId.Text,
                //    name = txtName.Text,
                //    dob = dtDOB.Value.ToString(),
                //    sex = txtSex.Text,
                //    movieName = txtMovieName.Text,
                //    movieRating = txtMovieRating.Text
                //};
                string DeleteEmployeeById = ConfigurationManager.AppSettings["DeleteEmployeeById"];
                string apiURL = string.Format(DeleteEmployeeById + txtId.Text);
                //string apiURL = string.Format("https://localhost:44385/api/Employee/" + txtId.Text);
                //string json = JsonConvert.SerializeObject(requestBody);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "DELETE";
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + _token);
                request.ContentType = "application/json";

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                //{
                //    streamWriter.Write(json);
                //}
                httpWebResponse = (HttpWebResponse)request.GetResponse();
                //string result = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    responseString = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                bool result = JsonConvert.DeserializeObject<bool>(responseString);
                if (result)
                {
                    MessageBox.Show($"Employee Deleted sucessfully");
                    ClearFields();
                    dataEmployees.DataSource = null;
                }
                else
                {
                    MessageBox.Show($"Employee not deleted with id:  {txtId.Text}");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearFields();
            }
        }
    }
}
