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
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void lblGoToLoginClick_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Name is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Email is required!");
                    return;
                }
                if (string.IsNullOrEmpty(dtDOB.Value.ToString()))
                {
                    MessageBox.Show("Date of birth is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtSex.Text))
                {
                    MessageBox.Show("Sex is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Password is required!");
                    return;
                }
                if (string.IsNullOrEmpty(txtConPassword.Text))
                {
                    MessageBox.Show("Password confirmation is required!");
                    return;
                }
                if(txtConPassword.Text != txtPassword.Text)
                {
                    MessageBox.Show("Passwords did not match");
                    txtPassword.Text = string.Empty;
                    txtConPassword.Text = string.Empty;
                    return;
                }
                object requestBody = new
                {
                    name = txtName.Text,
                    email = txtEmail.Text,
                    dob = dtDOB.Value.ToString(),
                    sex = txtSex.Text,
                    password = txtPassword.Text,
                    confirmPassword = txtConPassword.Text
                };
                string SignupUrl = ConfigurationManager.AppSettings["SignupUrl"];
                string apiURL = string.Format(SignupUrl);
                //string apiURL = string.Format("https://localhost:44385/api/Accounts/signup");
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
                string result = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                if (!string.IsNullOrEmpty(result))
                {
                    ClearFields();
                    MessageBox.Show("Signup successful");
                    LoginForm login = new LoginForm();
                    login.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Signup failed!");
                    ClearFields();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SignupForm_Load(object sender, EventArgs e)
        {

        }

        public void ClearFields()
        {
            txtName.Text = string.Empty;
            txtSex.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConPassword.Text = string.Empty;
            dtDOB.Value = DateTime.Now;
        }
    }
}
