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
    public partial class Home : Form
    {
        public AccountToken _Account { get; set; }
        public AccountModel objAccount { get; set; }
        public Home(AccountToken account)
        {
            InitializeComponent();
            _Account = account;
             GetAccountByEmail(_Account.Email);
        }

        
        private void GetAccountByEmail(string Email)
        {
            try
            {
                var responseString = "";
                string GetAccountByEmailUrl = ConfigurationManager.AppSettings["GetAccountByEmailUrl"];
                string apiURL = string.Format(GetAccountByEmailUrl + Email);
                //string apiURL = string.Format("https://localhost:44385/api/Accounts/"+Email);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "GET";
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
                AccountModel accountModel = JsonConvert.DeserializeObject<AccountModel>(responseString);
                lblName.Text = accountModel.Name;
                lblEmail.Text = accountModel.Email;
                lblSex.Text = accountModel.Sex;
                lblDOB.Text = accountModel.DOB;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void getAllEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllEmployeesForm allEmployeesForm = new AllEmployeesForm(_Account.Token);
            allEmployeesForm.Show();
        }

        private void saveEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee(_Account.Token);
            addEmployee.Show();
        }

        private void findEmployeeByMovieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeFrom employeeFrom = new EmployeeFrom(_Account.Token);
            employeeFrom.Show();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
