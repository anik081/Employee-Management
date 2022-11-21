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
    public partial class AllEmployeesForm : Form
    {
        string _token = string.Empty;
        public AllEmployeesForm(string token)
        {
            InitializeComponent();
            _token = token;
            FetchEmployees();
        }
        public void FetchEmployees() {

            try
            {
                var responseString = "";
                string GetAllEmployeeUrl = ConfigurationManager.AppSettings["GetAllEmployeeUrl"];
                string apiURL = string.Format(GetAllEmployeeUrl);
                //string apiURL = string.Format("https://localhost:44385/api/Employee/");
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
                    dataGridView1.DataSource = employeeList;
                }
                else
                {
                    MessageBox.Show($"No employee found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AllEmployeesForm_Load(object sender, EventArgs e)
        {

        }
    }
}
