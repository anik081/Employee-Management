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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("User name is recuired!");
                    return;
                } 
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Password recuired!");
                    txtPassword.Text = string.Empty;
                    return;
                }
                var responseString = "";
                object requestBody = new
                {
                    email = txtUserName.Text,
                    password = txtPassword.Text
                };
                string LoginUrl = ConfigurationManager.AppSettings["LoginUrl"];
                string apiURL = string.Format(LoginUrl);
                //string apiURL = string.Format("https://localhost:44385/api/Accounts/login");
                string json = JsonConvert.SerializeObject(requestBody);
                WebRequest request = WebRequest.Create(apiURL);
                HttpWebResponse httpWebResponse = null;
                request.Method = "POST";
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
                AccountToken accountToken = JsonConvert.DeserializeObject<AccountToken>(responseString);
                if (!string.IsNullOrEmpty(accountToken.Token))
                {
                    ClearFields();
                    Home home = new Home(accountToken);
                    home.Show();
                    this.Hide();
                }
                else
                {
                    ClearFields();
                    lblErrorMessage.Visible = true;
                }
                
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
                ClearFields();
            }
            
        }

        private void lblSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignupForm signupForm = new SignupForm();
            signupForm.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
        public void ClearFields()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Focus();
        }
    }
}
