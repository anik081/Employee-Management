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
    public partial class AddEmployee : Form
    {
        string _token = string.Empty;
        public AddEmployee(string token)
        {
            InitializeComponent();
            _token = token;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFileds();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Name is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtSex.Text))
                {
                    MessageBox.Show("Sex is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtMovieName.Text))
                {
                    MessageBox.Show("Movie name is required!");
                    return;
                }
                if (string.IsNullOrEmpty(dtDOB.Value.ToString()))
                {
                    MessageBox.Show("Date of birth is required!");
                    return;
                }
                var responseString = "";
                object requestBody = new
                {
                    Name = txtName.Text,
                    DOB = dtDOB.Value.ToString(),
                    Sex = txtSex.Text,
                    MovieName = txtMovieName.Text
                };
                string employeeSaveUrl = ConfigurationManager.AppSettings["EmployeeSaveUrl"];
                //string apiURL = string.Format("https://localhost:44385/api/Employee/");
                string apiURL = string.Format(employeeSaveUrl);
                string json = JsonConvert.SerializeObject(requestBody);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "POST";
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
                int id = JsonConvert.DeserializeObject<int>(responseString);
                if (id != 0)
                {
                    MessageBox.Show($"Employee save successfully with id:{id}");
                    ClearFileds();
                }
                else
                {
                    MessageBox.Show("Could not save employee");
                    ClearFileds();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearFileds();
            }
           
        }

        public void ClearFileds()
        {
            txtName.Text = string.Empty;
            txtSex.Text = string.Empty;
            txtMovieName.Text = string.Empty;
            dtDOB.Value = DateTime.Now;
        }
    }
}
